// src/app/app.ts

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoriesComponent } from './histories/histories.component'; 

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HistoriesComponent],
  template: `
    <main>
      <app-histories></app-histories>
    </main>
  `,
})
export class AppComponent {}
