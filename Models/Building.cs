namespace SecurityDemo.Models
{
    public class Building
    {
        public int buildingId { get; set; }  // Primary key!
        public string name { get; set; }
        public int cityId { get; set; }  // Foreign key!
        public List<Room> rooms { get; set; }
        public City city { get; set; }


    }
}
