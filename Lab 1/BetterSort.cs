using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace Lab_1
{
    internal class BetterSort
    {
        public string FileName { get; set; }
        private long length;
        public BetterSort(string path)
        {
            FileName = path;
            using (BinaryReader br = new BinaryReader(File.OpenRead(FileName)))
            {
                length = br.BaseStream.Length;
            }
        }
        public void MemoryMappedSort()
        {
            SortSections();
            Merge();
        }
        void SortSections()
        {
            long currentStart = 0;
            long sectionLength = (long)2 * int.MaxValue;
            using (var mmf = MemoryMappedFile.CreateFromFile(FileName))
            {
                for (int i = 0; i < 8; i++)
                {
                    sectionLength = Math.Min(sectionLength, length - currentStart);
                    using (var ms = mmf.CreateViewAccessor(currentStart, sectionLength, MemoryMappedFileAccess.ReadWrite))
                    {
                        int[] array = new int[sectionLength / 4];
                        ms.ReadArray(0, array, 0, array.Length);
                        Quick3way.sort(array);
                        ms.WriteArray(0, array, 0, array.Length);
                    }
                    currentStart += sectionLength;
                }
            }
        }
        private void Merge()
        {
            using (var a = MemoryMappedFile.CreateFromFile(FileName))
            {
                using (var b = new BinaryWriter(File.Create("B.data", int.MaxValue/1024)))
                {
                    int[][] numbers = new int[8][];
                    long sectionlength = (long)2 * int.MaxValue;
                    long[] startpos = new long[numbers.Length];
                    int[] currentElement = new int[numbers.Length];
                    long length = sectionlength/8;
                    long numElements = length / 4;
                    bool[] getted = new bool[numbers.Length];
                    for (int i = 0; i < 8; i++)
                    {
                        startpos[i] = sectionlength * i;
                        using (var ms = a.CreateViewAccessor(startpos[i], length, MemoryMappedFileAccess.Read))
                        {
                            numbers[i] = new int[numElements];
                            ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
                        }
                    }
                    while (true)
                    {
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            if (getted[i] && currentElement[i] % numElements == 0 && currentElement[i] < numElements * 8)
                            {
                                using (var ms = a.CreateViewAccessor(startpos[i], Math.Min(length, sectionlength * (i + 1) - startpos[i]), MemoryMappedFileAccess.Read))
                                {
                                    numbers[i] = new int[numElements];
                                    ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
                                }
                            }
                            getted[i] = false;
                        }
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            if (currentElement[i] < numElements * 8)
                            {
                                break;
                            }
                            else if (i == 7)
                            {
                                return;
                            }
                        }
                        long min = -1;
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            min = min == -1 && currentElement[i] < numElements * 8 ? i : min;
                            if (currentElement[i] < numElements * 8 && numbers[i][currentElement[i] % numElements] < numbers[min][currentElement[min] % numElements])
                            {
                                min = i;
                            }
                        }
                        b.Write(numbers[min][currentElement[min] % numElements]);
                        currentElement[min]++;
                        startpos[min] += 4;
                        getted[min] = true;
                    }
                }
            }
        }

        private void Merge2()
        {
            using (var a = new BinaryReader(File.OpenRead(FileName)))
            {
                using (var b = new BinaryWriter(File.Create("B.data", int.MaxValue / 2)))
                {
                    int?[] numbers = new int?[8];
                    int sectionlength = int.MaxValue / 2;
                    long[] startpos = new long[numbers.Length];
                    for (int i = 0; i < 8; i++)
                    {
                        startpos[i] = (long)sectionlength * i;
                        a.BaseStream.Position = startpos[i];
                        numbers[i] = a.ReadInt32();
                        startpos[i] += 4;
                    }
                    while (true)
                    {
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            if (numbers[i] == null && startpos[i] < (i + 1) * (long)sectionlength)
                            {
                                a.BaseStream.Position = startpos[i];
                                numbers[i] = a.ReadInt32();
                                startpos[i] += 4;
                            }
                        }
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            if (numbers[i] != null)
                            {
                                break;
                            }
                            else if (i == 7)
                            {
                                return;
                            }
                        }
                        int min = -1;
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            min = min == -1 && numbers[i] != null ? i : min;
                            if (numbers[i] != null && numbers[i] < numbers[min])
                            {
                                min = i;
                            }
                        }
                        b.Write(numbers[min].Value);
                        numbers[min] = null;
                    }
                }
            }
        }

        //private void Merge()
        //{
        //    using (var a = MemoryMappedFile.CreateFromFile(FileName))
        //    {
        //        using (var b = new BinaryWriter(File.Create("B.data", int.MaxValue / 2)))
        //        {
        //            int[][] numbers = new int[8][];
        //            int sectionlength = int.MaxValue / 2;
        //            long[] startpos = new long[numbers.Length];
        //            int[] currentElement = new int[numbers.Length];
        //            int length = sectionlength / 4;
        //            int numElements = length / 4;
        //            bool[] getted = new bool[numbers.Length];
        //            for (int i = 0; i < 8; i++)
        //            {
        //                startpos[i] = (long)sectionlength * i;
        //                using (var ms = a.CreateViewAccessor(startpos[i], length, MemoryMappedFileAccess.Read))
        //                {
        //                    numbers[i] = new int[numElements];
        //                    ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
        //                }
        //            }
        //            while (true)
        //            {
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    if (getted[i] && currentElement[i] % numElements == 0 && currentElement[i] < numElements * 4)
        //                    {
        //                        using (var ms = a.CreateViewAccessor(startpos[i], Math.Min(length, (long)sectionlength * (i + 1) - startpos[i]), MemoryMappedFileAccess.Read))
        //                        {
        //                            numbers[i] = new int[numElements];
        //                            ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
        //                        }
        //                    }
        //                    getted[i] = false;
        //                }
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    if (currentElement[i] < numElements * 4)
        //                    {
        //                        break;
        //                    }
        //                    else if (i == 7)
        //                    {
        //                        return;
        //                    }
        //                }
        //                long min = -1;
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    min = min == -1 && currentElement[i] < numElements * 4 ? i : min;
        //                    if (currentElement[i] < numElements * 4 && numbers[i][currentElement[i] % numElements] < numbers[min][currentElement[min] % numElements])
        //                    {
        //                        min = i;
        //                    }
        //                }
        //                b.Write(numbers[min][currentElement[min] % numElements]);
        //                currentElement[min]++;
        //                startpos[min] += 4;
        //                getted[min] = true;
        //            }
        //        }
        //    }
        //}

        //private void Merge()
        //{
        //    using (var a = MemoryMappedFile.CreateFromFile(FileName))
        //    {
        //        using (var b = new BinaryWriter(File.Create("B.data", 536870912)))
        //        {
        //            int[][] numbers = new int[8][];
        //            int sectionlength = int.MaxValue / 2;
        //            long[] startpos = new long[numbers.Length];
        //            int[] currentElement = new int[8];
        //            int length = int.MaxValue / 16;
        //            int numElements = length / 4;
        //            for (int i = 0; i < 8; i++)
        //            {
        //                startpos[i] = (long)sectionlength * i;
        //                using (var ms = a.CreateViewAccessor(startpos[i], length, MemoryMappedFileAccess.Read))
        //                {
        //                    numbers[i] = new int[numElements];
        //                    ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
        //                }
        //            }
        //            while (true)
        //            {
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    if (currentElement[i] != 0 && currentElement[i] % numElements == 0 && currentElement[i] < numElements*numbers.Length)
        //                    {
        //                        using (var ms = a.CreateViewAccessor(startpos[i], Math.Min(length,(long)sectionlength * (i+1) - startpos[i]), MemoryMappedFileAccess.Read))
        //                        {
        //                            numbers[i] = new int[numElements];
        //                            ms.ReadArray<int>(0, numbers[i], 0, numbers[i].Length);
        //                        }
        //                    }
        //                }
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    if (currentElement[i] < numElements * 8)
        //                    {
        //                        break;
        //                    }
        //                    else if (i == 7)
        //                    {
        //                        return;
        //                    }
        //                }
        //                long min = -1;
        //                for (int i = 0; i < numbers.Length; i++)
        //                {
        //                    min = min == -1 && currentElement[i] < numElements * 8 ? i : min; 
        //                    if (currentElement[i] < numElements * 8 && numbers[i][currentElement[i]%numElements] < numbers[min][currentElement[min] % numElements])
        //                    {
        //                        min = i;
        //                    }
        //                }
        //                b.Write(numbers[min][currentElement[min] % numElements]);
        //                currentElement[min]++;
        //                startpos[min] += 4;

        //            }
        //        }
        //    }
        //}
    }
}
