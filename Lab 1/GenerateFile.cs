using System;
using System.IO;

namespace Lab_1
{
    internal  class WorkWithFile
    {
        public void Generate(string path, long size)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Create(path)))
            {
                var random = new Random();                
                for (long i = 0; i < size / 4; i++)
                {
                    bw.Write((int)(random.Next()*1.21462));
                }
            }
        }
        public void Output(string path, int count)
        {
            using (BinaryReader bw = new BinaryReader(File.OpenRead(path)))
            {
                for (int i = 0; i < count; i++)
                {
                    Console.Write($"{bw.ReadInt32()} ");
                }
            }
        }
        public void Output(long satrtpos, string path)
        {
            using (BinaryReader bw = new BinaryReader(File.OpenRead(path)))
            {
                bw.BaseStream.Position = satrtpos;
                for (long i = satrtpos; i < bw.BaseStream.Length; i+=4)
                {
                    Console.Write($"{bw.ReadInt32()} ");
                }
            }
        }
    }
}
