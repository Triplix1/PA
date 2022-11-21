using Lab_4;

var population = Generator.GeneratePopulation(100,100);

//var (cost, weight) = Generator.GenerateCostWeight(100, 1, 10, 2, 20);

//using (var writer = new BinaryWriter(new FileStream("cost.dat", FileMode.Create)))
//{
//    foreach (var item in cost)
//    {
//        writer.Write(item);
//    }
//}

//using (var writer = new BinaryWriter(new FileStream("weight.dat", FileMode.Create)))
//{
//    foreach (var item in weight)
//    {
//        writer.Write(item);
//    }
//}

List<int> cost = new List<int>();
List<int> weight = new List<int>();

using (var reader = new BinaryReader(new FileStream("cost.dat", FileMode.Open)))
{
    while (reader.BaseStream.Position < reader.BaseStream.Length)
    {
        cost.Add(reader.ReadInt32());
    }
}

using (var reader = new BinaryReader(new FileStream("weight.dat", FileMode.Open)))
{
    while (reader.BaseStream.Position < reader.BaseStream.Length)
    {
        weight.Add(reader.ReadInt32());
    }
}

var genetic = new Genetic(cost.ToArray(), weight.ToArray(), (Member[])population.Clone(), 250, 0.05, 50, 1000);

var (current, Maxcost, Maxweight) = genetic.Run();

//Console.WriteLine(Maxcost.ToString());
//Console.WriteLine(Maxweight.ToString());
//Console.WriteLine("-----------");
//for (int i = 0; i < cost.Count; i+= 2)
//{
//    Console.WriteLine($"{cost[i].ToString(), -3}" + "- " + $"{weight[i].ToString(),-3}" + "  " + $"{cost[i+1].ToString(),-3}" + "- " + $"{weight[i+1].ToString(),-3}");
//}
//Console.WriteLine();