using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Dapper;
using Domain.Extensions;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// base repository class for CRUD operations. (lớp repository cơ sở cho các hoạt động CRUD)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IDbConnection _dbConnection;
        protected string TableName => StringExtension.ToSnakeCase(typeof(T).Name);
        public BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Retrieves an entity by its Id asynchronously. (lấy ra 1 entity theo id của nó 1 cách bất đồng bộ)
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve. (id của entity cần lấy)</param>
        /// <returns>The entity with the specified Id. (entity với id đã chỉ định)</returns>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var query = $"SELECT * FROM {TableName} WHERE Id = @Id";
            var result = await _dbConnection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            if (result == null)
            {
                throw new InvalidOperationException($"Entity with Id {id} not found.");
            }
            return result;
        }

        /// <summary>
        /// Retrieves all entities asynchronously. (lấy tất cả các entity 1 cách bất đồng bộ)
        /// </summary>
        /// <returns>A collection of all entities. (một tập hợp các entity)</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM {TableName}";
            return await _dbConnection.QueryAsync<T>(query);
        }

        /// <summary>
        /// Adds a new entity asynchronously. (thêm một entity mới 1 cách bất đồng bộ)
        /// </summary>
        /// <param name="entity">The entity to add. (entity cần thêm)</param>
        /// <returns>True if the entity was added successfully, otherwise false. (True nếu entity được thêm thành công, ngược lại là false)</returns>
        public virtual async Task<bool> AddAsync(T entity)
        {
            var query = GenerateInsertQuery();
            var result = await _dbConnection.ExecuteAsync(query, entity);
            return result > 0;
        }

        /// <summary>
        /// Updates an existing entity asynchronously. (cập nhật một entity đã tồn tại 1 cách bất đồng bộ)
        /// </summary>
        /// <param name="entity">The entity to update. (entity cần cập nhật)</param>
        /// <returns>True if the entity was updated successfully, otherwise false. (True nếu entity được cập nhật thành công, ngược lại là false)</returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            var query = GenerateUpdateQuery();
            var result = await _dbConnection.ExecuteAsync(query, entity);
            return result > 0;
        }

        /// <summary>
        /// Deletes an entity by its Id asynchronously. (xóa một entity theo id của nó 1 cách bất đồng bộ)
        /// </summary>
        /// <param name="id">The Id of the entity to delete. (id của entity cần xóa)</param>
        /// <returns>True if the entity was deleted successfully, otherwise false. (True nếu entity được xóa thành công, ngược lại là false)</returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var query = $"DELETE FROM {TableName} WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return result > 0;
        }

        /// <summary>
        /// Adds multiple entities asynchronously within a transaction. (thêm nhiều entity 1 cách bất đồng bộ trong một giao dịch)
        /// </summary>
        /// <param name="entities">The collection of entities to add. (tập hợp các entity cần thêm)</param>
        /// <returns>True if all entities were added successfully, otherwise false. (True nếu tất cả entity được thêm thành công, ngược lại là false)</returns>
        public virtual async Task<bool> AddMultipleAsync(IEnumerable<T> entities)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        var query = GenerateInsertQuery();
                        var result = await _dbConnection.ExecuteAsync(query, entity, transaction);
                        if (result <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates multiple entities asynchronously within a transaction. (cập nhật nhiều entity 1 cách bất đồng bộ trong một giao dịch)
        /// </summary>
        /// <param name="entities">The collection of entities to update. (tập hợp các entity cần cập nhật)</param>
        /// <returns>True if all entities were updated successfully, otherwise false. (True nếu tất cả entity được cập nhật thành công, ngược lại là false)</returns>
        public virtual async Task<bool> UpdateMultipleAsync(IEnumerable<T> entities)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        var query = GenerateUpdateQuery();
                        var result = await _dbConnection.ExecuteAsync(query, entity, transaction);
                        if (result <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes multiple entities by their Ids asynchronously within a transaction. (xóa nhiều entity theo id của chúng 1 cách bất đồng bộ trong một giao dịch)
        /// </summary>
        /// <param name="ids">The collection of Ids of the entities to delete. (tập hợp các id của các entity cần xóa)</param>
        /// <returns>True if all entities were deleted successfully, otherwise false. (True nếu tất cả entity được xóa thành công, ngược lại là false)</returns>
        public virtual async Task<bool> DeleteMultipleAsync(IEnumerable<int> ids)
        {
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var query = $"DELETE FROM {TableName} WHERE Id = @Id";
                        var result = await _dbConnection.ExecuteAsync(query, new { Id = id }, transaction);
                        if (result <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
         
        public virtual async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedDataAsync(string filter, int pageNumber, int pageSize)
        {
            // Parse the filter JSON
            var filterParsed = JArray.Parse(filter);

            // Build the dynamic SQL WHERE clause
            var whereClause = BuildWhereClause(filterParsed);

            // SQL query to get the filtered data and total count
            var sqlQuery = $@"
        SELECT * FROM {TableName}
        WHERE {whereClause}
        ORDER BY Id
        LIMIT @Offset, @PageSize;

        SELECT COUNT(*) FROM {TableName}
        WHERE {whereClause};";

            // Execute the SQL query
            var parameters = new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize };
            var multi = await _dbConnection.QueryMultipleAsync(sqlQuery, parameters);
            var data = (await multi.ReadAsync<T>()).ToList();
            var totalCount = (await multi.ReadAsync<int>()).Single();

            return (data, totalCount);
        }

        private string BuildWhereClause(JArray filter)
        {
            if (filter == null || !filter.Any())
            {
                return "1=1"; // No filter, return all records
            }

            var conditions = new List<string>();
            for (int i = 0; i < filter.Count; i++)
            {
                var item = filter[i];
                if (item is JArray subFilter)
                {
                    var subCondition = BuildWhereClause(subFilter);
                    if (!string.IsNullOrEmpty(subCondition))
                    {
                        conditions.Add($"({subCondition})");
                    }
                }
                else if (item is JValue jValue)
                {
                    // Logical operator (and/or)
                    var value = jValue.ToString().ToUpper();
                    if (i == 0 )
                    {

                    }
                    else if (i == 1)
                    {
                        value = GetSqlOperator(value);
                    } else if (i==2)
                    {
                        string rawOperator = filter[1] != null ? filter[1].ToString() : string.Empty;
                        value = FormatValueForSqlOperator(rawOperator, value);
                    }
                    conditions.Add($" value");
                }
            }

            return string.Join(" ", conditions);
        }

        private string GetSqlOperator(string operation)
        {
            switch (operation.ToLower())
            {
                case "startswith":
                case "endswith":
                case "contains":
                    return "LIKE";
                case "notcontains":
                    return "NOT LIKE";
                case "=":
                    return "=";
                case "<>":
                    return "<>";
                case ">":
                    return ">";
                case "<":
                    return "<";
                case ">=":
                    return ">=";
                case "<=":
                    return "<=";
                default:
                    throw new NotSupportedException($"Operation '{operation}' is not supported.");
            }
        }

        private string FormatValueForSqlOperator(string operation, string value)
        {
            switch (operation)
            {
                case "startswith":
                    return $"{value}%";
                case "endswith":
                    return $"%{value}";
                case "contains":
                case "notcontains":
                    return $"%{value}%";
                default:
                    return value;
            }
        }


        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {TableName} ");

            insertQuery.Append("(");

            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            foreach (var prop in properties)
            {
                insertQuery.Append($"[{prop.Name}],");
            }

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            foreach (var prop in properties)
            {
                insertQuery.Append($"@{prop.Name},");
            }

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");

            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            foreach (var prop in properties)
            {
                updateQuery.Append($"[{prop.Name}] = @{prop.Name},");
            }

            updateQuery
                .Remove(updateQuery.Length - 1, 1)
                .Append(" WHERE Id = @Id");

            return updateQuery.ToString();
        }

        /// <summary>
        /// Retrieves an entity by a specific field value asynchronously. (lấy ra một entity theo giá trị trường cụ thể 1 cách bất đồng bộ)
        /// </summary>
        /// <param name="fieldName">The name of the field to search by. (tên trường để tìm kiếm)</param>
        /// <param name="fieldValue">The value of the field to search for. (giá trị của trường cần tìm kiếm)</param>
        /// <returns>The entity with the specified field value. (entity với giá trị trường đã chỉ định)</returns>
        public virtual async Task<T> FindByFieldAsync(string fieldName, object fieldValue)
        {
            var query = $"SELECT * FROM {TableName} WHERE {fieldName} = @FieldValue";
            var result = await _dbConnection.QuerySingleOrDefaultAsync<T>(query, new { FieldValue = fieldValue });
            if (result == null)
            {
                throw new InvalidOperationException($"Entity with {fieldName} = {fieldValue} not found.");
            }
            return result;
        }
    }
}
