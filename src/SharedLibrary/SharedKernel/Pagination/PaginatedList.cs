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
