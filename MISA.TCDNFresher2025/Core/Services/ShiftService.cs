using Core.Entities;
using Core.Interfaces.Repositories;
using MISA.Core.Dtos.Common;
using MISA.Core.Dtos.Shift;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Services
{
    /// <summary>
    /// Service quản lý các nghiệp vụ liên quan đến ca làm việc (Shift).
    /// Triển khai IShiftService để thực hiện các phương thức CRUD 
    /// và kiểm tra nghiệp vụ đặc thù của ca làm việc.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ShiftService : BaseService<Shift> , IShiftService 
    {
        IShiftRepository _shiftRepository;

        public ShiftService(IShiftRepository shiftRepository, IExcelExporterService excelExporterService) : base(shiftRepository, excelExporterService)
        {
            this._shiftRepository = shiftRepository;
            
        }

        /// <summary>
        /// Tạo mới một ca làm việc.
        /// </summary>
        /// <param name="shiftCreateDto">Thông tin ca làm việc cần tạo.</param>
        /// <returns>Thông tin ca làm việc vừa tạo.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public ShiftResponseDto CreateShift(ShiftCreateDto shiftCreateDto)
        {
            // Map dữ liệu DTO sang Entity
            Shift shift = AutoMapperService<ShiftCreateDto, Shift>.Map(shiftCreateDto);
            var errors = new Dictionary<string, string>();

            // Validate dữ liệu 

            //Không được để trống mã ca
            if (string.IsNullOrWhiteSpace(shift.ShiftCode))
            {
                errors.Add("ShiftCode", "Mã ca không được để trống. ");
            }
            // Mã ca không được quá 20 ký tự
            else if (shift.ShiftCode.Length > 20)
            {
                errors.Add("ShiftCode", "Mã ca không được quá 20 ký tự. ");
            }
            // Mã ca không được trùng
            else if (IsShiftCodeExists(shift.ShiftCode))
            {
                errors.Add("ShiftCode", "Mã ca đã tồn tại trong hệ thống. ");
            }

            // Không được để trống tên ca
            if (string.IsNullOrWhiteSpace(shift.ShiftName))
            {
                errors.Add("ShiftName", "Tên ca không được để trống. ");
            }
            // Tên ca không được quá 50 ký tự
            else if (shift.ShiftName.Length > 50)
            {
                errors.Add("ShiftName", "Tên ca không được quá 50 ký tự. ");
            }

            // Giờ vào ca không được trống
            if(shift.BeginShiftTime == null || shift.BeginShiftTime == TimeSpan.Zero)
            {
                errors.Add("BeginShiftTime", "Giờ vào ca không được để trống. ");
            }

            // Giờ hết ca không được trống
            if (shift.EndShiftTime == null || shift.EndShiftTime == TimeSpan.Zero)
            {
                errors.Add("EndShiftTime", "Giờ hết ca không được để trống. ");
            }

            // Throw ra exception nếu như có lỗi 
            if(errors.Count > 0)
            {
                throw new MISAValidateException(errors);
            }

            // Gán id
            shift.ShiftId = Guid.NewGuid();
            
            shift.CreatedDate = DateTime.Now;
            shift.CreatedBy = "Admin";

            var res = _shiftRepository.Insert(shift);

            // Map từ Entity sang DTO
            ShiftResponseDto shiftResponse = AutoMapperService<Shift, ShiftResponseDto>.Map(res);

            return shiftResponse;
        }

        /// <summary>
        /// Xóa một ca làm việc theo Id.
        /// </summary>
        /// <param name="shiftId">Id của ca làm việc cần xóa.</param>
        /// <returns>true nếu xóa thành công, false nếu thất bại.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public void DeleteShift(Guid shiftId)
        {
            // Nếu như không có shift theo id thì sẽ throw ra lỗi 
            GetShiftById(shiftId);

            _shiftRepository.Delete(shiftId);
        }


        /// <summary>
        /// Lấy danh sách tất cả các ca làm việc.
        /// </summary>
        /// <returns>Danh sách ca làm việc.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public List<ShiftResponseDto> GetAllShifts()
        {
            // Lấy tất cả bản ghi Shift trong DB
            var shiftList = _shiftRepository.GetAll();

            // Map sang danh sách DTO
            var shiftResponseList = shiftList
                .Select(shift => AutoMapperService<Shift, ShiftResponseDto>.Map(shift))
                .ToList();

            return shiftResponseList;
        }

        /// <summary>
        /// Lấy danh sách ca làm việc theo phân trang, kèm điều kiện lọc và sắp xếp.
        /// </summary>
        /// <param name="pagingRequest">Đối tượng PagingRequest chứa thông tin pageIndex, pageSize, filters và sorts.</param>
        /// <returns>Đối tượng PagingResult chứa dữ liệu ca làm việc phân trang.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public PagingResult<ShiftResponseDto> DataPaging(PagingRequest pagingRequest)
        {
            PagingResult<Shift> pagingResult = _shiftRepository.getDataPaging(
                pagingRequest.PageIndex,
                pagingRequest.PageSize,
                pagingRequest.FilterItems,
                pagingRequest.CustomFilters,
                pagingRequest.SortItems
            );

            var shiftResponseList = pagingResult.DataPaging
                .Select(shift => AutoMapperService<Shift, ShiftResponseDto>.Map(shift))
                .ToList();

            return new PagingResult<ShiftResponseDto>
            {
                CurrentPage = pagingResult.CurrentPage,
                PageSize = pagingResult.PageSize,
                TotalRecords = pagingResult.TotalRecords,

                DataPaging = shiftResponseList // Gán danh sách DTO đã map
            };
        }


        /// <summary>
        /// Lấy chi tiết ca làm việc theo Id.
        /// </summary>
        /// <param name="shiftId">Id của ca làm việc cần lấy.</param>
        /// <returns>Thông tin chi tiết ca làm việc.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public ShiftResponseDto GetShiftById(Guid shiftId)
        {
            Shift shift = _shiftRepository.GetById(shiftId);
            
            // Nếu như không có shift trả về
            if( shift == null)
            {
                var errors = new Dictionary<string, string>();
                errors.Add("ShiftId", $"Không tồn tại ca làm việc có mã {shiftId}");

                throw new MISAValidateException(errors);
            }

            // Map từ Entity sang DTO
            ShiftResponseDto shiftResponse = AutoMapperService<Shift, ShiftResponseDto>.Map(shift);
            return shiftResponse;
        }


        /// <summary>
        /// Kiểm tra xem mã ca làm việc đã tồn tại hay chưa.
        /// </summary>
        /// <param name="shiftCode">Mã ca làm việc cần kiểm tra.</param>
        /// <returns>true nếu tồn tại, false nếu chưa tồn tại.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public bool IsShiftCodeExists(string shiftCode)
        {
            return _shiftRepository.CheckShiftCodeExists(shiftCode);
        }

        /// <summary>
        /// Cập nhật thông tin một ca làm việc.
        /// </summary>
        /// <param name="shiftUpdateDto">Thông tin ca làm việc cần cập nhật.</param>
        /// <returns>Thông tin ca làm việc sau khi cập nhật.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public ShiftResponseDto UpdateShift(ShiftUpdateDto shiftUpdateDto)
        {
            // Map dữ liệu DTO sang Entity
            Shift shift = AutoMapperService<ShiftUpdateDto, Shift>.Map(shiftUpdateDto);
            var errors = new Dictionary<string, string>();

            // Validate dữ liệu 

            // Lấy bản ghi hiện tại
            var existing = _shiftRepository.GetById(shiftUpdateDto.ShiftId);
            if (existing == null)
            {
                throw new MISAValidateException(new Dictionary<string, string>
                {
                    { "ShiftId", "Không tồn tại ca làm việc cần cập nhật." }
                });
            }

            //Không được để trống mã ca
            if (string.IsNullOrWhiteSpace(shift.ShiftCode))
            {
                errors.Add("ShiftCode", "Mã ca không được để trống. ");
            }
            // Mã ca không được quá 20 ký tự
            else if (shift.ShiftCode.Length > 20)
            {
                errors.Add("ShiftCode", "Mã ca không được quá 20 ký tự. ");
            }
            // Kiểm tra trùng mã ca nếu như mã ca hiện tại và mã ca đã sửa khác nhau
            else if (!shift.ShiftCode.Equals(existing.ShiftCode) && IsShiftCodeExists(shift.ShiftCode))
            {
                errors.Add("ShiftCode", "Mã ca đã tồn tại trong hệ thống.");
            }


            // Không được để trống tên ca
            if (string.IsNullOrWhiteSpace(shift.ShiftName))
            {
                errors.Add("ShiftName", "Tên ca không được để trống. ");
            }
            // Tên ca không được quá 50 ký tự
            else if (shift.ShiftName.Length > 50)
            {
                errors.Add("ShiftName", "Tên ca không được quá 50 ký tự. ");
            }

            // Giờ vào ca không được trống
            if (shift.BeginShiftTime == null || shift.BeginShiftTime == TimeSpan.Zero)
            {
                errors.Add("BeginShiftTime", "Giờ vào ca không được để trống. ");
            }

            // Giờ hết ca không được trống
            if (shift.EndShiftTime == null || shift.EndShiftTime == TimeSpan.Zero)
            {
                errors.Add("EndShiftTime", "Giờ hết ca không được để trống. ");
            }
          

            // Throw ra exception nếu như có lỗi 
            if (errors.Count > 0)
            {
                throw new MISAValidateException(errors);
            }

           

            // Ngày sửa
            shift.ModifiedDate = DateTime.Now;

            shift.CreatedBy = existing.CreatedBy;
            shift.CreatedDate = existing.CreatedDate;
            // Fake admin
            shift.ModifiedBy = "Admin Modify";


            var res = _shiftRepository.Update(shift);

            ShiftResponseDto shiftResponse = AutoMapperService<Shift, ShiftResponseDto>.Map(res);
            return shiftResponse;
        }

        /// <summary>
        /// Ngừng sử dụng các ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của các ca làm việc cần ngừng sử dụng.</param>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public void InactiveShifts(List<Guid> shiftIds)
        {
            _shiftRepository.InactivateShifts(shiftIds);
        }

        /// <summary>
        /// Kích hoạt (sử dụng lại) các ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của các ca làm việc cần kích hoạt.</param>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public void ActiveShifts(List<Guid> shiftIds)
        {
            _shiftRepository.ActivateShifts(shiftIds);
        }

        /// <summary>
        /// Xóa các ca làm việc theo danh sách Id.
        /// </summary>
        /// <param name="shiftIds">Danh sách Id của các ca làm việc cần xóa.</param>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        public void DeleteShifts(List<Guid> shiftIds){
            _shiftRepository.DeleteShifts(shiftIds);
        }

       
    }


}
