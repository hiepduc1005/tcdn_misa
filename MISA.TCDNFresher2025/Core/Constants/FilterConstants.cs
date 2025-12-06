using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Constants
{
    public static class FilterConstants
    {
        // Kiểm tra trống
        public const string IsEmpty = "empty";
        public const string IsNotEmpty = "not_empty";

        // So sánh bằng / khác
        public const string Equal = "eq";               // Bằng
        public const string NotEqual = "neq";           // Khác

        // Chuỗi
        public const string Contains = "contains";               // Chứa
        public const string NotContains = "not_contains";       // Không chứa
        public const string StartsWith = "starts_with";         // Bắt đầu với
        public const string EndsWith = "ends_with";             // Kết thúc với

        // Số / ngày
        public const string LessThan = "lt";                    // <
        public const string LessThanOrEqual = "lte";            // <=
        public const string GreaterThan = "gt";                 // >
        public const string GreaterThanOrEqual = "gte";         // >=
    }
}
