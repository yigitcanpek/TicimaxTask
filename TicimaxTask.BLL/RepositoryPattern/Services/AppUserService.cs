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
    public class AppUserService : BaseService<AppUser>, IAppUserService
    {
        IAppUserRepository _appUser;
        public AppUserService(IAppUserRepository appUser) : base(appUser)
        {
            _appUser = appUser;
        }

    }
}
