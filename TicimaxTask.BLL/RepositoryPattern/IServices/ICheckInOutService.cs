using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.BLL.RepositoryPattern.IServices
{
    public interface  ICheckInOutService
    {
        Task<Response<bool>> EnterAsync(CheckInOut checkInOut);
        Task<Response<bool>> ExitAsync(CheckInOut checkInOut);
        Task<Response<List<ReportDtoByDateScope>>> GetReport(int id, DateTime? firstDate, DateTime? secondDate);
        Task<Response<List<CheckInOut>>> GetUserMovementsWithDate(int userId, DateTime? firstDate, DateTime? secondDate);
    }
}
