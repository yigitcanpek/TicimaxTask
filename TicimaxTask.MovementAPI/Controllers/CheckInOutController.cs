﻿using Microsoft.AspNetCore.Http;
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
            //Response<AppUser?> checkOwnerUser = await _appUserService.GetByIdAsync(checkIn.AppUserID);
            //checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckIn;
            //_appUserService.Update(checkOwnerUser.Data);
            //checkIn.AppUser = checkOwnerUser.Data;
            //checkIn.CheckType = Entities.Entities.Enums.CheckStatus.CheckIn;
            //Response<bool> response = await _checkInOut.EnterAsync(checkIn);

        

            _rabbitMQPublisher.Publish(checkIn,"EnterExchange", "EnterRoute", "EnterQueue");

            return CreateActionResultInstance(Response<CheckInOut>.Success(checkIn,200));
        }

        [HttpPost]
        [Route("exit")]
        public async Task<IActionResult> Exit(CheckInOut checkOut)
        {
            //Response<AppUser?> checkOwnerUser = await _appUserService.GetByIdAsync(checkOut.AppUserID);
            //checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckOut;
            //_appUserService.Update(checkOwnerUser.Data);
            //checkOut.AppUser = checkOwnerUser.Data;
            //checkOut.CheckType = Entities.Entities.Enums.CheckStatus.CheckOut;
            //Response<bool> response = await _checkInOut.ExitAsync(checkOut);


            _rabbitMQPublisher.Publish(checkOut, "EnterExchange", "EnterRoute", "EnterQueue");

            return CreateActionResultInstance(Response<CheckInOut>.Success(checkOut, 200));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            
            

            Response<CheckInOut> checkInOut =  await _checkInOutService.GetByIdAsync(id);


            GetCheckInOutByIdDTO checkDto = new()
            {
                UserName = _appUserService.GetByIdAsync(checkInOut.Data.AppUserID).Result.Data.UserName,
                CheckTime = checkInOut.Data.CheckTime.ToLocalTime(),
                CheckType = checkInOut.Data.CheckType
            };
            

            return Ok(checkDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int personId,DateTime dateStart,DateTime dateEnd)
        {
            Response<List<CheckInOut>> report = await _checkInOut.GetUserMovementsWithDate(personId, dateStart, dateEnd);


            List<GetCheckInOutByIdDTO> checkDto = new();


            foreach (CheckInOut item in report.Data)
            {
                GetCheckInOutByIdDTO dto = new GetCheckInOutByIdDTO()
                {
                    UserName = _appUserService.GetByIdAsync(personId).Result.Data.Name,
                    CheckType = item.CheckType,
                    CheckTime = item.CheckTime
                };
                checkDto.Add(dto);
            }
                return CreateActionResultInstance(Response<List<GetCheckInOutByIdDTO>>.Success(checkDto,200));
        }

        [HttpGet]
        [Route("reports")]
        public async Task<IActionResult> GetReports(int personId, DateTime dateStart, DateTime dateEnd)
        {
           Response<List<ReportDto>> response =  await _checkInOut.GetReport(personId, dateStart, dateEnd);
            foreach (var item in response.Data)
            {
                item.UserName = _appUserService.GetByIdAsync(personId).Result.Data.UserName;
            }
            return CreateActionResultInstance(response);
        }
       
    }
}
