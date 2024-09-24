using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddMultipleAsync(IEnumerable<T> entities);
        Task<bool> UpdateMultipleAsync(IEnumerable<T> entities);
        Task<bool> DeleteMultipleAsync(IEnumerable<int> ids);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedDataAsync(
            string filter,
            int pageIndex = 0,
            int pageSize = 10);
        Task<T> FindByFieldAsync(string fieldName, object fieldValue);
    }
}
