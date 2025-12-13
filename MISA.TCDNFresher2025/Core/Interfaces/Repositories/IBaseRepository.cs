using MISA.Core.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface dùng chung cho các repository, định nghĩa các phương thức CRUD chung
    /// cho các entity trong hệ thống.
    /// </summary>
    /// <typeparam name="T">Kiểu entity mà repository này quản lý.</typeparam>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Lấy danh sách tất cả các bản ghi.
        /// </summary>
        /// <returns>Danh sách các bản ghi kiểu <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        List<T> GetAll();


        /// <summary>
        /// Lấy thông tin một bản ghi theo Id.
        /// </summary>
        /// <param name="entityId">Id của bản ghi cần lấy.</param>
        /// <returns>Bản ghi kiểu <typeparamref name="T"/> tương ứng với Id, hoặc null nếu không tồn tại.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        T GetById(Guid entityId);

        /// <summary>
        /// Thêm một bản ghi mới vào hệ thống.
        /// </summary>
        /// <param name="entity">Đối tượng kiểu <typeparamref name="T"/> cần thêm.</param>
        /// <returns>Bản ghi vừa được thêm vào.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        T Insert(T entity);


        /// <summary>
        /// Cập nhật thông tin của một bản ghi.
        /// </summary>
        /// <param name="entity">Đối tượng kiểu <typeparamref name="T"/> đã được chỉnh sửa.</param>
        /// <returns>Bản ghi đã được cập nhật.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        T Update(T entity);

        /// <summary>
        /// Xóa một bản ghi theo Id.
        /// </summary>
        /// <param name="entityId">Id của bản ghi cần xóa.</param>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        void Delete(Guid entityId);

        /// <summary>
        /// Lấy danh sách dữ liệu có phân trang, kết hợp lọc và sắp xếp.
        /// </summary>
        /// <param name="pageIndex">Số trang hiện tại (bắt đầu từ 1).</param>
        /// <param name="pageSize">Số lượng bản ghi trên một trang.</param>
        /// <param name="filters">Danh sách các điều kiện lọc (nếu có).</param>
        /// <param name="sorts">Danh sách các điều kiện sắp xếp (nếu có).</param>
        /// <returns>Đối tượng chứa danh sách dữ liệu và tổng số bản ghi tìm thấy.</returns>
        /// <remarks>
        /// Created By: hiepnd - 12/2025
        /// </remarks>
        PagingResult<T> getDataPaging(int pageIndex, int pageSize, List<FilterItem> filters = null, List<FilterItem> customFilters = null, List<SortItem> sorts = null);
    }
}
