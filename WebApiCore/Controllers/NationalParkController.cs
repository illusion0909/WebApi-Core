using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using WebApiCore.Data;
using WebApiCore.Models;
using WebApiCore.Models.DTOs;
using WebApiCore.Repository.IRepository;

namespace WebApiCore.Controllers
{
    [Route("api/nationalpark")]
    [ApiController]
  //  [Authorize]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDtoList = _nationalParkRepository.GetNationalParks().Select(_mapper.Map<NationalPark, NationalParkDto>);
            return Ok(nationalParkDtoList);
        }
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]//ADD NAME FOR.....
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();//404
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);//200
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);//400
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Already in database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nationalPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while save data!!{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok(nationalPark);
            return CreatedAtRoute("GetNationalPark", new { nationalparkId = nationalPark.Id }, nationalPark);//201
        }
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while save data!!{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return ok();
            return NoContent();//204
        }
        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();//404
            }
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data!!{nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();//200 success
        }
    }
}
