using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.DAL.PostgreSqlDb;
using TicimaxTask.DAL.Repositories.Interfaces;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.DAL.Repositories.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : BaseEntity
    {
        protected readonly PostgreSqlContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(PostgreSqlContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        public async Task<T> CreateAsync(T item)
        {
                item.CreatedDate = DateTime.UtcNow;
                await _dbSet.AddAsync(item);
                _context.SaveChanges();
                return item;
        }

        public async void DeleteAsync(int id)
        {
           
                T item = await _dbSet.FindAsync(id);
                item.Status = Entities.Entities.Enums.DataStatus.Deleted;
                item.DeletedDate = DateTime.Now;
                _context.SaveChanges();
                
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
         
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
           
        }

        public  T Update(T item)
        {

            
                item.ModifiedDate = DateTime.Now;
                item.Status = Entities.Entities.Enums.DataStatus.Updated;
                T toBeUpdated = _dbSet.Find(item.ID);
                _dbSet.Entry(toBeUpdated).CurrentValues.SetValues(item);
                _context.SaveChanges();
                return toBeUpdated;
        }
    }
}

