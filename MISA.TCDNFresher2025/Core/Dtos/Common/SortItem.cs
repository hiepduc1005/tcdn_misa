using MISA.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    public class SortItem
    {
        public string FieldName { get; set; }

        public SortDirection Direction { get; set; } // ASC hoặc DESC
    }
}
