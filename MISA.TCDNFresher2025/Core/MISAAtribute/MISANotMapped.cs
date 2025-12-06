using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{
    /// <summary>
    /// Attribute dùng để đánh dấu một property của entity **không được ánh xạ** vào cơ sở dữ liệu.
    /// Thường được sử dụng trong các lớp entity để loại bỏ một số trường khi thao tác CRUD tự động.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    /// 
    [AttributeUsage(AttributeTargets.Property)]
    public class MISANotMapped : Attribute
    {
        public string ErrorMessage { get; set; }
        public MISANotMapped(string error)
        {
            this.ErrorMessage = error;
        }
    }
}
