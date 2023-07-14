using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.BLL.RepositoryPattern.IRepositories;
using TicimaxTask.DAL.PostgreSqlDb;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.BaseModels;

namespace TicimaxTask.BLL.RepositoryPattern.Repositories
{
    public class AppUserRepository:BaseRepository<AppUser>,IAppUserRepository
    {
        protected readonly PostgreSqlContext _context;
        private readonly DbSet<AppUser> _dbSet;

        public AppUserRepository(PostgreSqlContext context, DbSet<AppUser> dbSet): base(context, dbSet) 
        {
            _context = context;
            _dbSet = dbSet;
        }

        public async Task<Response<List<CheckInOut>>> GetUserMovementsWithDate(int userId, DateTime firstDate, DateTime secondDate)
        {
            

            try
            {
                return Response<List<CheckInOut>>.Success(_dbSet.FindAsync(userId).Result.Checks.Where(x => x.CheckTime >= firstDate && x.CheckTime <= secondDate).ToList(), 200);
            }
            catch (Exception ex)
            {
                return Response<List<CheckInOut>>.Fail($"{ex.Message}", 400, true);
            }
            
        }

        public Task<Response<AppUser>> GetUserReportsWithDate(int userId, DateTime firstDate, DateTime secondDate)
        {
            throw new NotImplementedException();
        }
    }
}
