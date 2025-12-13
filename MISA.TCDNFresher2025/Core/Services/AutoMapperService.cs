using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.Core.Services
{
    /// <summary>
    ///  Static Class dùng để map dữ liệu giữa hai kiểu đối tượng.
    /// Thường dùng để chuyển đổi giữa DTO và Entity, hoặc giữa các model khác nhau.
    /// </summary>
    /// <typeparam name="T">Kiểu nguồn (source type) cần map từ đó.</typeparam>
    /// <typeparam name="Y">Kiểu đích (destination type) cần map sang.</typeparam>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public static class AutoMapperService<T,Y> 
        where T : class
        where Y : new() 
    {

        /// <summary>
        /// Map dữ liệu từ đối tượng source kiểu T sang đối tượng kiểu Y
        /// </summary>
        /// <param name="source">Đối tượng nguồn</param>
        /// <returns>Đối tượng mới kiểu Y với dữ liệu copy từ source</returns>
        public static Y Map(T source) 
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Y destination = new Y(); // tạo instance Y mới

            // Lấy tất cả property public
            var sourceProps = typeof(T).GetProperties();
            var destProps = typeof(Y).GetProperties();
            
            foreach (var sourceProp in sourceProps)
            {
                // Tìm prop cùng tên và kiểu dữ liệu trong destination 
                var destProp = Array.Find(destProps, p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);

                // Nếu như tìm được thì set dữ liệu cho prop của destination
                if (destProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    destProp.SetValue(destination, value);
                }
            }

            return destination;

        }

    }
}
