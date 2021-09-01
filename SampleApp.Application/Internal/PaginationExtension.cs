using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Application.Internal
{
    public static class PaginationExtension
    {
        public static async Task<PaginatedResult<T>> Paginate<T>(this IQueryable<T> source,
            int pageSize, int pageNumber)
        {
            return await new PaginatedResult<T>(pageNumber, pageSize).PaginateAsync(source);
        }
    }

    public class PaginatedResult<T>
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;
        private const int DefaultPageValue = default;

        public int Total { get; private set; }
        public int Limit { get; private set; }
        public int Page { get; private set; }
        public List<T> Data { get; private set; }

        internal PaginatedResult(int pageNumber, int pageSize = DefaultPageSize)
        {
            Limit = pageSize;
            Page = pageNumber;

            if (Limit < DefaultPageValue || Limit > MaxPageSize)
            {
                Limit = DefaultPageSize;
            }
            if (pageNumber < DefaultPageValue)
            {
                Page = DefaultPageValue;
            }
        }

        internal async Task<PaginatedResult<T>> PaginateAsync(IQueryable<T> queryable)
        {
            Total = queryable.Count();

            if (Limit > Total)
            {
                Limit = Total;
                Page = DefaultPageValue;
            }

            var skip = Page * Limit;

            if (skip + Limit > Total)
            {
                skip = Total - Limit;
                Page = Total / Limit - 1;
            }

            Data = queryable.Skip(skip).Take(Limit).ToList();
            await Task.CompletedTask;

            return this;
        }
    }
}