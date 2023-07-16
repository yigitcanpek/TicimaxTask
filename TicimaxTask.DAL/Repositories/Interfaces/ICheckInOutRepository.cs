using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.DAL.Repositories.Interfaces
{
    public interface ICheckInOutRepository:IGenericRepository<CheckInOut>
    {
        Task<bool> EnterAsync(CheckInOut checkInOut);
        Task<bool> ExitAsync(CheckInOut checkInOut);
        Task<List<ReportDtoByDateScope>> GetReport(int id, DateTime? firstDate, DateTime? secondDate);
        Task<List<CheckInOut>> GetUserMovementsWithDate(int userId, DateTime? firstDate, DateTime? secondDate);

    }
}
