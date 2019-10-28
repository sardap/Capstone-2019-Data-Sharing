using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBroker.Models
{
    public class MedicalCondition
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Severity { get; set; }

        [Required]
        public string Type { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
