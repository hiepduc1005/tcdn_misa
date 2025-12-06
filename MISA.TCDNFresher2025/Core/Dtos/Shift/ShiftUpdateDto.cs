using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Shift
{
    /// <summary>
    /// DTO dùng để cập nhật thông tin một ca làm việc (Shift) trong hệ thống.
    /// Chứa các thông tin cơ bản của ca làm việc, giờ bắt đầu/kết thúc, thời gian nghỉ,
    /// tổng giờ làm việc, trạng thái hoạt động, và Id của ca cần cập nhật.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ShiftUpdateDto
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
    }
}
