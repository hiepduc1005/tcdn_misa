using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Dtos.Common;
using MISA.Core.Dtos.Shift;
using MISA.Core.Interfaces.Services;

namespace MISA.Api.Controllers
{
    /// <summary>
    /// API quản lý ca làm việc (Shift)
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }


        /// <summary>
        /// Lấy tất cả ca làm việc.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _shiftService.GetAllShifts();

            return Ok(ResponseResult.Success(
                data: data,
                message: "Lấy danh sách ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Lấy chi tiết ca làm việc theo Id.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var data = _shiftService.GetShiftById(id);

            return Ok(ResponseResult.Success(
                data: data,
                message: "Lấy thông tin ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        [HttpPost("datapaging")]
        public IActionResult DataPaging([FromBody] PagingRequest pagingRequest)
        {
            var data = _shiftService.DataPaging(pagingRequest);

            return StatusCode(StatusCodes.Status200OK, ResponseResult.Success(
                data: data,
                message: "Lấy dữ liệu thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Tạo mới ca làm việc.
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] ShiftCreateDto dto)
        {
            var data = _shiftService.CreateShift(dto);

            return StatusCode(StatusCodes.Status201Created, ResponseResult.Success(
                data: data,
                message: "Tạo mới ca làm việc thành công.",
                status: StatusCodes.Status201Created
            ));
        }

        [HttpPost("inactive")]
        public IActionResult InactiveShift([FromBody] List<Guid> shiftIds)
        {
            _shiftService.InactiveShifts(shiftIds);

            return StatusCode(StatusCodes.Status200OK, ResponseResult.Success(
                data: shiftIds,
                message: "Ngừng sử dụng danh sách ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        [HttpPost("active")]
        public IActionResult ActiveShift([FromBody] List<Guid> shiftIds)
        {
            _shiftService.ActiveShifts(shiftIds);

            return StatusCode(StatusCodes.Status200OK, ResponseResult.Success(
                data: shiftIds,
                message: "Sử dụng danh sách ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Cập nhật ca làm việc.
        /// </summary>
        [HttpPut]
        public IActionResult Update( [FromBody] ShiftUpdateDto dto)
        {

            var data = _shiftService.UpdateShift(dto);

            return Ok(ResponseResult.Success(
                data: data,
                message: "Cập nhật ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Xóa ca làm việc theo Id.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _shiftService.DeleteShift(id);

            return Ok(ResponseResult.Success(
                data: null,
                message: "Xóa ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Xóa ca làm việc theo danh sách Id.
        /// </summary>
        [HttpPost("delete")]
        public IActionResult DeleteShifts(List<Guid> shiftIds)
        {
            _shiftService.DeleteShifts(shiftIds);

            return Ok(ResponseResult.Success(
                data: null,
                message: "Xóa ca làm việc thành công.",
                status: StatusCodes.Status200OK
            ));
        }

        /// <summary>
        /// Xuất khẩu danh sách ca làm việc ra Excel theo điều kiện lọc.
        /// </summary>
        /// <param name="pagingRequest">Các tham số lọc, sắp xếp (giống hệt API lấy dữ liệu phân trang)</param>
        /// <returns>File Excel (.xlsx)</returns>
        [HttpPost("export-excel")]
        public IActionResult ExportExcel([FromBody] PagingRequest pagingRequest)
        {
            var excelFileBytes = _shiftService.ExportExcel(pagingRequest);

            // Đặt tên file (Nên kèm timestamp để file không bị trùng tên khi tải nhiều lần)
            string fileName = $"CaLamViec_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

            // Định nghĩa loại file (MIME type cho Excel .xlsx)
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Frontend sẽ nhận được Blob và browser tự động tải xuống
            return File(excelFileBytes, contentType, fileName);
        }
    }
}
