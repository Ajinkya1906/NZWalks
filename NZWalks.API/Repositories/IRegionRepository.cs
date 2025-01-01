using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        //Definition of method i want to expose
       Task<List<Region>> GetAllAsync();
        //Task: Represents an asynchronous operation 

        Task<Region> GetByIdAsync(Guid id);
        //It can be nullable ?


        Task<Region> CreateAsync(Region region);

       Task<Region?> UpdateAsync(Guid id, Region region); 

        Task<Region?> DeleteAsync(Guid id);
    }
}
