using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
