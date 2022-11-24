namespace Lab_5.Test
{
    public class UnitTest1
    {
        (int[,], Member[], double, int, int) GenerateData()
        {
            int[,] cost = { { 0, 5, 10, 7 }, { 6, 0, 3, 1 }, { 1, 7, 0, 3 }, { 9, 3, 8, 0 } };
            Member[] members =
            {
                new Member(new int[] {0, 1, 2, 3, 0 }),
                new Member(new int[] {0, 3, 1, 2, 0}),
                new Member(new int[] {0, 2, 3, 1, 0})
            };
            double chance = 0.05;
            int point = 2;
            int iterations = 5;
            return (cost, members, chance, point, iterations);
        }

        [Fact]
        public void ConstructorNullTest()
        {
            //Arrange
            int[,] cost = null;
            Member[] members = null;
            double chance = 0.05;
            int point = 2;
            int iterations = 5;

            //Assert
            Assert.Throws<ArgumentNullException>(() => new Genetic(cost, members, chance, point, iterations));
        }

        [Fact]
        public void NegativeMutationChanceTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, -1, point, iterations));
        }

        [Fact]
        public void BigMutationChanceTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, 1.5, point, iterations));
        }

        [Fact]
        public void NegativePointChanceTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, chance, -1, iterations));
        }

        [Fact]
        public void PointMoreThanThingsTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, chance, 6, iterations));
        }

        [Fact]
        public void NegativeNumberIterationsTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, chance, point, -1));
        }

        [Fact]
        public void DifferentNumberOfCitiesInPopulationTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            new Genetic(cost,
            new Member[]
            {
                new Member(new int[] {0,1,0}),
                new Member(new int[] {0, 1, 2, 0}),
                new Member(new int[] {0, 1, 2, 3, 0})
            },
            chance,
            point,
            iterations));
        }

        [Fact]
        public void GeneratorNullParametersTest()
        {
            //Arrange
            var (cost, members, chance, point, iterations) = GenerateData();

            //Assert
            Assert.Throws<ArgumentNullException>(() => WorkWithList.Add<Member>(null, null, null));
        }
        [Fact]
        public void DifferentCostDimensionLengthTest()
        {
            //Arrange
            var (tmp, members, chance, point, iterations) = GenerateData();
            int[,] cost = { { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 } };

            //Assert
            Assert.Throws<ArgumentException>(() => new Genetic(cost, members, chance, point, iterations));
        }
    }
}