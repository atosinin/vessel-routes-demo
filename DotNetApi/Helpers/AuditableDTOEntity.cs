namespace DotNetApi.Helpers
{
    public abstract class AuditableDTOEntity
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.MaxValue;
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime LastModifiedOn { get; set; } = DateTime.MaxValue;
    }
}
