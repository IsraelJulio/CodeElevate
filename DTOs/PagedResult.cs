using Domain.Entities;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeElevate.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

    }
}
