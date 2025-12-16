using MISA.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    /// <summary>
    /// Lớp mô tả thông tin sắp xếp dữ liệu.
    /// Dùng để xác định trường cần sắp xếp và chiều sắp xếp (tăng dần hoặc giảm dần).
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class SortItem
    {
        /// <summary>
        /// Tên trường dùng để sắp xếp.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Chiều sắp xếp dữ liệu (ASC hoặc DESC).
        /// </summary>
        public SortDirection Direction { get; set; } // ASC hoặc DESC
    }
}
