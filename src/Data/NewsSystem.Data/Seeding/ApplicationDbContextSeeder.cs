﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NewsSystem.Data.Seeding
{
    public static class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new SourcesSeeder(),
                new MainNewsSourcesSeeder(),
                new WorkerTasksSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(dbContext, serviceProvider);
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
                dbContext.SaveChanges();
            }
        }
    }
}
