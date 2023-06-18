namespace DataAccess.DTOs
{
    public class Flight
    {
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public Transport Transport { get; set; }
        public int Price { get; set; }
    }
}
