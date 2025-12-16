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

        /// <summary>
        /// Cập nhật trạng thái inactive = 0 cho danh sách ca làm việc.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của ca làm việc.</param>
        /// <returns>Số bản ghi được cập nhật.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public int ActivateShifts(List<Guid> shiftIds)
        {
            if (shiftIds == null || shiftIds.Count == 0)
                return 0;

            string sql = $"UPDATE shift SET inactive = 0 , modified_date = NOW() , modified_by = 'Admin' WHERE shift_id IN @ShiftIds";
             
            var affectedRows = dbConnection.Execute(sql, new { ShiftIds = shiftIds });

            return affectedRows;
        }

        /// <summary>
        /// Cập nhật trạng thái inactive = 1 cho danh sách ca làm việc.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của ca làm việc.</param>
        /// <returns>Số bản ghi được cập nhật.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public int InactivateShifts(List<Guid> shiftIds)
        {
            if (shiftIds == null || shiftIds.Count == 0)
                return 0;

            string sql = $"UPDATE shift SET inactive = 1 , modified_date = NOW() , modified_by = 'Admin' WHERE shift_id IN @ShiftIds";

            var affectedRows = dbConnection.Execute(sql, new { ShiftIds = shiftIds });

            return affectedRows;
        }


        /// <summary>
        /// Xóa nhiều ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của các ca làm việc cần xóa.</param>
        /// <returns>
        /// Số lượng bản ghi bị ảnh hưởng (số ca làm việc đã bị xóa).
        /// </returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public int DeleteShifts(List<Guid> shiftIds)
        {
            if (shiftIds == null || shiftIds.Count == 0)
                return 0;

            string sql = "DELETE FROM shift WHERE shift_id IN @ShiftIds";

            var affectedRows = dbConnection.Execute(sql, new { ShiftIds = shiftIds });

            return affectedRows;
        }

        /// <summary>
        /// Tìm kiếm danh sách ca làm việc theo từ khóa.
        /// </summary>
        /// <param name="keyword">
        /// Từ khóa tìm kiếm.
        /// Áp dụng tìm kiếm theo mã ca, tên ca hoặc mô tả.
        /// </param>
        /// <returns>
        /// Danh sách ca làm việc thỏa mãn điều kiện tìm kiếm.
        /// </returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public List<Shift> SearchShifts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                string sqlAll = "SELECT * FROM shift";
                return dbConnection.Query<Shift>(sqlAll).ToList();
            }
            // Chuẩn hóa keyword
            keyword = $"%{keyword.Trim()}%";

            string sql = @"
                SELECT *
                FROM shift
                WHERE shift_code LIKE @Keyword
                   OR shift_name LIKE @Keyword
                   OR description LIKE @Keyword;
            ";

            var shifts = dbConnection.Query<Shift>(sql, new { Keyword = keyword });

            return shifts.ToList();
        }
    }
}
