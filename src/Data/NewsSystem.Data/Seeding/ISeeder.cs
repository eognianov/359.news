using System;
using System.Threading.Tasks;

namespace NewsSystem.Data.Seeding
{
    public interface ISeeder
    {
        void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}