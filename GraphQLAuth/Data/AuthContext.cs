using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLAuth.Data
{
    public class AuthContext : DbContext
    {

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
    }
}

