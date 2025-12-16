using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{
    /// <summary>
    /// Attribute dùng để khai báo thông tin cột tương ứng trong database
    /// cho một property của entity.
    /// </summary>
    /// <remarks>
    /// - Name  : Tên cột trong database
    /// - Label : Tên hiển thị (ví dụ: tiêu đề cột Excel, label trên UI)
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Tên cột tương ứng trong database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên hiển thị của cột (dùng cho UI, Excel, v.v).
        /// </summary>
        public string Label { get; set; }

        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
