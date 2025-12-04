import { Component, OnInit, ViewChild, ChangeDetectorRef, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common'; 
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent, MatPaginator } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HistoryService, History } from '../services/history.service';
import { Subject, Subscription } from 'rxjs'; 
import { debounceTime } from 'rxjs/operators';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-histories',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    DatePipe
  ],
  templateUrl: './histories.component.html',
  styleUrls: ['./histories.component.css'],
})
export class HistoriesComponent implements OnInit, AfterViewInit, OnDestroy { 
  allColumns: string[] = ['id','text','fullName','dateTime','eventType'];
  displayedColumns: string[] = this.allColumns.filter(c => c !== 'id');
  dataSource: History[] = [];
  totalCount = 0;
  pageSize = 10;
  currentPage = 1;
  startDate?: Date;
  endDate?: Date;
  textFilter = '';
  userFilter = '';
  eventTypeFilter = '';

  sortColumns: { column: string; direction: 'asc' | 'desc' }[] = [
    { column: 'dateTime', direction: 'desc' }
  ];

  columnWidths: Record<string, number> = {
    'id': 100,
    'text': 250,
    'fullName': 200,
    'dateTime': 180,
    'eventType': 150,
  };
  private resizingColumn: string | null = null;
  private startX = 0;
  private startWidth = 0;
  private resizeLine!: HTMLElement;
  private globalMouseMoveListener!: (e: MouseEvent) => void;
  private globalMouseUpListener!: (e: MouseEvent) => void;
  private subscriptions = new Subscription(); 

  private filterSubject = new Subject<{text:string, user:string, event:string}>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('root', { read: ElementRef }) rootRef!: ElementRef<HTMLElement>; 

  constructor(private historyService: HistoryService, private cdr: ChangeDetectorRef) {
    this.globalMouseMoveListener = this.onResize.bind(this);
    this.globalMouseUpListener = this.onResizeEnd.bind(this);
  }

  ngOnInit() {
    this.subscriptions.add(
      this.filterSubject.pipe(debounceTime(500)).subscribe(filters => {
        this.currentPage = 1;
        this.loadData(filters.text, filters.user, filters.event);
      })
    );

    this.loadData(this.textFilter, this.userFilter, this.eventTypeFilter);
  }

  ngAfterViewInit() {
    const resizeLineEl = document.getElementById('resize-line');
    if (resizeLineEl) {
      this.resizeLine = resizeLineEl;
    }

    document.addEventListener('mousemove', this.globalMouseMoveListener);
    document.addEventListener('mouseup', this.globalMouseUpListener);
  }

  ngOnDestroy() {
    document.removeEventListener('mousemove', this.globalMouseMoveListener);
    document.removeEventListener('mouseup', this.globalMouseUpListener);
    this.subscriptions.unsubscribe();
  }

onFilterChange() {
  // если конечная дата меньше начальной, сбрасываем её
  if (this.startDate && this.endDate && this.endDate < this.startDate) {
    this.endDate = undefined;
  }

  this.currentPage = 1;
  if (this.paginator) this.paginator.firstPage();

  this.filterSubject.next({
    text: this.textFilter,
    user: this.userFilter,
    event: this.eventTypeFilter
  });
}


  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  loadData(text: string, user: string, event: string) {
    const activeSort = this.sortColumns.filter(s => this.displayedColumns.includes(s.column));
    const start = this.startDate ? this.formatDate(this.startDate) : undefined;
    const end = this.endDate ? this.formatDate(this.endDate) : undefined;
    console.log('%cЗАПРОС К СЕРВЕРУ', 'color:blue; font-size:14px; font-weight:bold;');
    console.log(' currentPage:', this.currentPage);
    console.log(' pageSize:', this.pageSize);
    console.log(' sort:', activeSort.map(s => `${s.column} ${s.direction}`).join(', '));
    console.log(' textFilter:', text);
    console.log(' userFilter:', user);
    console.log(' eventTypeFilter:', event);
    console.log(' startDate:', start);
    console.log(' endDate:', end);
    
    this.historyService.getAllServerSide(
      this.currentPage,
      this.pageSize,
      activeSort.map(s => `${s.column} ${s.direction}`).join(', '),
      text,
      user,
      event,
      start,
      end
    ).subscribe((res: any) => {

      console.log('%cОТВЕТ ОТ СЕРВЕРА', 'color:green; font-size:14px; font-weight:bold;');
      console.log('histories:', res.histories);
      console.log('totalCount:', res.totalCount);

      setTimeout(() => {
        this.dataSource = res.histories || [];
        this.totalCount = res.totalCount || 0;
        this.cdr.detectChanges();
      });
    });
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.cdr.detectChanges(); 
    this.loadData(this.textFilter, this.userFilter, this.eventTypeFilter);
  }
  sortBy(column: string) {
    if (this.resizingColumn) return; 
    
    const existing = this.sortColumns.find(s => s.column === column);

    if (existing) {
      existing.direction = existing.direction === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumns.push({ column, direction: 'asc' });
    }

    console.log('Текущая сортировка:', this.sortColumns.map(s => `${s.column} ${s.direction}`).join(', '));
    this.loadData(this.textFilter, this.userFilter, this.eventTypeFilter);
  }

  getSortQuery(): string {
    return this.sortColumns
      .map(s => s.column + ' ' + s.direction)
      .join(', ');
  }

  getSortIndicator(column: string): string {
    if (!this.displayedColumns.includes(column)) return '—';
    const s = this.sortColumns.find(x => x.column === column);
    if (!s) return '—';
    return s.direction === 'asc' ? '▲' : '▼';
  }
  removeSortColumn(column: string) {
    this.sortColumns = this.sortColumns.filter(s => s.column !== column);
    this.loadData(this.textFilter, this.userFilter, this.eventTypeFilter);
  }
  isSorted(column: string): boolean {
    return this.displayedColumns.includes(column) && !!this.sortColumns.find(s => s.column === column);
  }
  
  toggleColumn(column: string) {
    if (this.displayedColumns.includes(column)) {
      this.displayedColumns = this.displayedColumns.filter(c => c !== column);
      this.removeSortColumn(column);
    } else {
      const index = this.allColumns.indexOf(column);
      this.displayedColumns = [
        ...this.displayedColumns.slice(0, index),
        column,
        ...this.displayedColumns.slice(index)
      ].filter((c, i, arr) => arr.indexOf(c) === i);
    }
    this.cdr.detectChanges();
  }
  
  onResizeStart(event: MouseEvent, column: string) {
    const thElement = (event.currentTarget as HTMLElement).closest('th');
    if (!thElement) return;

    const rect = thElement.getBoundingClientRect();
    const isNearBoundary = rect.right - event.clientX < 15;

    if (!isNearBoundary) {
      return;
    }

    this.resizingColumn = column;
    this.startX = event.clientX;
    this.startWidth = this.columnWidths[column] || 100;
    
    this.resizeLine.style.display = 'block';
    const rootRect = this.rootRef.nativeElement.getBoundingClientRect();
    this.resizeLine.style.left = `${event.clientX - rootRect.left + this.rootRef.nativeElement.scrollLeft}px`;

    document.body.classList.add('table-resizing');
    event.preventDefault(); 
  }


onResize(event: MouseEvent) {
    if (!this.resizingColumn) return;

    const MIN_COLUMN_WIDTH = 60;
    const SENSITIVITY_FACTOR = 1.0; 

    const rawDelta = event.clientX - this.startX;
    
    const delta = rawDelta / SENSITIVITY_FACTOR; 
    
    let newWidth = this.startWidth + delta; 

    if (newWidth < MIN_COLUMN_WIDTH) {
      newWidth = MIN_COLUMN_WIDTH;
    }

    this.columnWidths[this.resizingColumn] = newWidth;
    
    const rootRect = this.rootRef.nativeElement.getBoundingClientRect();
    this.resizeLine.style.left = `${event.clientX - rootRect.left + this.rootRef.nativeElement.scrollLeft}px`;
}

  onResizeEnd() {
    if (this.resizingColumn) {
      this.resizeLine.style.display = 'none';
      this.resizingColumn = null;
      document.body.classList.remove('table-resizing');
      this.cdr.detectChanges();
    }
  }

  setResizeCursor(event: MouseEvent, column: string): void {
    if (this.resizingColumn) return; 

    const thElement = event.currentTarget as HTMLElement;
    const rect = thElement.getBoundingClientRect();
    
    const isNearBoundary = rect.right - event.clientX < 15 && rect.right - event.clientX > 0;
    
    thElement.style.cursor = isNearBoundary ? 'col-resize' : 'pointer'; 
  }

  
}

