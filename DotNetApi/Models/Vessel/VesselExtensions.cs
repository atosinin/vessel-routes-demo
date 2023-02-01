namespace DotNetApi.Models
{
    public static class VesselExtensions
    {
        public static void CreateFromVesselDTO(this Vessel vessel, VesselDTO vesselDTO)
        {
            vessel.UpdateFromVesselDTO(vesselDTO);
        }

        public static void UpdateFromVesselDTO(this Vessel vessel, VesselDTO vesselDTO)
        {
            vessel.Name = vesselDTO.Name;
        }
    }
}
