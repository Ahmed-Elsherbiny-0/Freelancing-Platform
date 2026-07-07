using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Helpers
{
    public class Pagination<T>(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
    {
        public int PageSize { get; set; } = pageSize;
        public int PageIndex { get; set; } = pageIndex;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Data { get; set; } = data;
    }
}