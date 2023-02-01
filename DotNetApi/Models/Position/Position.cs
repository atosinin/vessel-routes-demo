using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetApi.Models
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.MaxValue;


        // Relationnal properties

        [ForeignKey("Vessel")]
        public int VesselId { get; set; }
        public Vessel Vessel { get; set; } = null!;
    }
}