using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models
{
    public class SeedData
    {
        public void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            if (!(context.Rows.Any()))
            {
                for (int i = 1; i <= 10000; i++)
                {
                    context.Rows.Add(new Row
                    {
                        RowId = i,
                        Value = i.ToString()
                    });
                }

                context.SaveChanges();
            }
        }
    }
}