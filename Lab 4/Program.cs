using Lab_4;

int populationNum = 100;
int elementsNum = 100;
int minWeight = 1;
int maxWeight = 10;
int minCost = 2;
int maxCost = 20;
int limit = 250;
double mutationChance = 0.05;
int point = 50;
int iterations = 1000;

var population = Generator.GeneratePopulation(populationNum,elementsNum);

var (cost, weight) = Generator.GenerateCostWeight(elementsNum, minWeight, maxWeight, minCost, maxCost);

var genetic = new Genetic(cost.ToArray(), weight.ToArray(), (Member[])population.Clone(), limit, mutationChance, point, iterations);

var (current, Maxcost, Maxweight) = genetic.Run();

Console.WriteLine("Generated Cost-Weight:\n");

PrintCostAndWeight(cost, weight);

Console.WriteLine("\n-----------\n");

Console.WriteLine("Resulted Cnapsack:");
Console.WriteLine("Cost: " + Maxcost.ToString());
Console.WriteLine("Weight: " + Maxweight.ToString());

void PrintCostAndWeight(int[] cost,int[] weight)
{
    for (int i = 0; i < cost.Length; i += 2)
    {
        Console.WriteLine($"{cost[i].ToString(),-3}" + "- " + $"{weight[i].ToString(),-3}" + "  " + $"{cost[i + 1].ToString(),-3}" + "- " + $"{weight[i + 1].ToString(),-3}");
    }
}