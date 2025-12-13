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
    
    [MISATable("shift", Label = "Ca làm việc")]    
    public class Shift
    {
        [MISAPrimaryKey]
        [MISAExportIgnore]
        [ColumnName("shift_id",Label = "Id ca làm việc")]
        public Guid ShiftId { get; set; }

        [ColumnName("shift_code", Label = "Mã ca làm việc")]
        public string ShiftCode { get; set; }

        [ColumnName("shift_name", Label = "Tên ca làm việc")] 
        public string ShiftName { get; set; }

        [ColumnName("begin_shift_time", Label = "Giờ vào ca")]
        public TimeSpan BeginShiftTime { get; set; }

        [ColumnName("end_shift_time", Label = "Giờ hết ca")]
        public TimeSpan EndShiftTime { get; set; }

        [ColumnName("begin_break_time", Label = "Bắt đầu nghỉ giữa ca")]
        public TimeSpan BeginBreakTime { get; set; }

        [ColumnName("end_break_time", Label = "Kết thúc nghỉ giữa ca")]
        public TimeSpan EndBreakTime { get; set; }

        [ColumnName("working_time", Label = "Thời gian làm việc (giờ)")]
        public double WorkingTime { get; set; }

        [ColumnName("breaking_time", Label = "Thời gian nghỉ giữa ca (giờ)")]
        public double BreakingTime { get; set; }

        [ColumnName("inactive", Label = "Trạng thái")]
        public bool Inactive { get; set; }

        [MISAExportIgnore]
        [ColumnName("description", Label = "Mô tả")]
        public string Description { get; set; }

        [MISAExportIgnore]
        [ColumnName("created_by", Label = "Người tạo")]
        public string CreatedBy { get; set; }

        [MISAExportIgnore]
        [ColumnName("created_date", Label = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [MISAExportIgnore]
        [ColumnName("modified_by", Label = "Người sửa")]
        public string ModifiedBy { get; set; }

        [MISAExportIgnore]
        [ColumnName("modified_date", Label = "Ngày sửa")]
        public DateTime ModifiedDate { get; set; }

    }
}
