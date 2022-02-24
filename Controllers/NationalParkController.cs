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
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _npRepo.GetNationalPark(nationalParkId);
            if(nationalPark == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<NationalParkDto>(nationalPark));
        }

        [HttpPost]
        public IActionResult NationalParkExist(string name)
        {
            if (!_npRepo.NationalParkExists(name))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult NationalParkExist(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
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

        [HttpPatch]
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
                ModelState.AddModelError("", $"Someting went wrong when saving the record {nationalParkDto.Name}");
                return StatusCode(404, ModelState);
            }
            return Ok(nationalPark);
        }

        [HttpDelete]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                ModelState.AddModelError("", "National park does not exist!");
                return StatusCode(404, ModelState);
            }

            var nationalPark = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Someting went wrong when deleting the record {nationalPark.Name}");
                return StatusCode(404, ModelState);
            }
            return Ok(nationalPark);
        }
    }
}
