using Microsoft.EntityFrameworkCore;
using SampleApp.Persistence.Infrastructure;

namespace SampleApp.Persistence
{
    public class SampleAppDbFactory : AbstractSampleAppDbFactory<SampleAppDbContext>
    {
        protected override SampleAppDbContext CreateNewInstance(DbContextOptions<SampleAppDbContext> options)
        {
            return new SampleAppDbContext(options);
        }
    }
}