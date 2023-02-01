using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetApi.Models
{
    public class Vessel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VesselId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Relationnal properties

        List<Position> Positions { get; set; } = new();
    }
}