using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Exceptions
{
    /// <summary>
    /// Exception tùy chỉnh dùng để xử lý các lỗi validate dữ liệu trong hệ thống.
    /// Chứa danh sách các lỗi chi tiết theo dạng key-value để trả về cho client.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
   
    public class MISAValidateException : Exception
    {
        IDictionary _data = new Dictionary<string, string>();

        private string? _message = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại thông tin.";
        public MISAValidateException(IDictionary data)
        {
            this._data = data;
        }

        public override string Message => this._message;

        public override IDictionary Data => this._data;
    }
}
