using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MISA.Core.Enums
{
    /// <summary>
    /// Enum xác định chiều sắp xếp dữ liệu.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        ASC,
        DESC
    }
}
