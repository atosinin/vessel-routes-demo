namespace DotNetApi.Models
{
    public static class PositionExtensions
    {
        public static void CreateFromPositionDTO(this Position position, PositionDTO positionDTO)
        {
            position.UpdateFromPositionDTO(positionDTO);
        }

        public static void UpdateFromPositionDTO(this Position position, PositionDTO positionDTO)
        {
            position.X = positionDTO.X;
            position.Y = positionDTO.Y;
            position.Timestamp = positionDTO.Timestamp;
        }
    }
}
