using DotNetApi.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetApi.Models
{
    public class Whatever : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WhateverId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }


        // Relationnal properties

        [ForeignKey("UserAccount")]
        public string UserAccountId { get; set; } = string.Empty;
        public UserAccount UserAccount { get; set; } = null!;

        // Constructor

        public Whatever(string createdBy) : base(createdBy)
        {
        }
    }
}