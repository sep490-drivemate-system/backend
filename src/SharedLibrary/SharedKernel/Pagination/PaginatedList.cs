using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Pagination
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyCollection<T> PageContent { get; set; } = new List<T>();

        public int PageCount => PageSize != 0 ? (int)Math.Ceiling((float)TotalCount / PageSize) : 0;
        public bool HasNextPage => CurrentPage < PageCount;
        public bool HasPreviousPage => CurrentPage > 1;

        public int FirstItemIndex => (CurrentPage - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(FirstItemIndex + PageContent.Count - 1, TotalCount);

        public bool IsEmpty => !PageContent.Any();
        public bool IsFull => PageContent.Count == PageSize;

        public static PaginatedList<T> Create<T>(List<T> source, int page, int pageSize)
        {
            var count = source.Count;
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedList<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = count,
                PageContent = items
            };
        }

        public static PaginatedList<T> Create<T>(IEnumerable<T> source, int page, int pageSize)
        {
            var list = source.ToList();
            return Create(list, page, pageSize);
        }

        // Từ IQueryable<T> (bất đồng bộ – EF Core)
        public static async Task<PaginatedList<T>> CreateAsync<T>(
            IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = count,
                PageContent = items
            };
        }
    }
}
