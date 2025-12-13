using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Shift
{
    /// <summary>s
    /// DTO dùng để trả về thông tin chi tiết của một ca làm việc (Shift) trong hệ thống.
    /// Chứa các thông tin cơ bản như mã ca, tên ca, giờ bắt đầu/kết thúc, thời gian nghỉ,
    /// tổng giờ làm việc và trạng thái hoạt động.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ShiftResponseDto
    {
        public Guid ShiftId { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan BeginShiftTime { get; set; }

        public TimeSpan EndShiftTime { get; set; }

        public TimeSpan BeginBreakTime { get; set; }

        public TimeSpan EndBreakTime { get; set; }


        public double WorkingTime { get; set; }

        public double BreakingTime { get; set; }

        public bool Inactive { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
