using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos
{
    public class PagingRequest
    {

        public int PageSize { get; set; }

        public int PageIndex { get; set; }  

        public List<FilterItem> filterItems  { get; set; } = new List<FilterItem>();

        // Danh sách các điều kiện lọc
        public List<SortItem> sortItems { get; set; } = new List<SortItem>();
    }
}
