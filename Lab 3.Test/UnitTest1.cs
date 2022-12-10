using Lab3.Models;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using Lab3.Controllers;
using System.Linq;

namespace Lab3.Test
{
    public class UnitTest1
    {
        List<Row> GetRowsForMock()
        {
            return new List<Row>
            {
                new Row { RowId = 0, Value = "0" },
                new Row { RowId = 1, Value = "1" },
                new Row { RowId = 2, Value = "2" }
            };
        }

        [Fact]
        public void ToListTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();
            var tree = new AVL(mockRepos.Object);

            //Act
            var result = tree.ToList();

            //Assert
            Assert.True(expected.SequenceEqual(result));
        }

        [Fact]
        public void AddPositiveToAVlTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = new Row { RowId = 3, Value = "3" };

            //Act
            tree.Add(new Node { Row = new Row { RowId = 3, Value = "3" } });

            //Assert
            Assert.Equal(expected, tree.ToList().Last());
        }

        [Fact]
        public void AddNegativeToAVlTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = new Row { RowId = -1, Value = "-1" };
            var node = new Node { Row = new Row { RowId = -1, Value = "-1" } };

            //Act
            tree.Add(node);

            //Assert
            Assert.Equal(expected, tree.ToList().First());
        }

        [Fact]
        public void AddNullToAVlTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = tree.ToList().Count;

            //Act
            tree.Add(null);

            //Assert
            Assert.Equal(expected, tree.ToList().Count);
        }

        [Fact]
        public void DeleteNonContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            int valueToDelete = -1;
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();

            //Act
            tree.Delete(valueToDelete);
            var resultList = tree.ToList();

            //Assert
            Assert.True(expected.SequenceEqual(resultList));
        }

        [Fact]
        public void DeleteContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            int valueToDelete = 1;
            var expected = GetRowsForMock().Where(x => x.RowId != 1).OrderBy(x => x.RowId).ToList();

            //Act
            tree.Delete(valueToDelete);
            var resultList = tree.ToList();

            //Assert
            Assert.True(expected.SequenceEqual(resultList));
        }

        [Fact]
        public void FindNonContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var valueToFind = -1;
            var compnum = 0;

            //Act
            var result = tree.Find(valueToFind, ref compnum);

            //Asssert
            Assert.Null(result);
        }

        [Fact]
        public void FindContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var valueToFind = 2;
            var compnum = 0;
            var expected = new Node { Row = new Row { RowId = 2, Value = "2" } };

            //Act
            var result = tree.Find(valueToFind, ref compnum);

            //Asssert
            Assert.Equal(expected.Row, result.Row);
        }

        [Fact]
        public void EditNullTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();

            //Act
            tree.Edit(null);
            var resultList = tree.ToList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }

        [Fact]
        public void EditContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());

            var valueToEdtit = new Row { RowId = 1, Value = "11" };
            var tree = new AVL(mockRepos.Object);
            var expected = new List<Row>
            {
                new Row { RowId = 0, Value = "0" },
                new Row { RowId = 1, Value = "11" },
                new Row { RowId = 2, Value = "2" }
            }; ;

            //Act
            tree.Edit(valueToEdtit);
            var resultList = tree.ToList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }

        [Fact] 
        public void EditNonContainedTest()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());

            var valueToEdtit = new Row { RowId = -1, Value = "11" };
            var tree = new AVL(mockRepos.Object);
            var expected = new List<Row>
            {
                new Row { RowId = 0, Value = "0" },
                new Row { RowId = 1, Value = "1" },
                new Row { RowId = 2, Value = "2" }
            }; ;

            //Act
            tree.Edit(valueToEdtit);
            var resultList = tree.ToList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }
    }
}
