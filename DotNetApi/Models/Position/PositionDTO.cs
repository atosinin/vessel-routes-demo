namespace DotNetApi.Models
{
    public class PositionDTO
    {
        public int PositionId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.MaxValue;
        public int VesselId { get; set; }
    }
}