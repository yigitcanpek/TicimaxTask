using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.BaseModels;

namespace TicimaxTask.BLL.RepositoryPattern.IRepositories
{
    public interface IAppUserRepository:IBaseRepository<AppUser> 
    {
        Task<Response<List<CheckInOut>>> GetUserMovementsWithDate(int userId, DateTime firstDate,DateTime secondDate);
        Task<Response<AppUser>> GetUserReportsWithDate(int userId, DateTime firstDate, DateTime secondDate);
    }
}
