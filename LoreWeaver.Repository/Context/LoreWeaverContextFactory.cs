using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LoreWeaver.Repository.Data
{
    public class LoreWeaverContextFactory : IDesignTimeDbContextFactory<LoreWeaverContext>
    {
        public LoreWeaverContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LoreWeaverContext>();
            optionsBuilder.UseSqlServer(@"Server=HEROBSSS\SQLEXPRESS;Database=LoreWeaver;Trusted_Connection=True;TrustServerCertificate=True;");

            return new LoreWeaverContext(optionsBuilder.Options);
        }
    }
}