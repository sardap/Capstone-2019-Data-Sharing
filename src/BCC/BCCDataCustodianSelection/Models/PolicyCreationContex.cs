using Microsoft.EntityFrameworkCore;

namespace BCCDataCustodianSelection.Models
{
    public class PolicyCreationContex : DbContext
    {
        public PolicyCreationContex(DbContextOptions<PolicyCreationContex> options)
            : base(options)
        {
        }

        public DbSet<PolicyCreationContex> TodoItems { get; set; }
    }
}