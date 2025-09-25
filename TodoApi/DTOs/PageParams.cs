namespace TodoApi.DTOs
{
    public class PageParams
    {
        public int Offset { get; set; }
        public int PageSize { get; set; }
        public int SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string SearchTerm { get; set; } = "";
    }

    public class PagedResult<T>
    {
        public long TotalRows { get; set; }
        public IEnumerable<T> PageData { get; set; }
    }
}
