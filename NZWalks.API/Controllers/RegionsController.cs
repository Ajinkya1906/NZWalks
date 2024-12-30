using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    //https://localhost:1234/api/
    [Route("api/[controller]")]
    //[Route("api/Regions")] -- SAME
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        //This declares a private field named dbContext of type NZWalksDbContext.
        //The readonly modifier ensures that this field can only be assigned during its initialization or in the constructor.
        //It’s used throughout the RegionsController class to interact with the database.

        public RegionsController(NZWalksDbContext dbContext)
        //NZWalksDbContext dbContext--dbContext Parameter:This parameter is an instance of the NZWalksDbContext class. It is injected automatically by .NET's DI container.
        {
            this.dbContext = dbContext;
            //The dbContext parameter is assigned to the private field this.dbContext, making it available to all methods in the class.
        }

        //NZWalksDbContext - Represents the database connection and access layer.

        //GET:https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
         //Every return type wrap inside a task.
        {
            // Regions table
            //Get data from database - Domain model
           var regionsDomain = await dbContext.Regions.ToListAsync();

            //Map domain model to DTO
            var regionDto = new List<RegionDto>();

            //loop on regions and convert to regionDto
            foreach (var regionDomain in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }
            //Return DTOs to client
            return Ok(regionDto);
        }


        //Get Region by Id
        //GET:https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        // FromRoute -- get from Route
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Method 1 
            //find only take primary key like id
            //var region = dbContext.Regions.Find(id);

            //Way 2: Linq method
            //Get Region Domain model from Database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
            {
                return NotFound(); // 404
            }

            // Map/Convert Region Domain Model to Region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO back to client
            return Ok(regionDto);
        }

        //Post to create New region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
          public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
          {
            //id create internally
            //In post we receive a body from client

            //Map or convert  DTO to domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Use Domain model to create Region
           await dbContext.Regions.AddAsync(regionDomainModel);
           //dbContext.SaveChanges();  //Save in Database
           await dbContext.SaveChangesAsync();  //Save in Database

            //Map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //CreatedAtAction -- 201
            //GetById maps to /api/regions/{id}
            //Example: If the Id is 123, the server might return /api/regions/123 --- new { id = regionDto.Id }
            //regionDto- This is the data (DTO)of the newly created region.It is included in the response body so that the client receives the details of the new region.
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id },
               regionDto);
          }


        //Update Region
        //PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map DTO to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            //Convert Domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }


        //Delete Region
        //DELETE https://localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Delete Region
            dbContext.Regions.Remove(regionDomainModel); // We cant have removeAsync method
            await dbContext.SaveChangesAsync();

            //optional- return deleted region back
            //Map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}