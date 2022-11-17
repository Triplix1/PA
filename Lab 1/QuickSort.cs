using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_1
{
    public class Quick3way
    {
        public static void sort(int[] a, int lo, int hi)
        {
            if (lo >= hi) return;
            int lt = lo; int i = lo + 1; int gt = hi;
            int t = a[lo];
            while (i <= gt)
            {
                int cmp = a[i].CompareTo(t);
                if (cmp < 0) { exch(a, i++, lt++); }
                else if (cmp > 0) { exch(a, i, gt--); }
                else { i++; }
            }
            sort(a, lo, lt - 1);
            sort(a, gt + 1, hi);
        }

        public static void exch(int[] a, int i, int j)
        {
            int t = a[i];
            a[i] = a[j];
            a[j] = t;
        }
        public static void sort(int[] a)
        {
            for (int i = 0; i < a.Length-1; i++)
            {
                if (a[i] > a[i+1])
                {
                    sort(a, 0, a.Length - 1);
                    break;
                }
            }            
        }
    }
    
}
