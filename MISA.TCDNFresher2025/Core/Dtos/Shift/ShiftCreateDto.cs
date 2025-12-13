using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos.Shift
{
    /// <summary>
    /// DTO dùng để tạo mới một ca làm việc (Shift) trong hệ thống.
    /// Chứa thông tin cơ bản về ca làm việc, giờ bắt đầu/kết thúc, thời gian nghỉ, 
    /// tổng giờ làm việc và trạng thái hoạt động.
    /// </summary>
    /// Created By: hiepnd - 12/2025
    public class ShiftCreateDto
    {
        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan BeginShiftTime { get; set; }

        public TimeSpan EndShiftTime { get; set; }

        public TimeSpan BeginBreakTime { get; set; }

        public TimeSpan EndBreakTime { get; set; }

        public double WorkingTime { get; set; }

        public double BreakingTime { get; set; }
        public string Description { get; set; }

        public bool Inactive { get; set; }

      
    }
}
