using Core.Entities;
using Core.Interfaces.Repositories;
using Dapper;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using MISA.Core.Dtos.Common;
using MISA.Core.MISAAtribute;
using MISA.Infrastructure.Utils;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository cơ sở triển khai các phương thức CRUD chung cho các entity.
    /// Kế thừa <see cref="IBaseRepository{T}"/> và <see cref="IDisposable"/> để quản lý kết nối cơ sở dữ liệu.
    /// </summary>
    /// <typeparam name="T">Kiểu entity mà repository này quản lý.</typeparam>
    /// <remarks>
    /// Created By: hiepnd - 12/2025
    /// </remarks>
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
    {
        protected readonly string connectionString;
        protected IDbConnection dbConnection;
     
        public BaseRepository (IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("StrongConnection");
            dbConnection = new MySqlConnection(connectionString);
        }


        /// <summary>
        /// Hàm xóa entity ra khỏi database
        /// </summary>
        /// <param name="entityId">Id của entity</param>
        /// Created By: hiepnd - 12/2025

        public void Delete(Guid entityId)
        {
            var porperties = typeof(T).GetProperties();
            var tableName = typeof(T).Name.ToLower();

            var idProp = porperties.FirstOrDefault(p =>
                p.IsDefined(typeof(MISAPrimaryKey), false));

            var idField = ReflectionHelper.GetColumnName(idProp);
            string sqlCommand = $"DELETE FROM {tableName} WHERE {idField} = @id";

            dbConnection.Execute(sqlCommand, new { id = entityId });
        }

        /// <summary>
        /// Hàm giải phóng bộ nhớ tránh memory leak
        /// </summary>
        /// Created By: hiepnd - 12/2025
        public void Dispose()
        {
            dbConnection?.Dispose();
        }

        /// <summary>
        /// Hàm lấy tất cả entity có trong database
        /// </summary>
        /// <returns>Danh sách entity</returns>
        /// Created By: hiepnd - 12/2025
        public List<T> GetAll()
        {
            var tableName = GetTableName();
            string sqlCommand = $"SELECT * FROM {tableName}";
            return dbConnection.Query<T>(sqlCommand).ToList();

        }

        /// <summary>
        /// Hàm lây entity theo id của entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Entity theo id truyền vào</returns>
        /// Created By: hiepnd - 12/2025
        public T GetById(Guid entityId)
        {
            var tableName = GetTableName();
            var idProp = typeof(T).GetProperties()
                  .FirstOrDefault(p => p.IsDefined(typeof(MISAPrimaryKey), false));

            var idField = ReflectionHelper.GetColumnName(idProp);
            string sqlCommand = $"SELECT * FROM {tableName} WHERE {idField} = @id";
            return dbConnection.QueryFirstOrDefault<T>(sqlCommand, new { id = entityId });
        }

        /// <summary>
        /// Hàm thêm entity
        /// </summary>
        /// <param name="entity">Entity cần thêm vào database</param>
        /// <returns>Entity sau khi đã thêm</returns>
        /// Created By: hiepnd - 12/2025
        public T Insert(T entity)
        {
            // 1.Lấy danh sách các thuộc tính(Property) của class
            var porperties = typeof(T).GetProperties();

            // 2. Lấy tên bảng
            var tableName = GetTableName();

            var columns = new StringBuilder();
            var columnsParam = new StringBuilder();
            var parameters = new DynamicParameters();

            foreach(var prop in porperties)
            {
                // Kiểm tra Attribute[NotMapped
                if (prop.IsDefined(typeof(MISANotMapped), false))
                {
                    continue; // Bỏ qua, không xử lý
                }
                string columnName = ReflectionHelper.GetColumnName(prop);
                columns.Append($"{columnName},");

                columnsParam.Append($"@{prop.Name},");


                //Thêm parameter cho Dapper

                parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            //Bỏ dấu phẩy ở cuối

            var columnString = columns.ToString().TrimEnd(',');
            var columnsParamString = columnsParam.ToString().TrimEnd(',');

            var sql = $"INSERT INTO {tableName} ({columnString}) VALUES ( {columnsParamString} ) ";

            dbConnection.Execute(sql, parameters);

            return entity;
        }

        /// <summary>
        /// Hàm cập nhật entity
        /// </summary>
        /// <param name="entity">Entity cần cập nhật</param>
        /// <returns>Entity sau khi cập nhật</returns>
        /// Created By: hiepnd - 12/2025
        public T Update(T entity)
        {
            // Lấy danh sách các thuộc tính của class
            var properties = typeof(T).GetProperties();

            var idProp = properties.FirstOrDefault(p =>
                p.IsDefined(typeof(MISAPrimaryKey), false));

            // Lấy tên bảng
            var tableName = GetTableName();

            var setClause = new StringBuilder();

            var parameters = new DynamicParameters();
            var propertyId = idProp.Name;
            var columnId = ReflectionHelper.GetColumnName(idProp);

            foreach ( var prop in properties)
            {
                // Kiểm tra Attribute[NotMapped
                if (prop.IsDefined(typeof(MISANotMapped), false))
                {
                    continue; // Bỏ qua, không xử lý
                }

                // Nếu là id thì lấy để không set id và làm where clause
                if (prop.Name.Equals(propertyId))
                {
                    parameters.Add($"@{propertyId}", prop.GetValue(entity));

                    //Tiếp tục k chạy phần dưới để k set id
                    continue;
                }

                string columnName = ReflectionHelper.GetColumnName(prop);

                setClause.Append($"{columnName} = @{prop.Name},");

                parameters.Add($"@{prop.Name}", prop.GetValue(entity));

            }
            
            // Bỏ dấu phẩy ở cuối
            var setClauseString = setClause.ToString().TrimEnd(',');

            var updateSql = $"UPDATE {tableName} SET {setClauseString} WHERE {columnId} = @{propertyId}";

            dbConnection.Execute(updateSql, parameters);
            return entity;
        }

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
        public PagingResult<T> getDataPaging(int pageIndex, int pageSize, List<FilterItem> filters = null, List<FilterItem> customFilters = null, List<SortItem> sorts = null)
        {
            // Lấy tên table
            var tableName = GetTableName();
            var filterClause = new StringBuilder();
            var customClause = new StringBuilder();
            var sortClause = new StringBuilder();

            var sqlPaging = new StringBuilder();

            var parameters = new DynamicParameters();

            sqlPaging.Append($"SELECT * FROM {tableName} ");

            // Nếu như có filters thì mới có mệnh đề WHERE
            if (filters != null && filters.Count > 0)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    var filter = filters[i];
                    var value = filter?.Value; 
                    var @operator = filter.Operator;

                    // Lấy tên cột trong sql dựa theo tên thuộc tính trong class
                    var column = ReflectionHelper.GetColumnNameFromFieldName<T>(filter.Column);

                    // Tên param
                    var paramName = $"filter_{column}_{i}";
                    var filterResult = MISASqlMapper.MapOperatorToSql(@operator, column, value, paramName);

                    // Nối AND nếu không phải filter đầu tiên 
                    if (i > 0)
                        filterClause.Append(" AND ");

                    filterClause.Append(filterResult.SqlClause);

                    // Thêm value vào DynamicParameters để bind
                    if (filterResult.Value != null)
                        parameters.Add(paramName, filterResult.Value);
                }

            }

            if (customFilters != null && customFilters.Count > 0)
            {
                for (int i = 0; i < customFilters.Count; i++)
                {
                    var filter = customFilters[i];
                    var column = ReflectionHelper.GetColumnNameFromFieldName<T>(filter.Column);
                    var paramName = $"custom_{column}_{i}";
                    var mapped = MISASqlMapper.MapOperatorToSql(filter.Operator, column, filter.Value, paramName);

                    if (i > 0)
                        customClause.Append(" OR ");

                    customClause.Append(mapped.SqlClause);

                    if (mapped.Value != null)
                        parameters.Add(paramName, mapped.Value);
                }
            }

            // Gom 2 cai filter và custom vào có dạng (filter1 AND filter2 ...) AND (custom1 OR custom2 ...)
            var whereParts = new List<string>();

            if (filterClause.Length > 0)
                whereParts.Add($"({filterClause})");

            if (customClause.Length > 0)
                whereParts.Add($"({customClause})");

            if (whereParts.Count > 0)
                sqlPaging.Append("WHERE " + string.Join(" AND ", whereParts) + " ");

            if (sorts != null && sorts.Count > 0)
            {
                for (int i = 0; i < sorts.Count; i++)
                {
                    var sort = sorts[i];
                    var column = ReflectionHelper.GetColumnNameFromFieldName<T>(sort.FieldName);

                    var orderBy = MISASqlMapper.MapSortToSql(sort.Direction, column);

                    // Nối phẩy nếu không phải sort đầu tiên
                    if (i > 0)
                        sortClause.Append(" , ");

                    sortClause.Append(orderBy);
                }

                sqlPaging.Append($"ORDER BY {sortClause.ToString()} ");
            }
            else
            {
                sqlPaging.Append($"ORDER BY created_date DESC ");
            }
            
            // page index bắt đầu từ 1 
            // Nếu pageIndex bé hơn hoặc = 0 thì offset = 0
            var offset = (pageIndex > 0) ? ((pageIndex - 1) * pageSize) : 0;

            sqlPaging.Append($"LIMIT {pageSize} OFFSET {offset}; ");

            sqlPaging.Append("SELECT COUNT(*) FROM " + tableName);
            if(whereParts.Count > 0)
            {
                sqlPaging.Append(" WHERE " + string.Join(" AND ", whereParts));
            }
            sqlPaging.Append(";");

            using (var multi = dbConnection.QueryMultiple(sqlPaging.ToString(), parameters))
            {
                var data = multi.Read<T>().ToList();       // Result set 1: dữ liệu
                var totalRecords = multi.ReadFirst<int>(); // Result set 2: total count

                var dataPaging = new PagingResult<T> { 
                    CurrentPage = pageIndex,
                    DataPaging = data,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };

                return dataPaging;
            }


        }

        /// <summary>
        /// Lấy tên bảng từ attribute [Table], nếu không có thì dùng tên class
        /// </summary>
        /// <returns></returns>
        protected string GetTableName()
        {
            var tableAttr = typeof(T).GetCustomAttributes(typeof(MISATable), false).FirstOrDefault() as MISATable;
            return tableAttr != null ? tableAttr.Name : typeof(T).Name.ToLower();
        }
    }
}
