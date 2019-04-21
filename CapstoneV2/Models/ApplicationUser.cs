using CapstoneV2.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneV2.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Gender { get; set; }

		public DateTime Birthday { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public Ethnicity Ethnicities { get; set; }
	}
}
