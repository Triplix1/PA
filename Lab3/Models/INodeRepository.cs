using System.Collections.Generic;
using System.Linq;

namespace Lab3.Models
{
    public interface INodeRepository
    {
        void Build(List<Row> nodes);
        IEnumerable<Row> Nodes { get; }
    }
}
