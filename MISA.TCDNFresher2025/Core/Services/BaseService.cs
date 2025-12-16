using ClosedXML.Excel;
using Core.Entities;
using Core.Interfaces.Repositories;
using MISA.Core.Dtos.Common;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Services
{
    /// <summary>
    /// Lớp service cơ sở, triển khai các chức năng dùng chung cho các service.
    /// </summary>
    /// <typeparam name="T">Kiểu entity.</typeparam>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class BaseService<T> : IBaseService<T>
    {
        IBaseRepository<T> _baseRepository;

        IExcelExporterService _excelExporterService;

        /// <summary>
        /// Khởi tạo BaseService với repository và service export Excel.
        /// </summary>
        /// <param name="baseRepository">Repository thao tác dữ liệu.</param>
        /// <param name="excelExporterService">Service xuất dữ liệu ra Excel.</param>
        public BaseService(IBaseRepository<T> baseRepository, IExcelExporterService excelExporterService)
        {
            this._baseRepository = baseRepository;
            this._excelExporterService = excelExporterService;
        }

        /// <summary>
        /// Xuất toàn bộ dữ liệu (theo điều kiện lọc và sắp xếp)
        /// ra file Excel.
        /// </summary>
        /// <param name="pagingRequest">
        /// Thông tin phân trang, lọc và sắp xếp.
        /// Khi export Excel, PageSize sẽ được set về giá trị lớn
        /// để lấy toàn bộ dữ liệu thay vì chỉ trang hiện tại.
        /// </param>
        /// <returns>
        /// Mảng byte đại diện cho file Excel (.xlsx).
        /// </returns>
        public byte[] ExportExcel(PagingRequest pagingRequest)
        {

            // Page size phải lớn để lấy được tất cả bản ghi chứ k phải lấy tất bản ghi ở trang hiện tại 
            pagingRequest.PageSize = int.MaxValue;
            pagingRequest.PageIndex = 1;

            PagingResult<T> pagingResult = _baseRepository.getDataPaging(
                pagingRequest.PageIndex,
                pagingRequest.PageSize,
                pagingRequest.FilterItems,
                pagingRequest.CustomFilters,
                pagingRequest.SortItems
            );

            List<T> shifts = pagingResult.DataPaging.ToList();

            return _excelExporterService.ExportExcel<T>( shifts );

        }
    }
}
