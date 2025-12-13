using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Common
{
    public class FilterItem
    {
        public String Column { get; set; }

        public string Value { get; set; }

        public string Operator { get; set; }

    }
}
