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
    public class CheckInOutRepository :GenericRepository<CheckInOut> ,ICheckInOutRepository
    {
        
        

        public CheckInOutRepository(PostgreSqlContext context):base(context)
        {   
            
        }

        public async Task<List<ReportDto>> GetReport(int id, DateTime firstDate, DateTime secondDate)
        {

            List<ReportDto> reportDtos = new List<ReportDto>();


            while (firstDate.Date <= secondDate.Date)
            {
                
                List<CheckInOut> userMovements = await _dbSet
                    .Where(x => x.AppUserID == id && x.CheckTime.Date.ToLocalTime() == firstDate)
                    .ToListAsync();

                
                DateTime? firstCheckIn = userMovements.FirstOrDefault(x => x.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn)?.CheckTime;
                DateTime? lastCheckOut = userMovements.LastOrDefault(x => x.CheckType == Entities.Entities.Enums.CheckStatus.CheckOut)?.CheckTime;

                
                TimeSpan? duration = lastCheckOut - firstCheckIn;
                TimeSpan actualDuration = duration.GetValueOrDefault();

                
                if (firstCheckIn != null && lastCheckOut != null)
                {
                    ReportDto reportDto = new ReportDto()
                    {
                        InCount = userMovements.Count(x => x.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn),
                        OutCount = userMovements.Count(x => x.CheckType == Entities.Entities.Enums.CheckStatus.CheckOut),
                        InTimeCount = actualDuration,
                    };
                    reportDtos.Add(reportDto);
                }

                
                firstDate = firstDate.AddDays(1);
            }

            return reportDtos;




        }

        public async Task<bool> EnterAsync(CheckInOut checkInOut)
        {
            
            await _dbSet.AddAsync(checkInOut);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> ExitAsync(CheckInOut checkInOut)
        {
            await _dbSet.AddAsync(checkInOut);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<CheckInOut>> GetUserMovementsWithDate(int userId, DateTime firstDate, DateTime secondDate)
        {

            
            List<CheckInOut> checkIns = await _dbSet.Where(x => x.AppUserID == userId && x.CheckTime.ToUniversalTime() >= firstDate.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn).ToListAsync();
            
            List<CheckInOut> checkOuts  = await _dbSet.Where(x => x.AppUserID == userId && x.CheckTime.ToUniversalTime() >= firstDate.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckOut).ToListAsync();

            List<CheckInOut> allChecks = new List<CheckInOut>();

            checkIns.ForEach(x => x.CheckTime.ToLocalTime());
            checkOuts.ForEach(x => x.CheckTime.ToLocalTime());

            allChecks.AddRange(checkIns);
            allChecks.AddRange(checkOuts);

            return allChecks;
        }
    }
}
