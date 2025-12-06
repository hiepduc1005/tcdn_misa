using MISA.Core.Dtos.Common;
using MISA.Core.Dtos.Shift;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Interfaces.Services
{
    /// <summary>
    /// Interface service quản lý các nghiệp vụ liên quan đến ca làm việc (Shift).
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public interface IShiftService 
    {

        /// <summary>
        /// Lấy danh sách tất cả các ca làm việc.
        /// </summary>
        /// <returns>Danh sách các ca làm việc.</returns>
        List<ShiftResponseDto> GetAllShifts();

        /// <summary>
        /// Lấy chi tiết ca làm việc theo Id.
        /// </summary>
        /// <param name="shiftId">Id của ca làm việc cần lấy.</param>
        /// <returns>Thông tin chi tiết của ca làm việc.</returns>
        ShiftResponseDto GetShiftById(Guid shiftId);

        /// <summary>
        /// Lấy danh sách ca làm việc theo phân trang, kèm điều kiện lọc và sắp xếp.
        /// </summary>
        /// <param name="pagingRequest">Đối tượng PagingRequest chứa thông tin pageIndex, pageSize, filters và sorts.</param>
        /// <returns>Đối tượng PagingResult chứa dữ liệu ca làm việc phân trang.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        PagingResult<ShiftResponseDto> DataPaging(PagingRequest pagingRequest);

        /// <summary>
        /// Thêm mới một ca làm việc.
        /// </summary>
        /// <param name="shiftCreateDto">Thông tin ca làm việc cần tạo.</param>
        /// <returns>Id của ca làm việc mới tạo.</returns>
        ShiftResponseDto CreateShift(ShiftCreateDto shiftCreateDto);

        /// <summary>
        /// Cập nhật thông tin ca làm việc.
        /// </summary>
        /// <param name="shiftUpdateDto">Thông tin ca làm việc cần cập nhật.</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng (thường là 1 nếu thành công).</returns>
        ShiftResponseDto UpdateShift(ShiftUpdateDto shiftUpdateDto);


        /// <summary>
        /// Xóa một ca làm việc theo Id.
        /// </summary>
        /// <param name="shiftId">Id của ca làm việc cần xóa.</param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng (thường là 1 nếu thành công).</returns>
        void DeleteShift(Guid shiftId);


        /// <summary>
        /// Kiểm tra xem mã ca làm việc đã tồn tại hay chưa.
        /// </summary>
        /// <param name="shiftCode">Mã ca làm việc cần kiểm tra.</param>
        /// <returns>true nếu tồn tại, false nếu chưa tồn tại.</returns>
        bool IsShiftCodeExists(string shiftCode);
    }
}
