using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.BLL.RepositoryPattern.IServices
{
    public interface IBaseService<T> where T : BaseEntity
    {

        Task<Response<T>> CreateAsync(T item);
        Response<T> Update(T item);
        Task<Response<T>> DeleteAsync(int id);
        Task<Response<List<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
    }
}
