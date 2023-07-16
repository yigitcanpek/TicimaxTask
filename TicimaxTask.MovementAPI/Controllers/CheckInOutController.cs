using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TicimaxTask.BLL.RepositoryPattern.IServices;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MovementAPI.DTOs;
using TicimaxTask.MovementAPI.RabbitMq.PublisherServices;
using TicimaxTask.Shared.ControllerBases;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.MovementAPI.Controllers
{

    public class CheckInOutController : CustomBaseController
    {
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        private readonly IBaseService<AppUser> _appUserService;
        private readonly IBaseService<CheckInOut> _checkInOutService;
        private readonly ICheckInOutService _checkInOut;
        TimeZoneInfo easternEuropeanTimeZone = TimeZoneInfo.CreateCustomTimeZone("Eastern European Standard Time", TimeSpan.FromHours(3), "Eastern European Standard Time", "Eastern European Standard Time");

        public CheckInOutController(IBaseService<AppUser> appUserService, IBaseService<CheckInOut> checkInOutService, ICheckInOutService checkInOut, RabbitMQPublisher rabbitMQPublisher)
        {
            _appUserService = appUserService;
            _checkInOutService = checkInOutService;
            _checkInOut = checkInOut;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        [HttpPost]
        [Route("enter")]
        public async Task<IActionResult> Enter(CheckInOut checkIn)
        {
            try
            {
                Response<AppUser?> checkOwnerUser = await _appUserService.GetByIdAsync(checkIn.AppUserID);
                checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckIn;
                _appUserService.Update(checkOwnerUser.Data);
                checkIn.AppUser = checkOwnerUser.Data;
                checkIn.CheckType = Entities.Entities.Enums.CheckStatus.CheckIn;



                _rabbitMQPublisher.Publish(checkIn, "EnterExitExchange", "EnterExitRoute", "EnterExitQueue");

                return CreateActionResultInstance(Response<CheckInOut>.Success(checkIn, 200));
            }
            catch (Exception ex)
            {

                return CreateActionResultInstance(Response<CheckInOut>.Fail($"{ex.Message}", 400));
            }
        }

        [HttpPost]
        [Route("exit")]
        public async Task<IActionResult> Exit(CheckInOut checkOut)
        {

            try
            {
                checkOut.CheckType = Entities.Entities.Enums.CheckStatus.CheckOut;


                _rabbitMQPublisher.Publish(checkOut, "EnterExitExchange", "EnterExitRoute", "EnterExitQueue");

                return CreateActionResultInstance(Response<CheckInOut>.Success(checkOut, 200));
            }
            catch (Exception ex)
            {

                return CreateActionResultInstance(Response<IActionResult>.Fail($"{ex.Message}", 400));
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Response<CheckInOut> checkInOut = await _checkInOutService.GetByIdAsync(id);


                GetCheckInOutByIdDTO checkDto = new()
                {
                    UserName = _appUserService.GetByIdAsync(checkInOut.Data.AppUserID).Result.Data.UserName,
                    CheckTime = TimeZoneInfo.ConvertTime(checkInOut.Data.CheckTime, easternEuropeanTimeZone),
                    CheckType = checkInOut.Data.CheckType
                };


                return CreateActionResultInstance(Response<GetCheckInOutByIdDTO>.Success(checkDto, 200));
            }
            catch (Exception ex)
            {

                return CreateActionResultInstance(Response<GetCheckInOutByIdDTO>.Fail($"{ex.Message}", 400));
            }


          
        }

        [HttpGet]
        public async Task<IActionResult> Get(int personId, DateTime? dateStart, DateTime? dateEnd)
        {
            try
            {
                Response<List<CheckInOut>> report = await _checkInOut.GetUserMovementsWithDate(personId, dateStart, dateEnd);

                if (report.Data == null)
                {
                    return CreateActionResultInstance(Response<List<GetCheckInOutByIdDTO>>.Success(null, 200));
                }

                List<GetCheckInOutByIdDTO> checkDto = new();
                foreach (CheckInOut item in report.Data)
                {
                    GetCheckInOutByIdDTO dto = new GetCheckInOutByIdDTO()
                    {
                        UserName = _appUserService.GetByIdAsync(item.AppUserID).Result.Data.Name,
                        CheckType = item.CheckType,
                        CheckTime = TimeZoneInfo.ConvertTime(item.CheckTime, easternEuropeanTimeZone),
                    };
                    checkDto.Add(dto);

                }
                return CreateActionResultInstance(Response<List<GetCheckInOutByIdDTO>>.Success(checkDto, 200));
            }
            catch (Exception ex)
            {

                return CreateActionResultInstance(Response<IActionResult>.Fail($"{ex.Message}", 400));
            }
        }

        [HttpGet]
        [Route("reports")]
        public async Task<IActionResult> GetReports(int personId, DateTime? dateStart, DateTime? dateEnd)
        {
            try
            {
                Response<List<ReportDtoByDateScope>> response = await _checkInOut.GetReport(personId, dateStart, dateEnd);

                if (response.Data == null)
                {
                    return CreateActionResultInstance(Response<List<GetCheckInOutByIdDTO>>.Success(null, 200));
                }

                foreach (var item in response.Data)
                {
                    item.DateTime = TimeZoneInfo.ConvertTime(item.DateTime, easternEuropeanTimeZone);
                    item.UserName = _appUserService.GetByIdAsync(item.UserId).Result.Data.UserName;


                }
                return CreateActionResultInstance(response);
            }
            catch (Exception ex)
            {

                return CreateActionResultInstance(Response<IActionResult>.Fail($"{ex.Message}", 400));
            }
        }

    }
}
