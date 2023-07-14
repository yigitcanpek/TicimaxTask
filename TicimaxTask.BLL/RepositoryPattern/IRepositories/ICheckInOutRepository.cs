using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.BaseModels;

namespace TicimaxTask.BLL.RepositoryPattern.IRepositories
{
    public interface ICheckInOutRepository:IBaseRepository<CheckInOut>
    {
        Task<Response<CheckInOut>> GetAllCheckWithDate(int id , DateTime firstDate,DateTime secondDate);
    }
}
