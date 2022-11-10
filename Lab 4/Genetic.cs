using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class WorkWithList
    {
        public static void Add<T>(List<T> arr, T node, Func<T, int> func)//Додавання грального поля в відсортований список
        {
            int start = 0;//Ліва границя
            int end = arr.Count - 1;// Права границя

            while (start <= end)
            {
                int mid = (start + end) / 2;// Середина

                if (func(arr[mid]) == func(node))
                {
                    arr.Insert(mid, node);
                    return;
                }
                else if (func(arr[mid]) < func(node))
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            arr.Insert(end + 1, node);
        }
    }
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

    public class Member
    {
        public bool[] Things;
        public Member(bool[] th)
        {
            Things = th;
        }
        public bool this[int index]
        {
            get { return Things[index]; }
            set { Things[index] = value; }
        }
    }
    public class Genetic
    {
        readonly int[] cost;
        readonly int[] weight;
        readonly int limit;
        readonly double mutationChance;
        readonly int point;
        readonly int iterations;
        List<Member> members = new List<Member>();
        public Genetic(int[] cost, int[] weight, Member[] members, int limit, double mutationChance, int point, int iterations)
        {
            this.cost = cost;
            this.weight = weight;
            foreach (Member member in members)
            {
                WorkWithList.Add(this.members, member, CalculateCost);
            }
            this.limit = limit;
            this.mutationChance = mutationChance;
            this.point = point;
            this.iterations = iterations;
        }
        public (List<Member>, int, int) Run()
        {
            for (int i = 0; i < iterations; i++)
            {
                var (firstParent, secondParent) = ChoseParents();
                var newChild = MergeParents(firstParent, secondParent);
                Mutation(newChild);
                LocalOptimization(newChild);
                if (CalculateWeight(newChild) <= limit && CalculateCost(newChild) >= CalculateCost(members.Last()))
                {
                    ChangeMin(newChild);
                }
            }
            return (members, CalculateCost(members.Last()), CalculateWeight(members.Last()));
        }
        (Member, Member) ChoseParents()
        {
            Random random = new Random();
            var firstParent = members.Last();
            var secondParent = FindSecondParent(); 
            return (firstParent, secondParent);
        }
        Member FindSecondParent()
        {
            Random rand = new Random();
            var randomValue = rand.Next(members.Sum(m => CalculateCost(m)) + 1);
            int sum = 0;
            for (int i = members.Count - 1; i >= 0; i--)
            {
                sum += CalculateCost(members[i]);
                if (sum >= randomValue)
                {
                    return members[i];
                }
            }
            throw new Exception();
        }        
        Member MergeParents(Member first, Member second)
        {
            Random rand = new Random();
            if (rand.NextDouble() < 0.5)
            {
                bool[] tmp = (bool[])first.Things.Clone();
                for (int i = point; i < second.Things.Length; i++)
                {
                    tmp[i] = second[i];
                }
                return new Member(tmp);
            }
            else
            {
                bool[] tmp = (bool[])second.Things.Clone();
                for (int i = point; i < first.Things.Length; i++)
                {
                    tmp[i] = first[i];
                }
                return new Member(tmp);
            }
        }
        void Mutation(Member member)
        {
            Random random = new Random();
            if (random.NextDouble() < mutationChance)
            {
                int randomValue = random.Next(member.Things.Length);
                member[randomValue] = !member[randomValue];
                if (CalculateWeight(member) > limit)
                {
                    member[randomValue] = !member[randomValue];
                }
            }
        }
        void LocalOptimization(Member member)
        {
            if (CalculateWeight(member) < limit)
            {
                int min = int.MaxValue;
                int minindex = int.MaxValue;
                for (int i = 0; i < member.Things.Length; i++)
                {
                    if (min > weight[i] && !member[i])
                    {
                        min = weight[i];
                        minindex = i;
                    }
                }
                if (0 <= minindex && minindex < member.Things.Length)
                {
                    member[minindex] = true;
                }
            }

            if(CalculateWeight(member) > limit)
            {
                int min = int.MaxValue;
                int minindex = int.MaxValue;
                for (int i = 0; i < member.Things.Length; i++)
                {
                    if (min > weight[i] && member[i])
                    {
                        min = weight[i];
                        minindex = i;
                    }
                }
                if (0 <= minindex && minindex < member.Things.Length)
                {
                    member[minindex] = false;
                }
            }
        }
        void ChangeMin(Member member)
        {
            members.RemoveAt(0);
            WorkWithList.Add(members, member, CalculateCost);
        }
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
    }

    //public class Member
    //{
    //    public bool[] Things;
    //    public Member(bool[] th)
    //    {
    //        Things = th;
    //    }
    //    public bool this[int index]
    //    {
    //        get { return Things[index]; }
    //        set { Things[index] = value; }
    //    }
    //}
    //public class Genetic
    //{
    //    readonly int[,] costWeight;
    //    readonly int limit;
    //    readonly double mutationChance;
    //    readonly int point;
    //    readonly int iterations;
    //    List<Member> members = new List<Member>();
    //    public Genetic(int[,] costWeight, Member[] members, int limit, double mutationChance, int point, int iterations)
    //    {
    //        this.costWeight = costWeight;
    //        foreach (Member member in members)
    //        {
    //            WorkWithList.Add(this.members, member, CalculateCost);
    //        }
    //        this.limit = limit;
    //        this.mutationChance = mutationChance;
    //        this.point = point;
    //        this.iterations = iterations;
    //    }
    //    public (List<Member>, int, int) Run()
    //    {
    //        for (int i = 0; i < iterations; i++)
    //        {
    //            var (firstParent, secondParent) = ChoseParents();
    //            var newChild = MergeParents(firstParent, secondParent);
    //            Mutation(newChild);
    //            if (CalculateWeight(newChild) <= limit)
    //            {
    //                ChangeMin(newChild);
    //            }
    //        }
    //        return (members, CalculateCost(members.Last()), CalculateWeight(members.Last()));
    //    }
    //    (Member, Member) ChoseParents()
    //    {
    //        Random random = new Random();
    //        var firstParent = members.Last();
    //        var secondParent = FindSecondParent(); //members[random.Next(members.Count)];
    //        return (firstParent, secondParent);
    //    }
    //    Member FindSecondParent()
    //    {
    //        Random rand = new Random();
    //        var randomValue = rand.Next(members.Sum(m => CalculateCost(m)) + 1);
    //        int sum = 0;
    //        for (int i = members.Count - 1; i >= 0; i--)
    //        {
    //            sum += CalculateCost(members[i]);
    //            if (sum >= randomValue)
    //            {
    //                return members[i];
    //            }
    //        }
    //        throw new Exception();
    //    }
    //    Member MergeParents(Member first, Member second)
    //    {
    //        Random rand = new Random();
    //        if (rand.NextDouble() < 0.5)
    //        {
    //            bool[] tmp = (bool[])first.Things.Clone();
    //            for (int i = point; i < second.Things.Length; i++)
    //            {
    //                tmp[i] = second[i];
    //            }
    //            return new Member(tmp);
    //        }
    //        else
    //        {
    //            bool[] tmp = (bool[])second.Things.Clone();
    //            for (int i = point; i < first.Things.Length; i++)
    //            {
    //                tmp[i] = first[i];
    //            }
    //            return new Member(tmp);
    //        }
    //    }
    //    void Mutation(Member member)
    //    {
    //        Random random = new Random();
    //        if (random.NextDouble() < mutationChance)
    //        {
    //            int randomValue = random.Next(member.Things.Length);
    //            member[randomValue] = !member[randomValue];
    //            if (CalculateWeight(member) > limit)
    //            {
    //                member[randomValue] = !member[randomValue];
    //            }
    //        }
    //    }
    //    void ChangeMin(Member member)
    //    {
    //        members.RemoveAt(0);
    //        WorkWithList.Add(members, member, CalculateCost);
    //    }
    //    int CalculateCost(Member member)
    //    {
    //        int result = 0;
    //        for (int i = 0; i < member.Things.Length; i++)
    //        {
    //            if (member[i])
    //            {
    //                result += costWeight[i, 0];
    //            }
    //        }
    //        return result;
    //    }
    //    int CalculateWeight(Member member)
    //    {
    //        int result = 0;
    //        for (int i = 0; i < member.Things.Length; i++)
    //        {
    //            if (member[i])
    //            {
    //                result += costWeight[i, 1];
    //            }
    //        }
    //        return result;
    //    }
    //}
}
