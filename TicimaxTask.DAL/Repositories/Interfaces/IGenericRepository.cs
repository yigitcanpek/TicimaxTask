using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T item);
        T Update(T item);
        void DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }
}
