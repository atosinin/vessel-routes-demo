namespace DotNetApi.Models
{
    public class VesselRoutesImport
    {
        public List<RouteImport> vessels = new();
    }

    public class RouteImport
    {
        public string name { get; set; } = string.Empty;
        public List<PositionImport> positions = new();
    }

    public class PositionImport
    {
        public int x { get; set; }
        public int y { get; set; }
        public string timestamp { get; set; } = string.Empty;
    }
}
