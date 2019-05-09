using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace blockchain_policy_deployer.Models
{
    public class UserPolicyContext :  DbContext
    {
        public UserPolicyContext(DbContextOptions<UserPolicyContext> options)
            : base(options)
        {
        }

        public UserPolicy incomingPolicy; 

        public DbSet<UserPolicy> UserPolicies { get; set; }
    }
}
