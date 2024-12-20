using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
//using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
      

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItem = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

       // [HttpGet("{id}", Name = "GetPlatformById")]
        [HttpGet("GetPlatformById/{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id=3)
        {
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost("CreatePlatform")]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
           // return  Ok(platformReadDto);
            // // Send Sync Message
            // try
            // {
            //     await _commandDataClient.SendPlatformToCommand(platformReadDto);
            // }
            // catch(Exception ex)
            // {
            //     Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            // }

            // //Send Async Message
            // try
            // {
            //     var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            //     platformPublishedDto.Event = "Platform_Published";
            //     _messageBusClient.PublishNewPlatform(platformPublishedDto);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            // }

           // return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
           return CreatedAtAction(nameof(GetPlatformById),new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}