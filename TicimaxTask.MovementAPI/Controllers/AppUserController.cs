using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicimaxTask.BLL.RepositoryPattern.IServices;
using TicimaxTask.BLL.RepositoryPattern.Services;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.Shared.ControllerBases;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.MovementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppUserController : CustomBaseController
    {
        private readonly IBaseService<AppUser> _appUserService;
        public AppUserController(IBaseService<AppUser> appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAppUser(AppUser appUser)
        {
            await _appUserService.CreateAsync(appUser);
            return CreateActionResultInstance(Response<AppUser>.Success(appUser,200));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppUser()
        {
            Response<List<AppUser>> appusers = await _appUserService.GetAllAsync();
            return CreateActionResultInstance(appusers);
        }

            
    }
}
