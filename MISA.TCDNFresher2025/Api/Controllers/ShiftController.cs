using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Dtos;
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
                status: 200
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
                status: 200
            ));
        }

        /// <summary>
        /// Tạo mới ca làm việc.
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] ShiftCreateDto dto)
        {
            var data = _shiftService.CreateShift(dto);

            return StatusCode(201, ResponseResult.Success(
                data: data,
                message: "Tạo mới ca làm việc thành công.",
                status: 201
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
                status: 200
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
                status: 200
            ));
        }
    }
}
