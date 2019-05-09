using System;
using System.Collections.Generic;
using System.Text;
using DataBroker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataBroker.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
	}
}
