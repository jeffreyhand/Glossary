using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Glossary;
using Glossary.Controllers;
using Moq;
using System.Data.Entity;
using Glossary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glossary.Tests.Controllers
{
    [TestClass]
    public class EntryControllerTest
    {

        [TestMethod]
        public void Index_NullSort_ReturnsIndexViewWithChangedSort()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string expectedViewName = "";
            string expectedViewBagNameSortParm = Sort.SORT_DESCENDING;

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedViewBagNameSortParm, result.ViewData.First().Value);
        }


        [TestMethod]
        public void Index_InvalidSortParam_ReturnsIndexViewWithChangedSort()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string expectedViewName = "";
            string expectedViewBagNameSortParm = Sort.SORT_DESCENDING;
            string inValidSortByParam = "zzz";

            // Act
            ViewResult result = controller.Index(inValidSortByParam) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedViewBagNameSortParm, result.ViewData.First().Value);
        }


        [TestMethod]
        public void Index_DescendingSortParam_ReturnsIndexViewWithChangedSort()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string expectedViewName = "";
            string validSortByParam = Sort.SORT_DESCENDING;
            string expectedViewBagNameSortParm = Sort.SORT_ASCENDING;

            // Act
            ViewResult result = controller.Index(validSortByParam) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedViewBagNameSortParm, result.ViewData.First().Value);
        }


        [TestMethod]
        public void Index_AscendingSortParam_ReturnsAlphabeticalEntries()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string sortByParam = Sort.SORT_ASCENDING;

            List<Entry> sortedDataSource = dataSource.ToList();
            sortedDataSource.Sort((a, b) => (a.Term.CompareTo(b.Term)));

            Entry expectedFirstAlphabeticalEntry = sortedDataSource.First();

            // Act
            ViewResult result = controller.Index(sortByParam) as ViewResult;
            List<Entry> actualModel = result.Model as List<Entry>;

            // Assert
            Assert.AreEqual(expectedFirstAlphabeticalEntry.Id, actualModel.First().Id);
        }


        [TestMethod]
        public void Index_DescendingSortParam_ReturnsReverseAlphabeticalEntries()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string sortByParam = Sort.SORT_DESCENDING;

            List<Entry> sortedDataSource = dataSource.ToList();
            sortedDataSource.Sort((a, b) => (a.Term.CompareTo(b.Term)));

            Entry expectedFirstAlphabeticalEntry = sortedDataSource.Last();

            // Act
            ViewResult result = controller.Index(sortByParam) as ViewResult;
            List<Entry> actualModel = result.Model as List<Entry>;

            // Assert
            Assert.AreEqual(expectedFirstAlphabeticalEntry.Id, actualModel.First().Id);
        }


        [TestMethod]
        public void New_Called_ReturnsNewFormViewAndEntry()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string expectedViewName = "EntryForm";
            int expectedUnknownNewEntryID = Entry.UNASSIGNED_ID;

            // Act
            ViewResult result = controller.New() as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedUnknownNewEntryID, ((Entry)result.Model).Id);
        }


        [TestMethod]
        public void Edit_ValidEntry_ReturnsEditFormViewAndCorrespondingEntry()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            string expectedViewName = "EntryForm";
            Entry expectedEntry = dataSource.Last();

            // Act
            ViewResult result = controller.Edit(expectedEntry.Id) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
            Assert.AreEqual(expectedEntry.Term, ((Entry)result.Model).Term);
        }
        

        [TestMethod]
        public void Edit_InValidEntryID_ThrowsHttpException()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            // Assert
            Assert.ThrowsException<HttpException>(() => controller.Edit(76));
        }


        [TestMethod]
        public void Edit_NullEntryID_ThrowsHttpException()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            // Assert
            Assert.ThrowsException<HttpException>(() => controller.Edit(null));
        }

        
        [TestMethod]
        public void Save_UpdatedEntryValues_ReturnsUpdatedDataSet()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            Entry updatedEntry = new Entry()
            {
                Id = dataSource.First().Id,
                Term = dataSource.First().Term,
                Definition = dataSource.First().Definition
            };
            
            updatedEntry.Term = "New Term Value From User";
            updatedEntry.Definition = "New Defintion Text to Replace Defintion From User Input";

            // Act
            controller.Save(updatedEntry);

            // Assert
            Assert.AreEqual(dataSource.First().Term, updatedEntry.Term);
            Assert.AreEqual(dataSource.First().Definition, updatedEntry.Definition);
        }


        [TestMethod]
        public void Save_UpdatedEntryValuesForInvalidEntry_ThrowsHttpException()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            Entry updatedEntry = new Entry()
            {
                Id = 9999,
                Term = dataSource.First().Term,
                Definition = dataSource.First().Definition
            };

            // Assert
            Assert.ThrowsException<HttpException>(() => controller.Save(updatedEntry));
        }


        [TestMethod]
        public void Delete_DeleteEntryId_ReturnsUpdatedDataSet()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            int deleteEntryId = dataSource.First().Id;
            int expectedDataSourceCount = dataSource.Count() - 1;

            // Act
            controller.Delete(deleteEntryId);
            var actualModel = mockContext.Object.Entries.Count();

            // Assert
            Assert.AreEqual(expectedDataSourceCount, mockContext.Object.Entries.Count());
        }


        [TestMethod]
        public void Delete_InValidEntryID_ThrowsHttpException()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            // Assert
            Assert.ThrowsException<HttpException>(() => controller.Delete(555));
        }


        [TestMethod]
        public void Delete_NullEntryID_ThrowsHttpException()
        {
            // Arrange
            var dataSource = GetEntriesData();
            var mockSet = new MockDbSet<Entry>(dataSource);
            var mockContext = new Mock<GlossaryContext>();
            mockContext.Setup(c => c.Set<Entry>()).Returns(mockSet.Object);

            var controller = new EntryController(mockContext.Object);

            // Assert
            Assert.ThrowsException<HttpException>(() => controller.Delete(null));
        }



        /// <summary>
        /// Helper method to populate data set for tests.
        /// </summary>
        /// <returns></returns>
        private List<Entry> GetEntriesData()
        {
            return new List<Entry>()
            {
                new Entry() {
                    Id = 1,
                    Term = "First Sample Term",
                    Definition = "Long Text of the Sample Definition for First Term"
                },
                new Entry()
                {
                    Id = 2,
                    Term = "Second Sample Term",
                    Definition = "Shorter text for 2nd Term"
                },
                new Entry()
                {
                    Id = 3,
                    Term = "Third Term",
                    Definition = "Longer text for 3rd Term. Lorem ipsum dolor sit amet," +
                        " consectetur adipiscing elit, sed do eiusmod tempor incididunt ut" +
                        " labore et dolore magna aliqua."
                },
                new Entry()
                {
                    Id = 4,
                    Term = "Fourth Term",
                    Definition = "Normal text for 4rth Term. Lorem ipsum dolor sit amet," +
                        " consectetur adipiscing elit."
                }
            };
        }

    }


    /// <summary>
    /// Helps mock the DB set for the tests.
    /// </summary>
    public class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
    {
        public MockDbSet(List<TEntity> dataSource = null)
        {
            var data = (dataSource ?? new List<TEntity>());
            var queryable = data.AsQueryable();

            this.As<IQueryable<TEntity>>().Setup(e => e.Provider).Returns(queryable.Provider);
            this.As<IQueryable<TEntity>>().Setup(e => e.Expression).Returns(queryable.Expression);
            this.As<IQueryable<TEntity>>().Setup(e => e.ElementType).Returns(queryable.ElementType);
            this.As<IQueryable<TEntity>>().Setup(e => e.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // Mock add for inserting items
            this.Setup(_ => _.Add(It.IsAny<TEntity>())).Returns((TEntity arg) => {
                data.Add(arg);
                return arg;
            });

            // Mock remove for removing items
            this.Setup(_ => _.Remove(It.IsAny<TEntity>())).Returns((TEntity arg) => {
                data.Remove(arg);
                return arg;
            });
        }
    }

}
