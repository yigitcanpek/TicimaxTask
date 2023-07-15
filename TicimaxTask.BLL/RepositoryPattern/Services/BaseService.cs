using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.BLL.RepositoryPattern.IServices;
using TicimaxTask.DAL.Repositories.Interfaces;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.BLL.RepositoryPattern.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected IGenericRepository<T> _repository;

        public BaseService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public  async Task<Response<T>> CreateAsync(T item)
        {
            try
            {

                await _repository.CreateAsync(item);
                return Response<T>.Success(item, 200);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}",400);
            }
        }

        public async Task<Response<T>> DeleteAsync(int id)
        {
            try
            {

                _repository.DeleteAsync(id);
                return Response<T>.Success(204);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}",400);
            }
        }

        public async Task<Response<List<T>>> GetAllAsync()
        {
            try
            {
               List<T> datas = await _repository.GetAllAsync();
                return Response<List<T>>.Success(datas, 200);
            }
            catch (Exception ex)
            {

                return Response<List<T>>.Fail($"{ex.Message}", 400);
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {
            try
            {
               T data =  await _repository.GetByIdAsync(id);
                return Response<T>.Success(data, 200);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}", 400);
            }
        }

        public  Response<T> Update(T item)
        {
            try
            {
                _repository.Update(item);
                return Response<T>.Success(204);
            }
            catch (Exception ex)
            {

                return Response<T>.Fail($"{ex.Message}", 400);
            }
        }
    }
}
