using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.MISAAtribute
{
    /// <summary>
    /// Attribute dùng để đánh dấu property
    /// là khóa chính (Primary Key) của entity.
    /// </summary>
    /// <remarks>
    /// Được sử dụng trong các xử lý generic như:
    /// - Xác định khóa chính khi CRUD
    /// - Mapping dữ liệu với Dapper
    /// - Sinh câu SQL động (WHERE, UPDATE, DELETE)
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAPrimaryKey : Attribute
    {
    }
}
