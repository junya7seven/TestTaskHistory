using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Data.Managers.Filters
{
    public static class HistoryQueryFiltersExtensions
    {

        /// <summary>
        /// фильтры поиска
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<History> ApplyFilters(
    this IQueryable<History> query, HistoryQueryFilters filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.TextFilter))
                query = query.Where(h => h.Text.Contains(filters.TextFilter));

            if (!string.IsNullOrWhiteSpace(filters.UserFilter))
                query = query.Where(h => h.User.FullName.Contains(filters.UserFilter));

            if (!string.IsNullOrWhiteSpace(filters.EventTypeFilter))
                query = query.Where(h => h.EventType.Name.Contains(filters.EventTypeFilter));


            if (filters.StartDate.HasValue)
            {
                var startDateValue = filters.StartDate.Value.Date;
                var startDateUtc = DateTime.SpecifyKind(startDateValue, DateTimeKind.Utc);
                query = query.Where(h => h.DateTime >= startDateUtc);
            }

            if (filters.EndDate.HasValue)
            {
                var endDateValue = filters.EndDate.Value.Date;
                var endDateUtc = DateTime.SpecifyKind(endDateValue, DateTimeKind.Utc);
                query = query.Where(h => h.DateTime < endDateUtc.AddDays(1));
            }


            return query;
        }


        private static readonly Dictionary<string, string> SortingMap = new()
        {
            { "id", "Id" },
            { "text", "Text" },
            { "dateTime", "DateTime" },
            { "eventType", "EventType.Name" },
            { "fullName", "User.FullName" }
        };

        /// <summary>
        /// маппер входящей строки для Linq.Dynamic
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private static string MapOrderString(this string orderBy)
        {

            var orderList = orderBy.Split(",")
                .Select(o => o.Trim())
                .ToList();

            var preparedParts = orderList.Select(x =>
            {
                var items = x.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var column = items[0];
                var dir = items.Length > 1 ? items[1] : "asc";

                if (SortingMap.TryGetValue(column, out var mapped))
                {
                    return $"{mapped} {dir}";
                }
                return x;
            });

            return string.Join(", ", preparedParts);
        }

        /// <summary>
        /// фильтр сортировки
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<History> ApplySorting(
            this IQueryable<History> query, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return query.OrderByDescending(h => h.DateTime);
            }

            try
            {
                return query.OrderBy(orderBy.MapOrderString());
            }
            catch
            {
                return query.OrderByDescending(h => h.DateTime);
            }
        }


        /// <summary>
        /// фильтр паггинации
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<History> ApplyPaging(
            this IQueryable<History> query, int page, int pageSize)
        {
            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
