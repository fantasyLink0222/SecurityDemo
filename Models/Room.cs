namespace SecurityDemo.Models
{
    public class Room
    {
        public int roomId { get; set; }      // Primary key!
        public string name { get; set; }
        public int capacity { get; set; }
        public int buildingId { get; set; }  // Foreign key!
        public Building building { get; set; }

    }
}
