using System;
using System.IO;

namespace Lab_1
{
    internal class MySort
    {
        public string FileName { get; set; }
        private long iterations;
        private long length;
        private long segments = 0;
        public MySort(string path)
        {
            FileName = path;
            iterations = 1;
            using (BinaryReader br = new BinaryReader(File.OpenRead(FileName)))
            {
                length = br.BaseStream.Length;
            }
        }
        public void SplitFile()
        {
            segments = 1;
            int element;
            using (BinaryReader br = new BinaryReader(File.OpenRead(FileName)))
            {
                using (BinaryWriter bw1 = new BinaryWriter(File.Create("b1.dat")))
                {
                    using (BinaryWriter bw2 = new BinaryWriter(File.Create("c1.dat")))
                    {
                        bool flag = true;
                        long counter = 0;
                        long length = br.BaseStream.Length;
                        long position = 0;
                        while (position != length)
                        {
                            if (counter == iterations)
                            {
                                flag = !flag;
                                counter = 0;
                                segments++;
                            }
                            element = br.ReadInt32();
                            position += 4;
                            if (flag)
                            {
                                bw1.Write(element);
                            }
                            else
                            {
                                bw2.Write(element);
                            }
                            counter++;
                        }
                    }
                }
            }
        }
        private void Merge()
        {
            using (BinaryWriter a = new BinaryWriter(File.Create(FileName)))
            {
                using (BinaryReader b = new BinaryReader(File.OpenRead("b1.dat")))
                {
                    using (BinaryReader c = new BinaryReader(File.OpenRead("c1.dat")))
                    {
                        long lengthb = b.BaseStream.Length;
                        long lengthc = c.BaseStream.Length;
                        bool endB = false, endC = false;
                        int? numberB = null;
                        int? numberC = null;
                        long iterB = iterations;
                        long iterC = iterations;
                        while (!endC || !endB)
                        {
                            if (iterB == 0 && iterC == 0)
                            {
                                iterB = iterations;
                                iterC = iterations;
                            }
                            if (b.BaseStream.Position != lengthb)
                            {
                                if (iterB > 0 && numberB == null)
                                {
                                    numberB = b.ReadInt32();
                                }                                
                            }
                            else
                            {
                                endB = true;
                            }

                            if (c.BaseStream.Position != lengthc)
                            {
                                if (iterC > 0 &&numberC == null)
                                {
                                    numberC = c.ReadInt32();
                                }                                
                            }
                            else
                            {
                                endC = true;
                            }

                            if (numberB != null)
                            {
                                if (numberC != null)
                                {
                                    if (numberB < numberC)
                                    {
                                        a.Write(numberB.Value);
                                        numberB = null;
                                        iterB--;
                                    }
                                    else
                                    {
                                        a.Write(numberC.Value);
                                        numberC = null;
                                        iterC--;
                                    }                                
                                }
                                else
                                {
                                    a.Write(numberB.Value);
                                    numberB = null;
                                    iterB--;
                                }
                            }
                            else if (numberC != null)
                            {
                                a.Write(numberC.Value);
                                numberC = null;
                                iterC--;
                            }
                        }
                        iterations *= 2;
                    }
                }
            }
        }
        public void Sort()
        {
            while (true)
            {
                SplitFile();
                if (segments == 1)
                {
                    break;
                }
                Merge();
            }
        }
    }
}
