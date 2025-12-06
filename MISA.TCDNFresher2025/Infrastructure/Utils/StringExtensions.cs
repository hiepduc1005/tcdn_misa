using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utils
{
    /// <summary>
    /// Lớp mở rộng (extension class) cho kiểu dữ liệu string/>.
    /// Chứa các phương thức tiện ích để thao tác với chuỗi, 
    /// ví dụ như chuyển đổi, kiểm tra, format, v.v.
    /// </summary>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public static class StringExtensions
    {

        /// <summary>
        /// Hàm chuyển chuỗi sang dạng sanke_case cho convert từ thuộc tính class sang tên trường trong database
        /// </summary>
        /// <param name="input">Chuỗi cần được chuyển sang snake_case</param>
        /// <returns>Chuỗi có dạng snake_case</returns>
        /// Created By: hiepnd - 12/2025
        public static string ToSnakeCase(this string input)
        {
            var sb = new StringBuilder();

            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if(i > 0 && Char.IsUpper(c))
                {
                    sb.Append('_');
                }

                sb.Append(Char.ToLower(c));
            }

            return sb.ToString();
        }
    }
}
