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
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        
        public AppUserRepository(PostgreSqlContext context) : base(context)
        {
           
        }

  

        public Task<AppUser> GetUserReportsWithDate(int userId, DateTime firstDate, DateTime secondDate)
        {
            throw new NotImplementedException();
        }
    }
}
