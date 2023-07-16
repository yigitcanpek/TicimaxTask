using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.DAL.PostgreSqlDb;
using TicimaxTask.DAL.Repositories.Interfaces;
using TicimaxTask.Entities.Entities.Enums;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.DAL.Repositories.Repositories
{
    public class CheckInOutRepository :GenericRepository<CheckInOut> ,ICheckInOutRepository
    {
        
        

        public CheckInOutRepository(PostgreSqlContext context):base(context)
        {   
            
        }

        public async Task<List<ReportDtoByDateScope>> GetReport(int id, DateTime? firstDate, DateTime? secondDate)
        {
            List<ReportDtoByDateScope> reportDtos = new List<ReportDtoByDateScope>();
            if (!firstDate.HasValue)
            {
                firstDate = secondDate.Value.AddDays(-7);
            }
            if (!secondDate.HasValue)
            {
                secondDate = firstDate.Value.AddDays(7);
            }

            if (id != 0)
            {

                List<CheckInOut> userCheckInOuts = await _dbSet.Where(x => x.AppUserID == id && x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime()).ToListAsync();
                List<IGrouping<DateTime, CheckInOut>> groupedCheckInOuts = userCheckInOuts.GroupBy(x => x.CheckTime.Date).ToList();


                foreach (var group in groupedCheckInOuts)
                {
                    DateTime currentDate = group.Key;
                    List<CheckInOut> checkInOuts = group.ToList();

                    // Gruplanmış veriler için istediğiniz işlemleri yapabilirsiniz
                    // Örneğin:
                    int userCheckIn = checkInOuts.Count(x => x.CheckType == CheckStatus.CheckIn);
                    int userCheckOut = checkInOuts.Count(x => x.CheckType == CheckStatus.CheckOut);
                    DateTime? lastCheckOut = checkInOuts.LastOrDefault(x => x.CheckType == CheckStatus.CheckOut)?.CheckTime;
                    DateTime? firstCheckIn = checkInOuts.FirstOrDefault(x => x.CheckType == CheckStatus.CheckIn)?.CheckTime;
                    TimeSpan? inTime = lastCheckOut - firstCheckIn;

                    ReportDtoByDateScope reportDto = new ReportDtoByDateScope()
                    {
                        UserId = id,
                        InCount = userCheckIn,
                        OutCount = userCheckOut,
                        InTimeCount = inTime,
                        DateTime = currentDate
                    };
                    reportDtos.Add(reportDto);
                }
                return reportDtos;
            }


            else
            {
                List<CheckInOut> userCheckInOuts = await _dbSet.Where(x => x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime()).ToListAsync();
                var groupedCheckInOuts = userCheckInOuts
                    .GroupBy(x => (x.AppUserID, x.CheckTime.Date))
                    .ToList();

                foreach (var group in groupedCheckInOuts)
                {
                    var keygroup = group.Key;
                    List<CheckInOut> checkInOuts = group.ToList();


                    int userCheckIn = checkInOuts.Count(x => x.CheckType == CheckStatus.CheckIn);
                    int userCheckOut = checkInOuts.Count(x => x.CheckType == CheckStatus.CheckOut);
                    DateTime? lastCheckOut = checkInOuts.LastOrDefault(x => x.CheckType == CheckStatus.CheckOut)?.CheckTime;
                    DateTime? firstCheckIn = checkInOuts.FirstOrDefault(x => x.CheckType == CheckStatus.CheckIn)?.CheckTime;
                    TimeSpan? inTime = firstCheckIn - lastCheckOut;

                    ReportDtoByDateScope reportDto = new ReportDtoByDateScope()
                    {
                        DateTime = keygroup.Date,
                        InCount = userCheckIn,
                        OutCount = userCheckOut,
                        InTimeCount = inTime,
                        UserId = keygroup.AppUserID
                    };
                    reportDtos.Add(reportDto);
                }
                return reportDtos;
            }

        }

        public async Task<bool> EnterAsync(CheckInOut checkInOut)
        {
            checkInOut.CheckTime = DateTime.Now.ToUniversalTime();
            checkInOut.CreatedDate = DateTime.Now.ToUniversalTime();
            await _dbSet.AddAsync(checkInOut);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> ExitAsync(CheckInOut checkInOut)
        {
            checkInOut.CheckTime = DateTime.Now.ToUniversalTime();
            checkInOut.CreatedDate = DateTime.Now.ToUniversalTime();
            await _dbSet.AddAsync(checkInOut);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<CheckInOut>> GetUserMovementsWithDate(int userId, DateTime? firstDate, DateTime? secondDate)
        {
            if (!firstDate.HasValue)
            {
                firstDate = secondDate.Value.AddDays(-7);
            }

            if (!secondDate.HasValue)
            {
                secondDate = firstDate.Value.AddDays(7);
            }

            if (userId != 0)
            {
                List<CheckInOut> checkIns = await _dbSet.Where(x => x.AppUserID == userId && x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn).ToListAsync();

                List<CheckInOut> checkOuts = await _dbSet.Where(x => x.AppUserID == userId && x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckOut).ToListAsync();

                List<CheckInOut> allChecks = new List<CheckInOut>();

                checkIns.ForEach(x => x.CheckTime.ToLocalTime());
                checkOuts.ForEach(x => x.CheckTime.ToLocalTime());

                allChecks.AddRange(checkIns);
                allChecks.AddRange(checkOuts);

                return allChecks;
            }

            else
            {
                List<CheckInOut> checkIns = await _dbSet.Where(x => x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn).ToListAsync();

                List<CheckInOut> checkOuts = await _dbSet.Where(x => x.CheckTime.ToUniversalTime() >= firstDate.Value.ToUniversalTime() && x.CheckTime.ToUniversalTime() <= secondDate.Value.ToUniversalTime() && x.CheckType == Entities.Entities.Enums.CheckStatus.CheckOut).ToListAsync();

                List<CheckInOut> allChecks = new List<CheckInOut>();

                checkIns.ForEach(x => x.CheckTime.ToLocalTime());
                checkOuts.ForEach(x => x.CheckTime.ToLocalTime());

                allChecks.AddRange(checkIns);
                allChecks.AddRange(checkOuts);

                return allChecks;
            }
        }
    }
}
