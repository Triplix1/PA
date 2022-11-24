using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Lab_5
{

    public class Member
    {
        public int[] Path;
        public Member(int[] th)
        {
            Path = th;
        }
        public int this[int index]
        {
            get { return Path[index]; }
            set { Path[index] = value; }
        }
    }

    public class Genetic
    {

        int[,] cost;
        List<Member> members = new List<Member>();
        readonly double mutationChance;
        readonly int point;
        readonly int iterations;

        public Genetic(int[,] cost, Member[] members, double mutationChance, int point, int iterations)
        {
            if (members == null || cost == null)
            {
                throw new ArgumentNullException("cost, members should not be null");
            }
            else
            {
                if (members.Length < 3)
                {
                    throw new ArgumentException("Members count can`t be less than 2");
                }
                if (cost.Length != members[0].Path.Length + 1)
                {
                    throw new ArgumentException("number element in count, weight and members should be equal");
                }
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
            if (cost.GetLength(0) != cost.GetLength(1))
            {
                throw new ArgumentException("Cost shuould be equal in demension length");
            }
            this.cost = cost;            
            this.mutationChance = mutationChance;
            this.point = point;
            this.iterations = iterations;
            foreach (Member member in members)
            {
                if (member.Path.Length != members.Last().Path.Length)
                {
                    throw new ArgumentException("The number of things capacity should be equal in each knapsack");
                }
                WorkWithList.Add(this.members, member, CalculatePath);
            }
        }

        public (Member, int) Run()
        {
            for (int i = 0; i < iterations; i++)
            {
                var (firstParent, secondParent) = TournamentSelection();
                var newChild = PartiallyMappedCrossower(firstParent, secondParent);
                MutationReverse(newChild);
                LocalOptimizationFromFour(newChild);
                if (CalculatePath(newChild) <= CalculatePath(members.Last()))
                {
                    ChangeMin(newChild);
                }
            }
            return (members[0], CalculatePath(members[0]));
        }

        (Member, Member) ChoseParentsProportional()
        {
            Random random = new Random();
            var firstParent = members[0];
            var secondParent = FindSecondParent();
            return (firstParent, secondParent);
        }        

        Member FindSecondParent()
        {
            Random rand = new Random();
            var randomValue = rand.NextDouble() * members.Sum(m => (double)1 / CalculatePath(m));
            double sum = 0;
            for (int i = members.Count - 1; i >= 0; i--)
            {
                sum += 1.0/CalculatePath(members[i]);
                if (sum >= randomValue)
                {
                    return members[i];
                }
            }
            throw new Exception();
        }

        (Member, Member) TournamentSelection()
        {
            List<int> group = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < members.Count; i++)
            {
                int randomFirst = rand.Next(members.Count - 2) + 1;
                int randomSecond = rand.Next(members.Count - 2) + 1;
                while (randomSecond == randomFirst)
                {
                    randomSecond = rand.Next(members.Count - 2) + 1;
                }
                if (!group.Contains(Math.Min(randomFirst, randomSecond)))
                {
                    group.Add(Math.Min(randomFirst, randomSecond));
                }                
            }
            int first = rand.Next(group.Count);
            int second = rand.Next(group.Count);
            while (second == first)
            {
                second = rand.Next(group.Count);
            }
            return (members[group[first]], members[group[second]]);
        }

        Member MergeParents(Member first, Member second)
        {
            List<int> firstChild = ((int[])first.Path.Clone()).Take(point).ToList();
            var currentPoints = second.Path.Where((num, index) => index >= point && !firstChild.Contains(num))
                   .Union(first.Path.Where((x, p) => p >= point && !firstChild.Contains(x)));
            for (int i = point; i < first.Path.Length - 1; i++)
            { 
               firstChild.Add(currentPoints.First());
                currentPoints = currentPoints.Where(x => x != firstChild.Last());
            }

            List<int> secondChild = ((int[])second.Path.Clone()).Take(point).ToList();
            currentPoints = first.Path.Where((x, p) => p >= point && !secondChild.Contains(x))
                   .Union(second.Path.Where((x, p) => p >= point && !secondChild.Contains(x)));
            for (int i = point; i < first.Path.Length - 1; i++)
            {
                secondChild.Add(currentPoints.First());
                currentPoints = currentPoints.Where(x => x != secondChild.Last());
            }
            firstChild.Add(0);
            secondChild.Add(0);
            Member fresult = new Member(firstChild.ToArray());
            Member sresult = new Member(secondChild.ToArray());
            return CalculatePath(fresult) > CalculatePath(sresult) ? sresult : fresult;
        }

        Member OrderCrossower(Member first, Member second)
        {
            int fourth = (first.Path.Length - 2) / 4;
            int[] firstChild = Enumerable.Repeat(-1, first.Path.Length).ToArray();
            for (int i = fourth; i < firstChild.Length - 1 - fourth; i++)
            {
                firstChild[i] = first[i];
            }
            for (int i = 0; i < firstChild.Length - 1; i++)
            {
                if (firstChild[i] == -1)
                {
                    firstChild[i] = second.Path.Where(x => !firstChild.Contains(x)).First();
                }
            }
            firstChild[firstChild.Length - 1] = firstChild[0];

            int[] secondChild = Enumerable.Repeat(-1, second.Path.Length).ToArray();
            for (int i = fourth; i < second.Path.Length - 1 - fourth; i++)
            {
                secondChild[i] = second[i];
            }
            for (int i = 0; i < secondChild.Length - 1; i++)
            {
                if (secondChild[i] == -1)
                {
                    secondChild[i] = first.Path.Where(x => !secondChild.Contains(x)).First();
                }
            }
            secondChild[secondChild.Length - 1] = secondChild[0];

            return CalculatePath(firstChild) < CalculatePath(secondChild) ? new Member(firstChild) : new Member(secondChild);
        }

        Member PartiallyMappedCrossower(Member first, Member second)
        {
            int fourth = (first.Path.Length - 2) / 4;
            int[] firstChild = Enumerable.Repeat(-1, first.Path.Length).ToArray();
            for (int i = fourth; i < first.Path.Length - 1 - fourth; i++)
            {
                firstChild[i] = first[i];
            }
            for (int i = 0; i < firstChild.Length-1; i++)
            {
                if (!firstChild.Contains(second[i]) && (i < fourth || i >= first.Path.Length - 1 - fourth))
                {
                    firstChild[i] = second[i];
                }
            }
            for (int i = 0; i < firstChild.Length-1; i++)
            {
                if (firstChild[i] == -1)
                {
                    firstChild[i] = second.Path.Where(x => !firstChild.Contains(x)).First();
                }
            }
            firstChild[firstChild.Length - 1] = firstChild[0];

            int[] secondChild = Enumerable.Repeat(-1, second.Path.Length).ToArray();
            for (int i = fourth; i < second.Path.Length - 1 - fourth; i++)
            {
                secondChild[i] = second[i];
            }
            for (int i = 0; i < secondChild.Length - 1; i++)
            {
                if (!secondChild.Contains(first[i]) && (i < fourth || i >= first.Path.Length - 1 - fourth))
                {
                    secondChild[i] = second[i];
                }
            }
            for (int i = 0; i < secondChild.Length - 1; i++)
            {
                if (secondChild[i] == -1)
                {
                    secondChild[i] = first.Path.Where(x => !secondChild.Contains(x)).First();
                }
            }
            secondChild[secondChild.Length - 1] = secondChild[0];

            return CalculatePath(firstChild) < CalculatePath(secondChild) ? new Member(firstChild) : new Member(secondChild);
        }

        void Mutation(Member member)
        {
            Random random = new Random();
            if (random.NextDouble() < mutationChance)
            {
                var first = random.Next(member.Path.Length - 2) + 1;
                var second = random.Next(member.Path.Length - 2) + 1;
                while (second == first)
                {
                    second = random.Next(member.Path.Length - 2) + 1;
                }
                member[first] = member[first] + member[second];
                member[second] = member[first] - member[second];
                member[first] = member[first] - member[second];
            }
        }

        void MutationReverse(Member member)
        {
            Random rand = new Random();
            var first = rand.Next(member.Path.Length - 3) + 1;
            var second = rand.Next(member.Path.Length - 2) + 1;
            while (first >= second)
            {
                second = rand.Next(member.Path.Length - 2) + 1;
            }
            for (int i = 0; i <= (second - first) / 2 + 1; i++)
            {
                var tmp = member[first + i];
                member[first + i] = member[second - i];
                member[second - i] = tmp;
            }
        }

        void LocalOptimizationFromFour(Member member)
        {
            for (int i = 0; i < member.Path.Length - 3; i++)
            {
                var tmp = member.Path.Skip(i).Take(4).ToArray();
                tmp[1] = tmp[1] + tmp[2];
                tmp[2] = tmp[1] - tmp[2];
                tmp[1] = tmp[1] - tmp[2];
                if (CalculatePath(tmp) < CalculatePath(member.Path.Skip(i).Take(4).ToArray()))
                {
                    member[i + 1] = member[i + 1] + member[i + 2];
                    member[i + 2] = member[i + 1] - member[i + 2];
                    member[i + 1] = member[i + 1] - member[i + 2];
                }
            }
        }

        void LocalOptimizationReversing(Member member)
        {
            for (int j = 0; j < mutationChance*100; j++)
            {
                Random rand = new Random();                
                var first = rand.Next(member.Path.Length - 3) + 1;
                var second = rand.Next(member.Path.Length - 2) + 1;
                while (first >= second)
                {
                    second = rand.Next(member.Path.Length - 2) + 1;
                }
                int costBefore = CalculatePath(member.Path.Skip(first - 1).Take(second - first + 1).ToArray());
                int costAfter = CalculatePath(member.Path.Skip(first - 1).Take(second - first + 1).Reverse().ToArray());
                if (costBefore > costAfter)
                {
                    for (int i = 0; i <= (second - first) / 2 + 1; i++)
                    {
                        var tmp = member[first + i];
                        member[first + i] = member[second - i];
                        member[second - i] = tmp;
                    }
                }                        
            }                         
        }

        void ChangeMin(Member member)
        {
            members.RemoveAt(members.Count - 1);
            WorkWithList.Add(members, member, CalculatePath);
        }

        int CalculatePath(Member member)
        {
            int result = 0;
            for (int i = 1; i < member.Path.Length - 1; i++)
            {
                result += cost[member[i - 1], member[i]];
            }
            result += cost[member.Path.Last(), member[0]];
            return result;
        }

        int CalculatePath(int[] member)
        {
            int result = 0;
            for (int i = 1; i < member.Length - 1; i++)
            {
                result += cost[member[i - 1], member[i]];
            }
            result += cost[member.Last(), member[0]];
            return result;
        }
    }
}
