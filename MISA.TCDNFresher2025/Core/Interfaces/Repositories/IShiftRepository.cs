using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface repository quản lý các thao tác dữ liệu liên quan đến ca làm việc (Shift).
    /// Kế thừa IBaseRepository để sử dụng các phương thức CRUD chung.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public interface IShiftRepository : IBaseRepository<Shift>
    {
        bool CheckShiftCodeExists(string shiftCode);
    }
}
