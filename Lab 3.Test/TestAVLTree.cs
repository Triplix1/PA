using Lab3.Models;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using Lab3.Controllers;
using System.Linq;

namespace Lab3.Test
{
    public class TestAVLTree
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
        public void AVL_GetRowsList()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();
            var tree = new AVL(mockRepos.Object);

            //Act
            var result = tree.GetRowsList();

            //Assert
            Assert.True(expected.SequenceEqual(result));
        }

        [Fact]
        public void AVL_Add_AddPositiveNumber()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = new Row { RowId = 3, Value = "3" };

            //Act
            tree.Add(new Node { Row = new Row { RowId = 3, Value = "3" } });

            //Assert
            Assert.Equal(expected, tree.GetRowsList().Last());
        }

        [Fact]
        public void AVL_Add_AddNegativeNumber()
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
            Assert.Equal(expected, tree.GetRowsList().First());
        }

        [Fact]
        public void AVL_Add_AddNull()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = tree.GetRowsList().Count;

            //Act
            tree.Add(null);

            //Assert
            Assert.Equal(expected, tree.GetRowsList().Count);
        }

        [Fact]
        public void AVL_Delete_DeleteNonContainedNode()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            int valueToDelete = -1;
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();

            //Act
            tree.Delete(valueToDelete);
            var resultList = tree.GetRowsList();

            //Assert
            Assert.True(expected.SequenceEqual(resultList));
        }

        [Fact]
        public void AVL_Delete_DeleteContainedNode()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            int valueToDelete = 1;
            var expected = GetRowsForMock().Where(x => x.RowId != 1).OrderBy(x => x.RowId).ToList();

            //Act
            tree.Delete(valueToDelete);
            var resultList = tree.GetRowsList();

            //Assert
            Assert.True(expected.SequenceEqual(resultList));
        }

        [Fact]
        public void AVL_Find_FindNonContainedNode()
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
        public void AVL_Find_FindContainedNode()
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
        public void AVL_Edit_EditNull()
        {
            //Arrange
            var mockRepos = new Mock<INodeRepository>();
            mockRepos.Setup(repo => repo.Nodes).Returns(GetRowsForMock());
            var tree = new AVL(mockRepos.Object);
            var expected = GetRowsForMock().OrderBy(x => x.RowId).ToList();

            //Act
            tree.Edit(null);
            var resultList = tree.GetRowsList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }

        [Fact]
        public void AVL_Edit_EditContainedNode()
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
            var resultList = tree.GetRowsList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }

        [Fact] 
        public void AVL_Edit_EditNonContainedNode()
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
            var resultList = tree.GetRowsList();

            //Asssert
            Assert.True(resultList.SequenceEqual(expected));
        }
    }
}
