using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class Generator
    {
        public static Member[] GeneratePopulation(int num, int countElemnt)
        {
            Member[] members = new Member[num];
            for (int i = 0; i < num; i++)
            {
                members[i] = new Member(new bool[countElemnt]);
                members[i].Things[i % countElemnt] = true;
            }
            return members;
        }
        public static (int[] cost, int[] weight) GenerateCostWeight(int num, int minWeight, int maxWeight, int minCost, int maxCost)
        {
            int[] cost = new int[num];
            int[] weight = new int[num];
            Random rand = new Random();
            for (int i = 0; i < num; i++)
            {
                cost[i] = rand.Next(minCost, maxCost + 1);
                weight[i] = rand.Next(minWeight, maxWeight + 1);
            }
            return (cost, weight);
        }
    }
}
