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


        /// <summary>
        /// Lấy tên label của column trong database ứng với một property.
        /// - Nếu property có gán <see cref="ColumnNameAttribute"/> thì dùng tên trong attribute.
        /// - Nếu không có attribute thì trả về tên property.
        /// </summary>
        /// <param name="property">Property cần lấy tên cột.</param>
        /// <returns>Tên label column trong DB hoặc tên property nếu không có attribute.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public static string GetColumnLabel(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<ColumnNameAttribute>();
            return attr?.Label ?? property.Name;
            // Nếu không có Label thì dùng tên field
        }

        /// <summary>
        /// Lấy tên cột trong database tương ứng với tên field (property) của entity.
        /// </summary>
        /// <typeparam name="T">Kiểu entity.</typeparam>
        /// <param name="fieldName">Tên property của entity.</param>
        /// <returns>Tên cột trong database.</returns>
        /// <exception cref="Exception">
        /// Ném ra exception nếu không tìm thấy property tương ứng trong entity.
        /// </exception>
        public static string GetColumnNameFromFieldName<T>(string fieldName)
        {
            // Lấy PropertyInfo từ tên field
            var prop = typeof(T).GetProperty(fieldName);
            if (prop == null)
                throw new Exception($"Không tìm thấy property '{fieldName}' trong {typeof(T).Name}");

            // Dùng lại helper lấy tên cột
            return ReflectionHelper.GetColumnName(prop);
        }

        /// <summary>
        /// Lấy label hiển thị của cột tương ứng với tên field (property) của entity.
        /// </summary>
        /// <typeparam name="T">Kiểu entity.</typeparam>
        /// <param name="fieldName">Tên property của entity.</param>
        /// <returns>Label hiển thị của cột.</returns>
        /// <exception cref="Exception">
        /// Ném ra exception nếu không tìm thấy property tương ứng trong entity.
        /// </exception>
        public static string GetColumnLabelFromFieldName<T>(string fieldName)
        {
            // Lấy PropertyInfo từ tên field
            var prop = typeof(T).GetProperty(fieldName);
            if (prop == null)
                throw new Exception($"Không tìm thấy property '{fieldName}' trong {typeof(T).Name}");

            // Dùng lại helper lấy tên label của cột
            return ReflectionHelper.GetColumnLabel(prop);
        }

        /// <summary>
        /// Lấy tên bảng trong database của entity.
        /// Ưu tiên lấy từ attribute <see cref="MISATable"/>,
        /// nếu không có thì sử dụng tên class (chuyển về chữ thường).
        /// </summary>
        /// <typeparam name="T">Kiểu entity.</typeparam>
        /// <returns>Tên bảng trong database.</returns>
        public static string GetTableName<T>()
        {
            var tableAttr = typeof(T).GetCustomAttributes(typeof(MISATable), false).FirstOrDefault() as MISATable;
            return tableAttr != null ? tableAttr.Name : typeof(T).Name.ToLower();
        }

        /// <summary>
        /// Lấy label hiển thị của bảng.
        /// Ưu tiên lấy từ attribute <see cref="MISATable"/>,
        /// nếu không có thì sử dụng tên class làm label.
        /// </summary>
        /// <typeparam name="T">Kiểu entity.</typeparam>
        /// <returns>Label hiển thị của bảng.</returns>
        public static string GetTableLabel<T>()
        {
            var tableAttr = typeof(T).GetCustomAttributes(typeof(MISATable), false).FirstOrDefault() as MISATable;

            // Neu nhu k co label thi de ten class lam label
            return tableAttr != null ? tableAttr.Label : typeof(T).Name;
            
        }
    }
}
