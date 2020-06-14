using Abp.EntityFrameworkCore;
using DAMS.EventReminder.Event;
using Microsoft.EntityFrameworkCore;

namespace DAMS.EntityFrameworkCore
{
    public class DAMSDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        public virtual DbSet<OneTimeEvent> OneTimeEvents { get; set; } 


        public DAMSDbContext(DbContextOptions<DAMSDbContext> options) 
            : base(options)
        {
            
        }
    }
}
