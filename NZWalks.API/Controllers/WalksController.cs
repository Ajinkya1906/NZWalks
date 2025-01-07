using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
            //Ctrl + . -->Create and assign field
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        public IWalkRepository WalkRepository { get; }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTo to domain model
            //map AddWalkRequestDto to Walk domain model
             var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

             await walkRepository.CreateAsync(walkDomainModel);

             //Map domain model to DTO
             return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //GET Walks
        //GET:/api/walks?filterOn=Name?filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn,filterQuery);

            //Map Domain model to DTO
            //List-have multiple
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }


        //Get by Id
        //GET: /api/Walks/{id}
        //[HttpGet]
        //[Route("{id:Guid}")]

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update walk by Id
        //put /api/Walks/{id}
        //[HttpPut]
        //[Route("{id:Guid}")]

        [HttpPut("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
                //Map DTO to domain model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                //Map Domain Model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));       
        }


        //Delete walk by Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteAsync(id);

            if(deleteWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }


    }
}

