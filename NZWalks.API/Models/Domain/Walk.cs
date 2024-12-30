namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        //DifficultyId is foreign key
        public Guid RegionId { get; set; }
        //Navigation Properties
        public Difficulty Difficulty { get; set; }
        //Point 1.1
        public Region Region { get; set; }
    }
}
