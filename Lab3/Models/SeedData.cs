using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!(context.Rows.Any()))
            {
                //context.Rows.AddRange(
                //    new Row { RowId = 1,Value ="aaa" },
                //    new Row { RowId = 2, Value = "bbb" },
                //    new Row { RowId = 3, Value = "ccc" },
                //    new Row { RowId = 4, Value = "ddd" },
                //    new Row { RowId = 5, Value = "eee" },
                //    new Row { RowId = 6, Value = "fff" }                  
                //);
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