using System.Collections.Generic;
using System.Linq;

namespace Lab3.Models
{
    public class EFNodesRepository : INodeRepository
    {
        ApplicationDbContext context;
        public EFNodesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Build(List<Row> nodes)
        {
            context.Rows.RemoveRange(context.Rows.ToArray());
            context.Rows.AddRange(nodes);
            context.SaveChanges();
        }
        public IEnumerable<Row> Nodes => context.Rows;
    }
}