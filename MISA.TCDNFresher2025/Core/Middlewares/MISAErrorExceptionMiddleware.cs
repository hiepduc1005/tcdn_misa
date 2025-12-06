using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MISA.Core.Dtos.Common;
using MISA.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
namespace MISA.Core.Middlewares
{
    // <summary>
    /// Middleware dùng để bắt và xử lý các exception trong hệ thống.
    /// Tự động trả về JSON theo chuẩn ResponseResult.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class MISAErrorExceptionMiddleware
    {
        /// <summary>
        /// Đại diện cho middleware kế tiếp.
        /// </summary>
        private readonly RequestDelegate _delegate;

        /// <summary>
        /// Hàm khởi tạo middleware.
        /// @delegate dùng để tránh trùng từ khóa "delegate" của C#.
        /// </summary>
        public MISAErrorExceptionMiddleware(RequestDelegate @delegate)
        {
            this._delegate = @delegate;
        }




        /// <summary>
        /// Hàm xử lý logic chính của middleware.
        /// Tự động bắt lỗi Validate và lỗi hệ thống.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _delegate(context);
            }
            // Bắt lỗi validate
            catch (MISAValidateException ex)
            {
                // Log
                Console.WriteLine("Validate Error: " + ex.Message);

                // Trả JSON về client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = ResponseResult.Fail(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ex.Message,
                        errors: ex.Data
                    );

                // Convert thành json
                var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(responseJson);
            }
            // Bắt lỗi server
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Error: " + ex.Message);

                // Trả JSON về client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ResponseResult.Fail(
                    statusCode: StatusCodes.Status500InternalServerError,
                    message: "Lỗi hệ thống! Vui lòng thử lại sau.",
                    errors: ex.Message
                );

                // Convert thành json
                var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(responseJson);
            }
        }
    }

    

    public static class MISAMiddlewareExtensions
    {
        /// <summary>
        /// Hàm mở rộng giúp đăng ký middleware vào pipeline.
        /// </summary>
        public static IApplicationBuilder UseMISAExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MISAErrorExceptionMiddleware>();
        }
    }

}
