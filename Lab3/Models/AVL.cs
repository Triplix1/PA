using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lab3.Models
{
    public class AVL
    {
        Node Root;

        public AVL() { }

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
            if (data != null && data.Row != null)
            {
                Root = Add(Root, data);
            }            
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
            int b_factor = GetHeightDifference(current);
            if (b_factor > 1)
            {
                if (GetHeightDifference(current.Left) > 0)
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
                if (GetHeightDifference(current.Right) > 0)
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
        {            
            Root = Delete(Root, target);
        }

        private Node Delete(Node current, int target)
        {
            Node parent;

            if (current == null)
            { 
                return null; 
            }
            else
            {
                if (target < current.Row.RowId)
                {
                    current.Left = Delete(current.Left, target);
                    current = BalanceTree(current);
                }
                else if (target > current.Row.RowId)
                {
                    current.Right = Delete(current.Right, target);
                    current = BalanceTree(current);
                }
                else
                {
                    if (current.Right != null)
                    {
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
                    {
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

        public Node Find(int key, Node node, ref int comparesNumer)
        {
            comparesNumer++;

            if (node == null) return null;

            if (key.CompareTo(node.Row.RowId) < 0)
            {
                node = Find(key, node.Left, ref comparesNumer);
            }
            else if (key.CompareTo(node.Row.RowId) > 0)
            {
                node = Find(key, node.Right, ref comparesNumer);
            }

            return node;
        }

        public void Edit(Row row)
        {
            if (row != null)
            {
                int i = 0;

                var result = Find(row.RowId, ref i);

                if (result != null)
                {
                    result.Row.Value = row.Value;
                }                
            }            
        }
        public List<Row> GetRowsList()
        {
            List<Row> nodes = new List<Row>();

            GetRowsList(Root, nodes);

            return nodes;
        }
        private void GetRowsList(Node current, List<Row> list)
        {
            if (current != null)
            {
                GetRowsList(current.Left, list);
                list.Add(current.Row);
                GetRowsList(current.Right, list);
            }
        }
        private int GetHeightDifference(Node current)
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
