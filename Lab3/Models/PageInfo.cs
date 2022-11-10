namespace Lab3.Models
{
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public PageInfo(int curr, int count, int size)
        {
            CurrentPage = curr;
            PageCount = count;
            PageSize = size;
        }
    }
}
