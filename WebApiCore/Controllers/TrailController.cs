using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using WebApiCore.Models;
using WebApiCore.Models.DTOs;
using WebApiCore.Repository.IRepository;

namespace WebApiCore.Controllers
{
    [Route("api/trail")]
    [ApiController]
    [Authorize]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            var trailDtoList = _trailRepository.GetTrails().Select(_mapper.Map<Trail, TrailDto>);
            return Ok(trailDtoList);
        }
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            var trailDto = _mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if (trailDto == null) return BadRequest();
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", $"Trail in use!!!{trailDto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var trail = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"SOMETHING WENT WRONG WHILE SAVEING TRAIL: {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto)
        {
            if (trailDto == null) return BadRequest();
            var trail = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"SOMETHING WENT WRONG WHILE update data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            var trailInDb = _trailRepository.GetTrail(trailId);
            if (trailInDb == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trailInDb))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data:{trailInDb.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }


    }
}

