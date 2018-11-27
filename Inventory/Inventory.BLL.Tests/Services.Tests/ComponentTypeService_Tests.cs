using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inventory.BLL.Tests.Services.Tests
{
    [TestClass]
    public class ComponentTypeService_Tests : BaseInit
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.ComponentAndComponentTypeInit();
        }

        [TestMethod]
        public void GetMethodReturnsComponentTypeDTOItem()
        {
            // arrange
            var entityItemType = moqComponentTypeRepository.Types.First().GetType();
            var expectedItemId = moqComponentTypeRepository.Types.First().Id;
            var expectedItemName = moqComponentTypeRepository.Types.First().Name;

            // act
            var returnedItem = ComponentTypeService.Get(expectedItemId) as ComponentTypeDTO;

            // assert
            Assert.AreNotEqual(entityItemType, returnedItem.GetType());
            Assert.AreEqual(expectedItemId, returnedItem.Id);
            Assert.AreEqual(expectedItemName, returnedItem.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMethodWithNullIdThrowsArgumentNullException()
        {
            // act // assert
            ComponentTypeService.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetMethodWithWrongIdThrowsNotFoundException()
        {
            // act // assert
            ComponentTypeService.Get((Guid?)Guid.NewGuid());
        }

        [TestMethod]
        public void GetAllMethodReturnsAllItemsList()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Types.Count();

            // act
            var items = ComponentTypeService.GetAll() as IEnumerable<ComponentTypeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetListOrderedByNameReturnsOrderedAllItemsList()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Types.Count();
            Guid expectedFirstItemId = moqComponentTypeRepository.Types.OrderBy(t => t.Name).First().Id;
            Guid expectedLastItemId = moqComponentTypeRepository.Types.OrderBy(t => t.Name).Last().Id;

            // act
            var items = ComponentTypeService.GetListOrderedByName() as IEnumerable<ComponentTypeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.AreEqual(expectedFirstItemId, items.First().Id);
            Assert.AreEqual(expectedLastItemId, items.Last().Id);
        }

        [TestMethod]
        public void AddMethodCreatesNewItem()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Types.Count() + 1;
            var itemToAdd = new ComponentTypeDTO
            {
                Name = $"New-{DateTime.Now}"
            };

            // act
            ComponentTypeService.Add(itemToAdd);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.Types.Count());
            Assert.IsTrue(moqComponentTypeRepository.Types.Any(t => t.Name == itemToAdd.Name));
            Assert.IsNotNull(moqComponentTypeRepository.Types.Where(t => t.Name == itemToAdd.Name).First().Id);
        }

        [TestMethod]
        public void UpdateMethodUpdatesItemFields()
        {
            // arrange
            var expectedItemId = moqComponentTypeRepository.Types.First().Id;
            var nameToChange = moqComponentTypeRepository.Types.First().Name;
            var item = new ComponentTypeDTO
            {
                Id = expectedItemId,
                Name = $"NewName-{DateTime.Now}"
            };

            // act
            ComponentTypeService.Update(item);
            var itemAfterUpdate = moqComponentTypeRepository.Types.First();

            // assert
            Assert.AreEqual(expectedItemId, itemAfterUpdate.Id);
            Assert.AreEqual(itemAfterUpdate.Name, item.Name);
            Assert.AreNotEqual(nameToChange, itemAfterUpdate.Name);
        }

        [TestMethod]
        public void DeleteMethodRemovesItemIfItHasNotRelations()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Types.Count() - 1;
            var removedItemId = moqComponentTypeRepository.Types.Last().Id;

            // act
            ComponentTypeService.Delete(removedItemId);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.Types.Count());
            Assert.IsTrue(!moqComponentTypeRepository.Types.Any(t => t.Id == removedItemId));
        }

        [TestMethod]
        [ExpectedException(typeof(HasRelationsException))]
        public void DeleteMethodThrowsExceptionIfItemHasRelations()
        {
            // act // assert
            ComponentTypeService.Delete(moqComponentTypeRepository.Types.First().Id);
        }
    }
}
