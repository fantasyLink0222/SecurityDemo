namespace SecurityDemo.Models
{
    public class City
    {
        public int cityId { get; set; }  // Primary key!
        public string cityName { get; set; }
        public List<Building> buildings { get; set; }

    }
}
