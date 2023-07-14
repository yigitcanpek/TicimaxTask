using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.BLL.RepositoryPattern.IRepositories;
using TicimaxTask.DAL.PostgreSqlDb;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.BaseModels;
using TicimaxTask.Shared.DTO_s;

namespace TicimaxTask.BLL.RepositoryPattern.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly PostgreSqlContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(PostgreSqlContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        public async Task<Response<T>> CreateAsync(T item)
        {
            try
            {
                await _dbSet.AddAsync(item);
                _context.SaveChanges();
                return Response<T>.Success(item, 200);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}",400,true);
            }


           
        }

        public async Task<Response<T>> DeleteAsync(int id)
        {
            try
            {
                T item = await _dbSet.FindAsync(id);
                item.Status = Entities.Entities.Enums.DataStatus.Deleted;
                item.DeletedDate = DateTime.Now;
                _context.SaveChanges();
                return Response<T>.Success(204);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}", 400, true);
            }
        }

        public async Task<Response<List<T>>> GetAllAsync()
        {
            try
            {
                return Response<List<T>>.Success(await _dbSet.ToListAsync(), 200);
            }
            catch (Exception ex)
            {

                return Response<List<T>>.Fail($"{ex.Message}", 400, true);
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {

            try
            {
                return Response<T>.Success(await _dbSet.FindAsync(id), 200);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}", 400, true);
            }
        }

        public  Response<T> UpdateAsync(T item)
        {
            
            try
            {
                item.ModifiedDate = DateTime.Now;
                item.Status = Entities.Entities.Enums.DataStatus.Updated;
                T toBeUpdated = _dbSet.Find(item.ID);
                _dbSet.Entry(toBeUpdated).CurrentValues.SetValues(item);
                _context.SaveChanges();
                return Response<T>.Success(toBeUpdated, 200);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}", 400, true);
            }
        }
    }
}

