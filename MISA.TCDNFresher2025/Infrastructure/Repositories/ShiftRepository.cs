using Core.Entities;
using Core.Interfaces.Repositories;
using Dapper;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Infrastructure.Repositories
{
    /// <summary>
    /// Repository quản lý các thao tác dữ liệu liên quan đến ca làm việc (Shift).
    /// Kế thừa <see cref="BaseRepository{Shift}"/> để sử dụng các phương thức CRUD chung
    /// và triển khai <see cref="IShiftRepository"/> để định nghĩa các phương thức đặc thù của Shift.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Kiểm tra xem mã ca làm việc (<c>ShiftCode</c>) đã tồn tại trong hệ thống hay chưa.
        /// </summary>
        /// <param name="shiftCode">Mã ca cần kiểm tra.</param>
        /// <returns>
        /// <c>true</c> nếu mã ca tồn tại trong bảng; 
        /// ngược lại trả về <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public bool CheckShiftCodeExists(string shiftCode)
        {
            string sql = "SELECT COUNT(*) FROM shift WHERE shift_code = @ShiftCode";
            var result = dbConnection.ExecuteScalar<int>(sql, new { ShiftCode = shiftCode });

            return result > 0;
        }
    }
}
