using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://localhost:1234/api/
    [Route("api/[controller]")]
    //[Route("api/Regions")] -- SAME
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //GET:https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
         //Every return type wrap inside a task.
        {
            //Get data from database - Domain model
           var regionsDomain = await regionRepository.GetAllAsync();

            //Return DTOs to client
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        //Get Region by Id
        [HttpGet]
        [Route("{id:Guid}")]
        // FromRoute -- get from Route
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Way 2: Linq method
            //Get Region Domain model from Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound(); // 404
            }

            //Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //Post to create New region
        [HttpPost]
          public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
          {
            //id create internally
            //In post we receive a body from client

            //Map or convert  DTO to domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
          }


        //Update Region
        //PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain model to DTO -- return directly- shortcut
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        //Delete Region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}