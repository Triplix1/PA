using System.Runtime.Serialization.Formatters.Binary;

namespace Lab_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int numberMembers = 30;
            const int numberCities = 30;
            const int maxCost = 10;

            var population = Generator.GeneratePopulation(numberMembers, numberCities);
            BinaryFormatter formatter = new BinaryFormatter();

            //var cost = Generator.GenerateCost(numberCities, maxCost);            
            //using (var writer = new FileStream("cost.dat", FileMode.Create)) // write cost to file
            //{
            //    formatter.Serialize(writer, cost);
            //}

            int[,] cost;
            using (var reader = new FileStream("cost.dat", FileMode.Open)) //read cost from file
            {
                cost = (int[,])formatter.Deserialize(reader);
            }

            decimal sum = 0;
            for (int i = 0; i < 3; i++)
            {
                Genetic genetic = new Genetic(cost, population, 0.05, 15, 10000);
                (Member result, int c) = genetic.Run();

                for (int j = 0; j < result.Path.Length; j++)
                {
                    Console.Write(result[j].ToString() + " ");
                }
                Console.WriteLine();
                Console.WriteLine(c.ToString());
                Console.WriteLine();
                sum += c;
            }
            Console.WriteLine(sum / 3);       
        }
    }
}