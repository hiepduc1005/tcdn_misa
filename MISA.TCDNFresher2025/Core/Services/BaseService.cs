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
    public class BaseService<T> : IBaseService<T>
    {
        IBaseRepository<T> _baseRepository;

        IExcelExporterService _excelExporterService;

        public BaseService(IBaseRepository<T> baseRepository, IExcelExporterService excelExporterService)
        {
            this._baseRepository = baseRepository;
            this._excelExporterService = excelExporterService;
        }

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
