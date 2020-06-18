using Abp.EntityFrameworkCore;
using DAMS.EventReminder;
using DAMS.EventReminder.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DAMS.EntityFrameworkCore
{
    public class DAMSDbContext : AbpDbContext
    {
        private DbContextOptions<DbContext> contextOptions;

        //Add DbSet properties for your entities...
        public virtual DbSet<OneTimeEvent> OneTimeEvents { get; set; }
        public virtual DbSet<CustomEvent> CustomEvents { get; set; }
        public virtual DbSet<PeriodEvent> PeriodEvents { get; set; }

        public DAMSDbContext(DbContextOptions<DAMSDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomEvent>()
                .Property(b => b.Dates).HasConversion
                (v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<DateTime, EventStatus>>(v));
        }


    }
}
