using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{

    /// <summary>
    /// Attribute để đánh dấu tên bảng của một entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MISATable : Attribute
    {
        /// <summary>
        /// Tên bảng trong database
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Tên bảng được hiển thị trên UI
        /// </summary>
        public string Label { get; set; }

        public MISATable(string name)
        {
            Name = name;
        }
    }
}
