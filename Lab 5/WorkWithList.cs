﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_5
{
    public class WorkWithList
    {
        public static void Add<T>(List<T> arr, T node, Func<T, int> func)//Додавання грального поля в відсортований список
        {
            if (arr is null || node is null || func is null)
            {
                throw new ArgumentNullException("Parameters can't be null");
            }

            int start = 0;//Ліва границя
            int end = arr.Count - 1;// Права границя

            while (start <= end)
            {
                int mid = (start + end) / 2;// Середина

                if (func(arr[mid]) == func(node))
                {
                    arr.Insert(mid, node);
                    return;
                }
                else if (func(arr[mid]) < func(node))
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            arr.Insert(end + 1, node);
        }
    }
}