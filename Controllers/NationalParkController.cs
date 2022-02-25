using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using TestProjectCore.Model;
using TestProjectCore.Model.DTOs;
using TestProjectCore.Repository;

namespace TestProjectCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// For getting national park list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalParkDtoList = new List<NationalParkDto>();
            var nationalParkList = _npRepo.GetNationalParks();

            foreach (var nationalPark in nationalParkList)
            {
                nationalParkDtoList.Add(_mapper.Map<NationalParkDto>(nationalPark));
            }
            return Ok(nationalParkDtoList);
        }
        /// <summary>
        /// For getting national park with nationalParkId
        /// </summary>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _npRepo.GetNationalPark(nationalParkId);
            if(nationalPark == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<NationalParkDto>(nationalPark));
        }
        /// <summary>
        /// If any exist with the given name
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult NationalParkExist(string name)
        {
            if (!_npRepo.NationalParkExists(name))
            {
                return NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// If any exist with the given id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult NationalParkExist(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// Create new national park
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark(NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_npRepo.NationalParkExists(nationalParkDto.Name) || _npRepo.NationalParkExists(nationalParkDto.Id))
            {
                ModelState.AddModelError("", "National park already exists!");
                return StatusCode(404, ModelState);
            }
            
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Someting went wrong when saving the record {nationalParkDto.Name}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id }, nationalPark);
        }
        /// <summary>
        /// Update existing national park
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(200, Type=typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_npRepo.NationalParkExists(nationalParkDto.Name) || !_npRepo.NationalParkExists(nationalParkDto.Id))
            {
                ModelState.AddModelError("", "National park does not exist!");
                return StatusCode(404, ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Someting went wrong when saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok(nationalParkDto);
        }
        /// <summary>
        /// Delete existing national park by id
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            var nationalPark = _npRepo.GetNationalPark(nationalParkId);

            if (nationalPark == null)
            {
                ModelState.AddModelError("", "National park does not exist!");
                return StatusCode(404, ModelState);
            }

            if (!_npRepo.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Someting went wrong when deleting the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);

            return Ok(nationalParkDto);
        }
    }
}
