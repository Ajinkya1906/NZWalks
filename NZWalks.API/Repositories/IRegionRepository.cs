using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        //Definition of method i want to expose
       Task<List<Region>> GetAllAsync();
        //Task: Represents an asynchronous operation 
    
    
    
    }
}
