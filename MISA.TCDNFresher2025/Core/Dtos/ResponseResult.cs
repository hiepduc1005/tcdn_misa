using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Dtos
{
    /// <summary>
    /// Lớp chuẩn hóa dữ liệu trả về cho client.
    /// Dùng để thống nhất format response của API (thành công hoặc thất bại).
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class ResponseResult
    {
        /// <summary>
        /// Mã trạng thái HTTP (200, 400, 404, 500...).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Thông báo dành cho client (Success, Error, Validate failed...).
        /// </summary>
        /// 
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về (có thể là object, list, dic hoặc null nếu có lỗi).
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Danh sách lỗi chi tiết (Dic,list,obj).
        /// Null nếu không có lỗi.
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// Tạo object ResponseResult thành công.
        /// </summary>
        public static ResponseResult Success(int status,object? data = null, string message = "Success")
        {
            return new ResponseResult
            {
                StatusCode = status,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        /// <summary>
        /// Tạo object ResponseResult khi xảy ra lỗi.
        /// </summary>
        public static ResponseResult Fail(int statusCode, string message, object? errors = null)
        {
            return new ResponseResult
            {
                StatusCode = statusCode,
                Message = message,
                Data = null,
                Errors = errors
            };
        }
    }
}
