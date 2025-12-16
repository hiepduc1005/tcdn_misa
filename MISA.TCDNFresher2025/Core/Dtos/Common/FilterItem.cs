using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    /// <summary>
    /// Lớp mô tả một điều kiện lọc dữ liệu.
    /// Dùng để xây dựng các điều kiện filter động khi truy vấn dữ liệu.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class FilterItem
    {
        /// <summary>
        /// Tên cột / trường dữ liệu cần áp dụng điều kiện lọc.
        /// </summary>
        public String Column { get; set; }

        /// <summary>
        /// Giá trị dùng để so sánh khi lọc dữ liệu.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Toán tử so sánh dùng cho điều kiện lọc.
        /// </summary>
        public string Operator { get; set; }

    }
}
