using gAMSPro.EntityFrameworkCore;

namespace gAMSPro.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly gAMSProDbContext _context;

        public InitialHostDbBuilder(gAMSProDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
