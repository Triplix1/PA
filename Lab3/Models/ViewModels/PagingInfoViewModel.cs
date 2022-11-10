using System.Collections.Generic;

namespace Lab3.Models.ViewModels
{
    public class PagingInfoViewModel
    {
        public IEnumerable<Row> Rows { get; set; }
        public PageInfo PageInfo { get; set; }
        public PagingInfoViewModel(IEnumerable<Row> rows, PageInfo Page)
        {
            Rows = rows;
            PageInfo = Page;
        }
    }
}
