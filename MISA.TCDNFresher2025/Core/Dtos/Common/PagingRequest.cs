using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    /// <summary>
    /// DTO dùng để chứa thông tin phân trang và sắp xếp khi truy vấn dữ liệu.
    /// Bao gồm các thuộc tính PageSize, PageIndex, danh sách điều kiện lọc (filterItems)
    /// và danh sách điều kiện sắp xếp (sortItems).
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    
    public class PagingRequest
    {

        /// <summary>
        /// Số bản ghi trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Chỉ số trang hiện tại (bắt đầu từ 1)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Danh sách các điều kiện lọc dữ liệu
        /// </summary>
        public List<FilterItem> FilterItems  { get; set; } = new List<FilterItem>();


        /// <summary>
        /// Điều kiện lọc dành cho search
        /// </summary>
        public List<FilterItem> CustomFilters { get; set; } = new List<FilterItem>();


        /// <summary>
        /// Danh sách các điều kiện sắp xếp dữ liệu
        /// </summary>
        public List<SortItem> SortItems { get; set; } = new List<SortItem>();

    }
}
