using Infrastructure.Utils;
using MISA.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.Infrastructure.Utils
{
    /// <summary>
    /// Helper sử dụng Reflection để lấy ra tên cột (Column) 
    /// dựa trên Attribute được gán cho property.
    /// Cho phép tách biệt tên Property trong C# và tên cột trong Database.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ReflectionHelper
    {
        /// <summary>
        /// Lấy tên column trong database ứng với một property.
        /// - Nếu property có gán <see cref="ColumnNameAttribute"/> thì dùng tên trong attribute.
        /// - Nếu không có attribute thì trả về tên property.
        /// </summary>
        /// <param name="property">Property cần lấy tên cột.</param>
        /// <returns>Tên column trong DB hoặc tên property nếu không có attribute.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public static string GetColumnName(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<ColumnNameAttribute>();
            return attr?.Name ?? property.Name.ToSnakeCase();
            // Nếu không có attribute thì dùng tên property
        }

        public static string GetColumnNameFromFieldName<T>(string fieldName)
        {
            // Lấy PropertyInfo từ tên field
            var prop = typeof(T).GetProperty(fieldName);
            if (prop == null)
                throw new Exception($"Không tìm thấy property '{fieldName}' trong {typeof(T).Name}");

            // Dùng lại helper lấy tên cột
            return ReflectionHelper.GetColumnName(prop);
        }


    }
}
