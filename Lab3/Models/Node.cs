namespace Lab3.Models
{
    public class Node
    {
        public Row Row { get; set; }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public Node Parent { get; set; }

        public Node() { }

        public Node(Node parent, Row row)
        {
            Parent = parent;
            Row = row;
        }
    }
}
