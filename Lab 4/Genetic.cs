using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
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
            if (members == null|| cost == null|| weight == null)
            {
                throw new ArgumentNullException("cost, weight, members should not be null");
            }
            else
            {
                if (members.Length < 2)
                {
                    throw new ArgumentException("Members count can`t be less than 2");
                }
                if (cost.Length != members[0].Things.Length || cost.Length != weight.Length)
                {
                    throw new ArgumentException("number element in count, weight and members should be equal");
                }
            }
            if (limit < 1)
            {
                throw new ArgumentException("Limit can`t be less than 1");
            }
            if (mutationChance < 0 || mutationChance > 1)
            {
                throw new ArgumentException("mutation chance should be from 0 to 1");
            }
            if (point < 0 || point >= cost.Length)
            {
                throw new ArgumentException("point should be from 0 to number things in one knapsack");
            }
            if (iterations < 0)
            {
                throw new ArgumentException("iterations should be from 0 to infinity!");
            }
            this.cost = cost;
            this.weight = weight;                        
            foreach (Member member in members)
            {
                if (member.Things.Length != members.Last().Things.Length)
                {
                    throw new ArgumentException("The number of things capacity should be equal in each knapsack");
                }
                WorkWithList.Add(this.members, member, CalculateCost);
            }
            this.limit = limit;
            this.mutationChance = mutationChance;
            this.point = point;
            this.iterations = iterations;
        }
        public (List<Member>, int, int) Run()
        {
            for (int i = 1; i <= iterations; i++)
            {
                var (firstParent, secondParent) = ChoseParents();
                var newChild = MergeParents(firstParent, secondParent);
                Mutation(newChild);
                LocalOptimization(newChild);
                if (CalculateWeight(newChild) <= limit && CalculateCost(newChild) >= CalculateCost(members[0]))
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
            
                Member firstChild = new Member((bool[])first.Things.Clone());
                for (int i = point; i < second.Things.Length; i++)
                {
                firstChild[i] = second[i];
                }
                     
                Member secondChild = new Member((bool[])second.Things.Clone());
                for (int i = point; i < first.Things.Length; i++)
                {
                    secondChild[i] = first[i];
                }
                return CalculateCost(firstChild) > CalculateCost(secondChild) ? firstChild : secondChild;
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
}
