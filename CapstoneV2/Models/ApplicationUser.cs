using CapstoneV2.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneV2.Models
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        [Required]
		public string Gender { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

		public Ethnicity? Ethnicities { get; set; }

        public List<MedicalCondition> MedicalConditions { get; set; }
	}
}
