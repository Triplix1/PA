using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Lab3.Models
{
    public class Row
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RowId { get; set; }
        public string Value { get; set; }
    }
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
    public class AVL
    {
        Node Root;
        public AVL(INodeRepository repos)
        {
            if (repos.Nodes != null)
            {
                foreach (Row row in repos.Nodes)
                {
                    Add(new Node { Row = row });
                }
            }
        }
        public void Add(Node data)
        {
            Root = Add(Root, data);
        }
        private Node Add(Node current, Node n)
        {
            if (current == null)
            {
                current = n;
                return current;
            }
            else if (n.Row.RowId < current.Row.RowId)
            {
                current.Left = Add(current.Left, n);
                current = BalanceTree(current);
            }
            else if (n.Row.RowId > current.Row.RowId)
            {
                current.Right = Add(current.Right, n);
                current = BalanceTree(current);
            }
            return current;
        }
        private Node BalanceTree(Node current)
        {
            int b_factor = HeightDifference(current);
            if (b_factor > 1)
            {
                if (HeightDifference(current.Left) > 0)
                {
                    current = SmallRight(current);
                }
                else
                {
                    current = BigRight(current);
                }
            }
            else if (b_factor < -1)
            {
                if (HeightDifference(current.Right) > 0)
                {
                    current = BigLeft(current);
                }
                else
                {
                    current = SmallLeft(current);
                }
            }
            return current;
        }
        public void Delete(int target)
        {//and here
            Root = Delete(Root, target);
        }
        private Node Delete(Node current, int target)
        {
            Node parent;
            if (current == null)
            { return null; }
            else
            {
                //left subtree
                if (target < current.Row.RowId)
                {
                    current.Left = Delete(current.Left, target);
                    current = BalanceTree(current);
                }
                //right subtree
                else if (target > current.Row.RowId)
                {
                    current.Right = Delete(current.Right, target);
                    current = BalanceTree(current);
                }
                //if target is found
                else
                {
                    if (current.Right != null)
                    {
                        //delete its inorder successor
                        parent = current.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        current.Row.RowId = parent.Row.RowId;
                        current.Row.Value = parent.Row.Value;
                        current.Right = Delete(current.Right, parent.Row.RowId);
                        current = BalanceTree(current);
                    }
                    else
                    {   //if current.left != null
                        return current.Left;
                    }
                }
            }
            return current;
        }
        public Node Find(int key, ref int i)
        {
            return Find(key, Root, ref i);
        }

        public Node Find(int key, Node node, ref int i)
        {
            i++;
            if (node == null) return null;

            if (key.CompareTo(node.Row.RowId) < 0)
            {
                node = Find(key, node.Left, ref i);
            }
            else if (key.CompareTo(node.Row.RowId) > 0)
            {
                node = Find(key, node.Right, ref i);
            }

            return node;
        }

        public void Edit( Row row)
        {
            int i = 0;
            var result = Find(row.RowId, ref i);
            result.Row.Value = row.Value;
        }
        public List<Row> ToList()
        {
            List<Row> nodes = new List<Row>();
            ToList(Root, nodes);
            return nodes;
        }
        private void ToList(Node current, List<Row> list)
        {
            if (current != null)
            {
                ToList(current.Left, list);
                list.Add(current.Row);
                ToList(current.Right, list);
            }
        }
        private int HeightDifference(Node current)
        {
            int l = Height(current.Left);
            int r = Height(current.Right);             
            return l - r;
        }
        private Node SmallLeft(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }
        private Node SmallRight(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }
        private Node BigRight(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = SmallLeft(pivot);
            return SmallRight(parent);
        }
        private Node BigLeft(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = SmallRight(pivot);
            return SmallLeft(parent);
        }

        //public bool CheckBalance(ref int i)
        //{
        //    return CheckBalance(Root, ref i);
        //}
        //bool CheckBalance(Node current, ref int i)
        //{
        //    i++;
        //    if (current == null)
        //    {
        //        return true;
        //    }
        //    if (Math.Abs(HeightDifference(current)) <= 1)
        //    {
        //        return CheckBalance(current.Left, ref i) && CheckBalance(current.Right, ref i);
        //    }
        //    return false;
        //}

        private int Height(Node node)
        {
            if (node == null)
            {
                return 0;
            }
            return 1 + Math.Max(Height(node.Left), Height(node.Right));
        }        
    }
}
