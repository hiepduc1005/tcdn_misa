using Core.Entities;
using MISA.Core.Dtos.Common;
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
        /// <summary>
        /// Kiểm tra mã ca làm việc đã tồn tại trong hệ thống hay chưa.
        /// </summary>
        /// <param name="shiftCode">Mã ca làm việc cần kiểm tra.</param>
        /// <returns>
        /// true nếu mã ca đã tồn tại; ngược lại false.
        /// </returns>
        bool CheckShiftCodeExists(string shiftCode);

        /// <summary>
        /// Kích hoạt (sử dụng) danh sách ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id ca làm việc.</param>
        /// <returns>
        /// Số bản ghi được cập nhật.
        /// </returns>
        int ActivateShifts(List<Guid> shiftIds);

        /// <summary>
        /// Ngừng sử dụng danh sách ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id ca làm việc.</param>
        /// <returns>
        /// Số bản ghi được cập nhật.
        /// </returns>
        int InactivateShifts(List<Guid> shiftIds);

        /// <summary>
        /// Xóa danh sách ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id ca làm việc.</param>
        /// <returns>
        /// Số bản ghi bị xóa.
        /// </returns>
        int DeleteShifts(List<Guid> shiftIds);

        /// <summary>
        /// Tìm kiếm ca làm việc theo từ khóa.
        /// </summary>
        /// <param name="keyword">
        /// Từ khóa tìm kiếm (mã ca, tên ca hoặc mô tả).
        /// </param>
        /// <returns>
        /// Danh sách ca làm việc thỏa mãn điều kiện tìm kiếm.
        /// </returns>
        List<Shift> SearchShifts(string keyword);
    }
}
