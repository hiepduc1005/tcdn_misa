using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    /// <summary>
    /// Lớp bao đóng kết quả phân trang dữ liệu.
    /// Dùng để trả về danh sách bản ghi kèm theo thông tin phân trang cho client.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của phần tử trong danh sách.</typeparam>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class PagingResult<T>
    {
        /// <summary>
        /// Danh sách dữ liệu của trang hiện tại.
        /// </summary>
        public IEnumerable<T> DataPaging { get; set; }

        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện (không phân trang).
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Tổng số trang.
        /// Được tính dựa trên TotalRecords và PageSize.
        /// </summary>
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>
        /// Số bản ghi trên một trang.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Trang hiện tại (bắt đầu từ 1).
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Khởi tạo đối tượng PagingResult mặc định.
        /// </summary>
        public PagingResult()
        {
            
        }

        /// <summary>
        /// Khởi tạo đối tượng PagingResult với đầy đủ thông tin phân trang.
        /// </summary>
        /// <param name="data">Danh sách dữ liệu của trang hiện tại.</param>
        /// <param name="totalRecords">Tổng số bản ghi.</param>
        /// <param name="pageSize">Số bản ghi trên một trang.</param>
        /// <param name="currentPage">Trang hiện tại.</param>
        public PagingResult(IEnumerable<T> data, int totalRecords, int pageSize, int currentPage)
        {
            DataPaging = data;
            TotalRecords = totalRecords;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }
    }
}
