
using Microsoft.EntityFrameworkCore;
using ModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelService
{
    public class ApplicationAPSWCCDbContext:DbContext
    {
        public ApplicationAPSWCCDbContext(DbContextOptions<ApplicationAPSWCCDbContext> context) : base(context)
        {
           
        }
        public DbSet<Captch> captcha { get; set; }
    }
}
