using MISA.Core.Constants;
using MISA.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infrastructure.Utils
{
    public static class MISASqlMapper
    {
        public static (string SqlClause, object? Value) MapOperatorToSql(string filterOperator, string columnName, object value, string paramName)
        {
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
                FilterConstants.IsEmpty => ($"({columnName} IS NULL OR {paramName} = '')", null),
                FilterConstants.IsNotEmpty => ($"({columnName} IS NOT NULL AND {paramName} != '')", null),
                _ => throw new Exception($"Không hỗ trợ operator: {filterOperator}")
            };
        }

        public static string MapSortToSql(SortDirection sort, string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name không được rỗng.", nameof(columnName));

            var direction = sort.ToString().ToUpper();

            return $"{columnName} {direction}";
        }
    }
}
