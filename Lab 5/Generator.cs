using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_5
{
    public static class Generator
    {
        public static int[,] GenerateCost(int count, int maxCost)
        {
            int[,] costs = new int[count, count];
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        costs[i, j] = 0;
                    }
                    else
                    {
                        costs[i, j] = random.Next(maxCost) + 1;
                    }
                }
            }
            return costs;
        }
        public static Member[] GeneratePopulation(int count, int numberCities)
        {
            Member[] members = new Member[count];
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                members[i] = new Member(new int[numberCities + 1]);
                for (int j = 0; j < numberCities-1; j++)
                {                    
                    members[i].Path[j + 1] = (i + j) % (numberCities-1) + 1;
                }                
            }
            return members;
        }
    }
}
