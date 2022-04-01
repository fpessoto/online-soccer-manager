using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context
{
    public class DesignTimeOnlineSoccerContextFactory : IDesignTimeDbContextFactory<OnlineSoccerDBContext>
    {
        public OnlineSoccerDBContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<OnlineSoccerDBContext> optionsBuilder = new DbContextOptionsBuilder<OnlineSoccerDBContext>();

            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineSoccer;Persist Security Info=True; Integrated Security=True;",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new OnlineSoccerDBContext(optionsBuilder.Options);
        }
    }
}
