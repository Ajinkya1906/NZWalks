﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

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
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTo to domain model
            //map AddWalkRequestDto to Walk domain model
           var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            //Map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await walkRepository.GetAllAsync();

            //Map Domain model to DTO
            //List-have multiple
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }


        //Get by Id
        //GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid Id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(Id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


    
    }
}
