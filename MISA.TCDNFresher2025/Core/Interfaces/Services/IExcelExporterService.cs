using MISA.Core.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Core.Interfaces.Services
{
    public interface IExcelExporterService
    {

        byte[] ExportExcel<T>(List<T> entities);
    }
}
