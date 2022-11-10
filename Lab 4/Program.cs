using Lab_4;

var population = Generator.GeneratePopulation(100,100);

//var (cost, weight) = Generator.GenerateCostWeight(100, 1, 10, 2, 20);

//using (var writer = new BinaryWriter(new FileStream("cost.dat", FileMode.OpenOrCreate)))
//{
//    foreach (var item in cost)
//    {
//        writer.Write(item);
//    }
//}

//using (var writer = new BinaryWriter(new FileStream("weight.dat", FileMode.OpenOrCreate)))
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

for (int i = 20; i <= 1000; i += 20)
{
    var genetic = new Genetic(cost.ToArray(), weight.ToArray(), (Member[])population.Clone(), 250, 0.05, 50, i);

    var (current, c, w) = genetic.Run();

    Console.WriteLine(c.ToString());
    Console.WriteLine(w.ToString());
    Console.WriteLine("-----------");
}

//var genetic = new Genetic(cost.ToArray(), weight.ToArray(), (Member[])population.Clone(), 250, 0.05, 50, 10000);

//var (current, Maxcost, Maxweight) = genetic.Run();

//Console.WriteLine(cost.ToString());
//Console.WriteLine(weight.ToString());
//Console.WriteLine("-----------");
//foreach (var item in current)
//{
//    Console.WriteLine(CalculateCost(item));
//    Console.WriteLine(CalculateWeight(item));
//    Console.WriteLine();
//}

int CalculateCost(Member member)
{
    int result = 0;
    for (int i = 0; i < member.Things.Length; i++)
    {
        if (member[i])
        {
            result += cost[i];
        }
    }
    return result;
}
int CalculateWeight(Member member)
{
    int result = 0;
    for (int i = 0; i < member.Things.Length; i++)
    {
        if (member[i])
        {
            result += weight[i];
        }
    }
    return result;
}