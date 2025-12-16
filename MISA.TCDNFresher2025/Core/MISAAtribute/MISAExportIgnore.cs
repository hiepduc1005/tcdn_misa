using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{
    /// <summary>
    /// Attribute dùng để đánh dấu các property
    /// KHÔNG được xuất ra file Excel.
    /// </summary>
    /// <remarks>
    /// Áp dụng cho các property không cần hiển thị khi export dữ liệu
    /// (ví dụ: Id, khóa ngoại, cờ nội bộ, metadata...).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAExportIgnore : Attribute
    {
    }
}
