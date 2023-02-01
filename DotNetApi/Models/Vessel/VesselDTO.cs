using DotNetApi.Helpers;

namespace DotNetApi.Models
{
    public class VesselDTO
    {
        public int VesselId { get; set; }
        public string Name { get; set; } = string.Empty;
        List<PositionDTO> Positions { get; set; } = new();
    }
}