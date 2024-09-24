using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public virtual async Task<bool> AddMultipleAsync(IEnumerable<T> entities)
        {
            return await _repository.AddMultipleAsync(entities);
        }

        public virtual async Task<bool> UpdateMultipleAsync(IEnumerable<T> entities)
        {
            return await _repository.UpdateMultipleAsync(entities);
        }

        public virtual async Task<bool> DeleteMultipleAsync(IEnumerable<int> ids)
        {
            return await _repository.DeleteMultipleAsync(ids);
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedDataAsync(
            string filter,
            int pageIndex = 0,
            int pageSize = 10)
        {
            return await _repository.GetPagedDataAsync(filter, pageIndex, pageSize);
        }

        public virtual async Task<T> FindByFieldAsync(string fieldName, object fieldValue)
        {
            return await _repository.FindByFieldAsync(fieldName, fieldValue);
        }
    }
}
