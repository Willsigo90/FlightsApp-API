namespace DataAccess.DTOs
{
    public class FlightDto
    {
        public string? DepartureStation { get; set; }
        public string? ArrivalStation { get; set; }
        public string? FlightCarrier { get; set; }
        public string? FlightNumber { get; set; }
        public int Price { get; set; }
    }
}
