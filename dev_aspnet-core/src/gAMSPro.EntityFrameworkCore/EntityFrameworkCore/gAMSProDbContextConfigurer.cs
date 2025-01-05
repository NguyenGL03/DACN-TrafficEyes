using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace gAMSPro.EntityFrameworkCore
{
    public static class gAMSProDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<gAMSProDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<gAMSProDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}