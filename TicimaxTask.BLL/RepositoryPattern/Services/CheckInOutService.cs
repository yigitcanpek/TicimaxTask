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
    public class CheckInOutService : BaseService<CheckInOut>, ICheckInOutService
    {

        ICheckInOutRepository _checkInOut;

        public CheckInOutService(ICheckInOutRepository checkInOut) : base(checkInOut)
        {
            _checkInOut = checkInOut;
        }

        public async Task<Response<bool>> EnterAsync(CheckInOut checkInOut)
        {
            try
            {

                await _checkInOut.EnterAsync(checkInOut);
                return Response<bool>.Success(true,200);
            }
            catch (Exception ex)
            {

                return Response<bool>.Fail($"{ex.Message}", 200);
            }
        }

        public async Task<Response<bool>> ExitAsync(CheckInOut checkInOut)
        {
            try
            {
                await _checkInOut.ExitAsync(checkInOut);
                return Response<bool>.Success(true,200);
            }
            catch (Exception ex)
            {

                return Response<bool>.Fail($"{ex.Message}", 400);
            }
        }

        public async Task<Response<List<ReportDto>>> GetReport(int id, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                List<ReportDto> reports =await _checkInOut.GetReport(id, firstDate, secondDate);
                return Response<List<ReportDto>>.Success(reports,200);
            }
            catch (Exception ex)
            {

                return  Response<List<ReportDto>>.Fail($"{ex.Message}", 400);
            }
        }

        public async Task<Response<List<CheckInOut>>> GetUserMovementsWithDate(int userId, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                List<CheckInOut> report = await _checkInOut.GetUserMovementsWithDate(userId, firstDate, secondDate);
                
                return Response<List<CheckInOut>>.Success(report,200);
            }
            catch (Exception ex)
            {
                return Response<List<CheckInOut>>.Fail($"{ex.Message}", 400);

            }
        }
    }
}
