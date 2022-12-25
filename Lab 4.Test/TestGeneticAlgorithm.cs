namespace Lab_4.Test
{
    public class GeneticTest
    {
        (int[], int[], Member[], int, double, int, int) GenerateData()
        {
            int[] cost = { 1, 5, 10, 7 };
            int[] weight = { 1, 4, 7, 9 };
            Member[] members =
            {
                new Member(new bool[] {true, false, false, false}),
                new Member(new bool[] {false, true, false, false}),
                new Member(new bool[] {false, false, true, false}),
                new Member(new bool[] {false, false, false, true})
            };
            int limit = 16;
            double chance = 0.05;
            int point = 2;
            int iterations = 5;
            return (cost, weight, members, limit, chance, point, iterations);
        }

        [Fact]
        public void Genetic_Constructor_NullInputData()
        {
            //Arrange
            int[] cost = null;
            int[] weight = null;
            Member[] members = null;
            int limit = 16;
            double chance = 0.05;
            int point = 2;
            int iterations = 5;

            //Assert
            Assert.Throws<ArgumentNullException>(() => new Genetic(cost, weight, members, limit, chance, point, iterations));
        }

        [Fact]
        public void Genetic_Constructor_NegativeLimit()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, -5, chance, point, iterations));
        }

        [Fact]
        public void Genetic_Constructor_NegativeMutationChance()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, limit, -1, point, iterations));
        }

        [Fact]
        public void Genetic_Constructor_BigMutationChance()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, limit, 1.5, point, iterations));
        }

        [Fact]
        public void Genetic_Constructor_NegativePointChance()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, limit, chance, -1, iterations));
        }

        [Fact]
        public void Genetic_Constructor_PointMoreThanThings()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, limit, chance, 6, iterations));
        }

        [Fact]
        public void Genetic_Constructor_NegativeNumberIterations()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, weight, members, limit, chance, point, -1));
        }

        [Fact]
        public void Genetic_Constructor_DifferentKnapsackCapacity()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => 
            new Genetic(cost, 
            weight, 
            new Member[]
            {
                new Member(new bool[] {true, false, false, }),
                new Member(new bool[] {false, true, false, false}),
                new Member(new bool[] {false, false, true, false}),
                new Member(new bool[] {false, false, false, true})
            }, 
            limit, 
            chance, 
            point, 
            iterations));
        }

        [Fact]
        public void Genetic_Constructor_DifferentCountThings()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(new int[] {1, 2, 3 }, new int[] {1}, members, limit, chance, point, -1));
        }

        [Fact]
        public void WorkWithList_Add_NullParameters()
        {
            //Arrange
            var (cost, weight, members, limit, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentNullException>(() => WorkWithList.Add<Member>(null, null, null));
        }
    }
}