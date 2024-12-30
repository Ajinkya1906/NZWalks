namespace NZWalks.API.Models.DTO
{
    public class RegionDto
    {
        //Properties expose to client - add in DTO(from domain)
        //Domain not expose to client - best practise
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
