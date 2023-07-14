using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.BaseModels;

namespace TicimaxTask.BLL.RepositoryPattern.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<Response<T>> CreateAsync(T item);
        Response<T> UpdateAsync(T item);
        Task<Response<T>> DeleteAsync(int id);
        Task<Response<List<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
    }
}
