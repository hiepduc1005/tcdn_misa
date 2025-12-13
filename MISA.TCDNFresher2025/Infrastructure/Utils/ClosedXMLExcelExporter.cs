using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAAtribute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MISA.Infrastructure.Utils
{
    public class ClosedXMLExcelExporter : IExcelExporterService
    {
        public byte[] ExportExcel<T>(List<T> entities)
        {
            // Lấy các property mà không có MISAExportIgnore
            var propertiesToExport = typeof(T)
                .GetProperties()
                .Where(prop => !Attribute.IsDefined(prop, typeof(MISAExportIgnore)))
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                // Ten label cua table (Shift -> Ca lam viec)
                var tableLabel = ReflectionHelper.GetTableLabel<T>();
                var worksheet = workbook.Worksheets.Add(tableLabel);

                // Cấu hình Font chung
                worksheet.Style.Font.FontName = "Times New Roman";
                worksheet.Style.Font.FontSize = 12;

                // Thêm cột STT nên phải cộng 1 
                var totalRow = propertiesToExport.Count + 1;

                var titleRange = worksheet.Range(2, 1, 2, totalRow);

                // Các style của title
                titleRange.Merge();
                titleRange.Value = tableLabel.ToUpper();
                titleRange.Style.Font.FontSize = 16;
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Bảng bắt đầu ở dòng thứ 4 và cột thứ 2 (cột đầu là STT)
                int headerRow = 4;
                int headerCol = 2;

                // Cột STT
                var cellSTT = worksheet.Cell(headerRow, 1);
                cellSTT.Value = "STT";

                // Style cho cột STT
                cellSTT.Style.Font.Bold = true;
                cellSTT.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cellSTT.Style.Fill.BackgroundColor = XLColor.FromHtml("#D8D8D8");

                foreach (var prop in propertiesToExport)
                {
                    // Tên header của bảng
                    var headerLabel = ReflectionHelper.GetColumnLabel(prop);

                    var currentCell = worksheet.Cell(headerRow, headerCol);
                    currentCell.Value = headerLabel;

                    // Style Header
                    currentCell.Style.Font.Bold = true;
                    currentCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D8D8D8");

                    // Đến cột tiếp theo
                    headerCol++;
                }

                // Kẻ viền Header (cộng thêm 1 vì thêm cột STT)
                worksheet.Range(headerRow, 1, headerRow, totalRow)
                         .Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                // Dữ liệu

                // Bên dưới header
                int currentRow = 5;
                int stt = 1;

                foreach(var entity in entities)
                {
                    // Cột đầu tiên là STT
                    worksheet.Cell(currentRow, 1).Value = stt++;

                    // Bỏ qua cột STT
                    int currentDataCol = 2;

                    foreach(var prop in propertiesToExport)
                    {
                        var value = prop.GetValue(entity);
                        worksheet.Cell(currentRow, currentDataCol).Value = value != null ? value.ToString() : "";
                        currentDataCol++;
                    }

                    currentRow++;
                }

                // Rộng bằng content k bị che mất
                worksheet.Columns().AdjustToContents();

                // Kẻ viền Data
                worksheet.Range(headerRow + 1, 1, currentRow, totalRow)
                         .Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                         .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                // Trả về byte[]
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }   
    }
}
