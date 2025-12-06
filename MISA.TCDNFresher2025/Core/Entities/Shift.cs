using MISA.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    /// <summary>
    /// Entity đại diện cho một ca làm việc (Shift) trong hệ thống.
    /// Chứa các thông tin cơ bản về ca làm việc bao gồm mã, tên, giờ bắt đầu/kết thúc,
    /// thời gian nghỉ, tổng giờ làm việc, thời gian nghỉ, và trạng thái hoạt động.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    
    [MISATable("shift")]    
    public class Shift
    {
        [MISAPrimaryKey]
        [ColumnName("shift_id")]
        public Guid ShiftId { get; set; }

        [ColumnName("shift_code")]
        public string ShiftCode { get; set; }

        [ColumnName("shift_name")]
        public string ShiftName { get; set; }

        [ColumnName("begin_shift_time")]
        public TimeSpan BeginShiftTime { get; set; }

        [ColumnName("end_shift_time")]
        public TimeSpan EndShiftTime { get; set; }

        [ColumnName("begin_break_time")]
        public TimeSpan BeginBreakTime { get; set; }

        [ColumnName("end_break_time")]
        public TimeSpan EndBreakTime { get; set; }

        [ColumnName("working_time")]
        public double WorkingTime { get; set; }

        [ColumnName("breaking_time")]
        public double BreakingTime { get; set; }

        [ColumnName("inactive")]
        public bool Inactive { get; set; }

        [ColumnName("created_by")]
        public string CreatedBy { get; set; }

        [ColumnName("created_date")]
        public DateTime CreatedDate { get; set; }

        [ColumnName("modified_by")]
        public string ModifiedBy { get; set; }

        [ColumnName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}
