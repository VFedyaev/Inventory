using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.Services.Tests
{
    [TestClass]
    public class ComponentService_Tests : BaseInit
    {
        [TestInitialize]
        public void TestIntitialize()
        {
            base.ComponentAndComponentTypeInit();
        }

        [TestMethod]
        public void GetMethodReturnsComponentDTOItem()
        {
            // arrange
            var itemEntityType = moqComponentRepository.Components.First().GetType();
            var expectedItemId = moqComponentRepository.Components.First().Id;
            var expectedItemInventNumber = moqComponentRepository.Components.First().InventNumber;

            // act
            var item = ComponentService.Get(expectedItemId) as ComponentDTO;

            // assert
            Assert.AreNotEqual(itemEntityType, item.GetType());
            Assert.AreEqual(expectedItemId, item.Id);
            Assert.AreEqual(expectedItemInventNumber, item.InventNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetmethodWithNullIdThrowsArgumentNullException()
        {
            // act // assert
            ComponentService.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetMethodWithWrongIdThrowsNotFoundException()
        {
            // act //assert
            ComponentService.Get((Guid?)Guid.NewGuid());
        }

        [TestMethod]
        public void GetAllMethodReturnsAllItemsList()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Components.Count();

            // act
            var items = ComponentService.GetAll() as IEnumerable<ComponentDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredListReturnsListFilteredByComponentTypeId()
        {
            // arrange
            Guid ComponentTypeId = moqComponentTypeRepository.Types.First().Id;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = ComponentTypeId.ToString()
            };
            int expectedItemsCount = moqComponentRepository.Components.Where(c => c.ComponentTypeId == ComponentTypeId).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.ComponentTypeId == ComponentTypeId));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredListReturnsListFilteredByModelName()
        {
            // arrange
            string ModelName = moqComponentRepository.Components.First().ModelName;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ModelName = ModelName
            };
            int expectedItemsCount = moqComponentRepository.Components.Where(c => c.ModelName == ModelName).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.ModelName == ModelName));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredListReturnsListFilteredByName()
        {
            // arrange
            string Name = moqComponentRepository.Components.Last().Name;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                Name = Name
            };
            int expectedItemsCount = moqComponentRepository.Components.Where(c => c.Name == Name).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.Name == Name));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredListReturnsListFilteredByAllParams()
        {
            // arrange
            string Name = moqComponentRepository.Components.Last().Name;
            string ModelName = moqComponentRepository.Components.Last().ModelName;
            Guid ComponentTypeId = moqComponentRepository.Components.Last().ComponentTypeId;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                Name = Name,
                ModelName = ModelName,
                ComponentTypeId = ComponentTypeId.ToString()
            };
            int expectedItemsCount = moqComponentRepository.Components.Where(c =>
            {
                return c.Name == Name && c.ModelName == ModelName && c.ComponentTypeId == ComponentTypeId;
            }).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.Name == Name));
            Assert.IsTrue(items.All(c => c.ModelName == ModelName));
            Assert.IsTrue(items.All(c => c.ComponentTypeId == ComponentTypeId));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredListReturnsListOrderedByName()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Components.Count();
            Guid expectedFirstItemId = moqComponentRepository.Components.OrderBy(c => c.Name).First().Id;
            Guid expectedLastItemId = moqComponentRepository.Components.OrderBy(c => c.Name).Last().Id;

            // act
            var items = ComponentService.GetFilteredList(new FilterParamsDTO()) as IEnumerable<ComponentDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.AreEqual(expectedFirstItemId, items.First().Id);
            Assert.AreEqual(expectedLastItemId, items.Last().Id);
        }

        [TestMethod]
        public void AddAndGetIdCreatesItemAndReturnsId()
        {
            // arrange
            ComponentDTO item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Types.First().Id,
                Name = $"Name-{DateTime.Now}",
                ModelName = $"Modelname-{DateTime.Now}"
            };

            // act
            var createdItemId = ComponentService.AddAndGetId(item) as Guid?;

            // assert
            Assert.IsNotNull(createdItemId);
            Assert.IsTrue(moqComponentRepository.Components.Any(c => c.Name == item.Name));
        }

        [TestMethod]
        public void AddMethodCreatesItem()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Components.Count() + 1;
            ComponentDTO item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Types.First().Id,
                Name = $"Name-{DateTime.Now}"
            };

            // act
            ComponentService.Add(item);

            // assert
            Assert.IsTrue(moqComponentRepository.Components.Any(c => c.Name == item.Name));
            Assert.AreEqual(expectedItemsCount, moqComponentRepository.Components.Count());
        }

        [TestMethod]
        public void UpdateMethodUpdatesItemFields()
        {
            // arrange
            var expectedItemId = moqComponentRepository.Components.First().Id;
            string modelNameThatWillBeUpdated = moqComponentRepository.Components.First().ModelName;
            string nameThatWillBeUpdated = moqComponentRepository.Components.First().Name;
            var item = new ComponentDTO
            {
                Id = expectedItemId,
                ModelName = $"NewModelName-{DateTime.Now}",
                Name = $"NewName-{DateTime.Now}"
            };

            // act
            ComponentService.Update(item);
            var updatedItem = moqComponentRepository.Components.Where(c => c.Id == expectedItemId).First();

            // assert
            Assert.AreEqual(expectedItemId, updatedItem.Id);
            Assert.AreNotEqual(modelNameThatWillBeUpdated, updatedItem.ModelName);
            Assert.AreNotEqual(nameThatWillBeUpdated, updatedItem.Name);
            Assert.IsTrue(updatedItem.Price == 0);
            Assert.IsNull(updatedItem.InventNumber);
        }

        [TestMethod]
        public void DeleteMethodRemovesItemWithoutRelations()
        {

        }
    }
}
