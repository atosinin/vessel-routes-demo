using System.ComponentModel.DataAnnotations;

namespace DotNetApi.Helpers
{
    public abstract class AuditableEntity
    {
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.MaxValue;
        [Required]
        public string LastModifiedBy { get; set; }
        [Required]
        public DateTime LastModifiedOn { get; set; } = DateTime.MaxValue;

        public AuditableEntity(string createdBy)
        {
            DateTime now = DateTime.UtcNow;
            CreatedBy = createdBy;
            CreatedOn = now;
            LastModifiedBy = createdBy;
            LastModifiedOn = now;
        }

        public virtual void UpdateLastModified(string modifiedBy)
        {
            LastModifiedOn = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
