using Microsoft.EntityFrameworkCore;

namespace DAMS.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<DAMSDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for DAMSDbContext */
            dbContextOptions.UseMySQL(connectionString);
        }
    }
}
