using Lab_4;

var population = Generator.GeneratePopulation(100,100);

var (cost, weight) = Generator.GenerateCostWeight(100, 1, 10, 2, 20);

var genetic = new Genetic(cost.ToArray(), weight.ToArray(), (Member[])population.Clone(), 250, 0.05, 50, 1000);

var (current, Maxcost, Maxweight) = genetic.Run();

Console.WriteLine(Maxcost.ToString());
Console.WriteLine(Maxweight.ToString());