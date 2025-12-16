using MISA.Core.Constants;
using MISA.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infrastructure.Utils
{
    /// <summary>
    /// Lớp mapper dùng để chuyển đổi điều kiện lọc và sắp xếp
    /// sang các mệnh đề SQL tương ứng.
    /// Phục vụ xây dựng câu truy vấn động (dynamic SQL).
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public static class MISASqlMapper
    {
        /// <summary>
        /// Chuyển đổi toán tử lọc sang mệnh đề SQL tương ứng.
        /// </summary>
        /// <param name="filterOperator">Toán tử lọc (theo FilterConstants).</param>
        /// <param name="columnName">Tên cột trong database.</param>
        /// <param name="value">Giá trị dùng để so sánh.</param>
        /// <param name="paramName">Tên parameter dùng cho câu lệnh SQL.</param>
        /// <returns>
        /// Tuple gồm:
        /// <list type="bullet">
        /// <item><description>SqlClause: Mệnh đề SQL tương ứng.</description></item>
        /// <item><description>Value: Giá trị parameter (có thể null).</description></item>
        /// </list>
        /// </returns>
        /// <exception cref="Exception">
        /// Ném ra exception nếu operator không được hỗ trợ.
        /// </exception>
        public static (string SqlClause, object? Value) MapOperatorToSql(string filterOperator, string columnName, object value, string paramName)
        {
            // Trường hợp filter trường Inactive
            if (columnName.Equals("inactive")) 
            {
               
                return ($"{columnName} = @{paramName}",  Boolean.Parse(filterOperator));
            }
            
            return filterOperator switch
            { 
                FilterConstants.Equal => ($"{columnName} = @{paramName}", value),
                FilterConstants.NotEqual => ($"{columnName} <> @{paramName}", value),
                FilterConstants.Contains => ($"{columnName} LIKE CONCAT('%', @{paramName}, '%')", value),
                FilterConstants.NotContains => ($"{columnName} NOT LIKE CONCAT('%', @{paramName}, '%')", value),
                FilterConstants.StartsWith => ($"{columnName} LIKE CONCAT(@{paramName}, '%')", value),  
                FilterConstants.EndsWith => ($"{columnName} LIKE CONCAT('%', @{paramName})", value),
                FilterConstants.LessThan => ($"{columnName} < @{paramName}", value),
                FilterConstants.LessThanOrEqual => ($"{columnName} <= @{paramName}", value),
                FilterConstants.GreaterThan => ($"{columnName} > @{paramName}", value),
                FilterConstants.GreaterThanOrEqual => ($"{columnName} >= @{paramName}", value),
                FilterConstants.IsEmpty => ($"({columnName} IS NULL OR {columnName} = '')", null),
                FilterConstants.IsNotEmpty => ($"({columnName} IS NOT NULL AND {columnName} != '')", null),
                _ => throw new Exception($"Không hỗ trợ operator: {filterOperator}")
            };
        }

        // <summary>
        /// Chuyển đổi thông tin sắp xếp sang mệnh đề ORDER BY trong SQL.
        /// </summary>
        /// <param name="sort">Chiều sắp xếp (ASC hoặc DESC).</param>
        /// <param name="columnName">Tên cột trong database.</param>
        /// <returns>Mệnh đề ORDER BY tương ứng.</returns>
        /// <exception cref="ArgumentException">
        /// Ném ra exception nếu tên cột không hợp lệ.
        /// </exception>
        public static string MapSortToSql(SortDirection sort, string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name không được rỗng.", nameof(columnName));
            
            
            var direction = sort.ToString().ToUpper();

            return $"{columnName} {direction}";
        }
    } 
}
