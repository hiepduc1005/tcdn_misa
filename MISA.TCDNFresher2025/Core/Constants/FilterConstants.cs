using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Constants
{
    /// <summary>
    /// Tập hợp các hằng số định nghĩa toán tử lọc dữ liệu (Filter Operator).
    /// </summary>
    /// <remarks>
    /// - Được sử dụng để mapping filter từ client → backend
    /// - Áp dụng trong việc sinh câu SQL động (WHERE clause)
    /// - Kết hợp với FilterItem và MISASqlMapper
    /// 
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public static class FilterConstants
    {
        /// <summary>
        /// Giá trị rỗng hoặc NULL.
        /// </summary>
        public const string IsEmpty = "empty";

        /// <summary>
        /// Giá trị không rỗng và không NULL.
        /// </summary>
        public const string IsNotEmpty = "not_empty";

        /// <summary>
        /// Bằng (=).
        /// </summary>
        public const string Equal = "eq";               // Bằng


        /// <summary>
        /// Khác (<>).
        /// </summary>
        public const string NotEqual = "neq";           // Khác

        /// <summary>
        /// Chứa chuỗi (LIKE %value%).
        /// </summary>
        public const string Contains = "contains";               // Chứa

        /// <summary>
        /// Không chứa chuỗi (NOT LIKE %value%).
        /// </summary>
        public const string NotContains = "not_contains";       // Không chứa

        /// <summary>
        /// Bắt đầu bằng chuỗi (LIKE value%).
        /// </summary>
        public const string StartsWith = "starts_with";         // Bắt đầu với

        /// <summary>
        /// Kết thúc bằng chuỗi (LIKE %value).
        /// </summary>
        public const string EndsWith = "ends_with";             // Kết thúc với

        /// <summary>
        /// Nhỏ hơn (&lt;).
        /// </summary>
        public const string LessThan = "lt";                    // <

        /// <summary>
        /// Nhỏ hơn hoặc bằng (&lt;=).
        /// </summary>
        public const string LessThanOrEqual = "lte";            // <=

        /// <summary>
        /// Lớn hơn (&gt;).
        /// </summary>
        public const string GreaterThan = "gt";                 // >

        /// <summary>
        /// Lớn hơn hoặc bằng (&gt;=).
        /// </summary>
        public const string GreaterThanOrEqual = "gte";         // >=
    }
}
