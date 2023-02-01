using DotNetApi.Helpers;

namespace DotNetApi.Models
{
    public class WhateverDTO : AuditableDTOEntity
    {
        public int WhateverId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string UserAccountId { get; set; } = string.Empty;
    }
}