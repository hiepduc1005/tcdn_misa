using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    public class FilterItem
    {
        public String Column { get; set; }

        public object Value { get; set; }

        public string Operator { get; set; }

    }
}
