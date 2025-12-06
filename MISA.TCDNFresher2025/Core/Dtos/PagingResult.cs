using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalRecords / PageSize);


        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public PagingResult()
        {
            
        }

        public PagingResult(IEnumerable<T> data, int totalRecords, int pageSize, int currentPage)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }
    }
}
