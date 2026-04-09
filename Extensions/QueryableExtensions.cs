using CodeElevate.DTOs;
using System.Linq;

namespace CodeElevate.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedResult<T> Paginate<T>(
           this IQueryable<T> query,
           int page,
           int pageSize)
        {
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<T>
            {
                Data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNext = page < totalPages,
                HasPrevious = page > 1
            };
        }

    }
}
