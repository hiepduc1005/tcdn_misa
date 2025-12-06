using Core.Interfaces.Repositories;
using Dapper;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using MISA.Core.MISAAtribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
            var tableName = typeof(T).Name.ToLower();
            var idField = (tableName + "Id").ToSnakeCase();
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
            var tableName = typeof(T).Name.ToLower();
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
            var tableName = typeof(T).Name.ToLower();
            var idField = (tableName + "Id").ToSnakeCase();
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
            var tableName = typeof(T).Name.ToLower();

            var columns = ""; 
            var columnsParam = "";
            var parameters = new DynamicParameters();

            foreach(var prop in porperties)
            {
                // Kiểm tra Attribute[NotMapped
                if (prop.IsDefined(typeof(MISANotMapped), false))
                {
                    continue; // Bỏ qua, không xử lý
                }
                string columnName = prop.Name.ToSnakeCase();
                columns += $"{columnName},";

                columnsParam += $"@{prop.Name},";

                //Thêm parameter cho Dapper

                parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            //Bỏ dấu phẩy ở cuối

            columns = columns.TrimEnd(',');
            columnsParam = columnsParam.TrimEnd(',');

            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ( {columnsParam} ) ";

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

            // Lấy tên bảng
            var tableName = typeof(T).Name.ToLower();

            var setClause = "";

            var parameters = new DynamicParameters();
            var propertyId = (tableName + "Id");
            var columnId = propertyId.ToSnakeCase();

            foreach( var prop in properties)
            {
                // Kiểm tra Attribute[NotMapped
                if (prop.IsDefined(typeof(MISANotMapped), false))
                {
                    continue; // Bỏ qua, không xử lý
                }

                // Nếu là id thì lấy để không set id và làm where clause
                if (prop.Name.Equals(propertyId))
                {
                    parameters.Add($"@{prop.Name}", prop.GetValue(entity));

                    //Tiếp tục k chạy phần dưới để k set id
                    continue;
                }

                string columnName = prop.Name.ToSnakeCase();

                setClause += $"{columnName} = @{prop.Name},";

                parameters.Add($"@{prop.Name}", prop.GetValue(entity));

            }

            // Bỏ dấu phẩy ở cuối
            setClause = setClause.TrimEnd(',');

            var updateSql = $"UPDATE {tableName} SET {setClause} WHERE {columnId} = @{propertyId}";

            dbConnection.Execute(updateSql, parameters);
            return entity;
        }
    }
}
