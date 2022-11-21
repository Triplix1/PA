using System;
using System.IO;

namespace Lab_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var workWithFile = new WorkWithFile();

            workWithFile.Generate("bbbb.data", 10485760);

            var sort = new MySort("bbbb.data");

            sort.Sort();

            workWithFile.Output("bbbb.data", 1000);
            Console.WriteLine();
            workWithFile.Output(10485760-1024, "bbbb.data");
        }		
	}

}
