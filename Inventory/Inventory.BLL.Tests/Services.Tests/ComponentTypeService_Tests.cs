using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Services;
using Inventory.BLL.Tests.MoqRepositories;
using Inventory.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Inventory.BLL.Tests.Services.Tests
{
    [TestClass]
    public class ComponentTypeService_Tests : BaseInit
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ResetAndInitializeMapper();
            moqComponentTypeRepository = new MoqComponentTypeRepository();
            moqComponentRepository = new MoqComponentRepository(moqComponentTypeRepository.ComponentTypes);

            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork
                .Setup(u => u.ComponentTypes)
                .Returns(moqComponentTypeRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Components)
                .Returns(moqComponentRepository.repository.Object);
            ComponentTypeService = new ComponentTypeService(moqUnitOfWork.Object);
        }

        [TestMethod]
        public void GetMethodReturnsComponentTypeDTOObject()
        {
            // arrange
            var expectedItem = moqComponentTypeRepository.ComponentTypes.First();

            // act
            var returnedItem = ComponentTypeService.Get(expectedItem.Id) as ComponentTypeDTO;

            // assert
            Assert.AreNotEqual(expectedItem.GetType(), returnedItem.GetType());
            Assert.AreEqual(expectedItem.Id, returnedItem.Id);
            Assert.AreEqual(expectedItem.Name, returnedItem.Name);
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
            int expectedItemsCount = moqComponentTypeRepository.ComponentTypes.Count();

            // act
            var items = ComponentTypeService.GetAll() as IEnumerable<ComponentTypeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetListOrderedByNameReturnsOrderedAllItemsList()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.ComponentTypes.Count();
            Guid expectedFirstItemId = moqComponentTypeRepository.ComponentTypes.OrderBy(t => t.Name).First().Id;
            Guid expectedLastItemId = moqComponentTypeRepository.ComponentTypes.OrderBy(t => t.Name).Last().Id;

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
            int expectedItemsCount = moqComponentTypeRepository.ComponentTypes.Count() + 1;
            var itemToAdd = new ComponentTypeDTO
            {
                Name = $"New-{DateTime.Now}"
            };

            // act
            ComponentTypeService.Add(itemToAdd);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.ComponentTypes.Count());
            Assert.IsTrue(moqComponentTypeRepository.ComponentTypes.Any(t => t.Name == itemToAdd.Name));
            Assert.IsNotNull(moqComponentTypeRepository.ComponentTypes.Where(t => t.Name == itemToAdd.Name).First().Id);
        }

        [TestMethod]
        public void UpdateMethodUpdatesItemFields()
        {
            // arrange
            var expectedItemId = moqComponentTypeRepository.ComponentTypes.First().Id;
            var nameToChange = moqComponentTypeRepository.ComponentTypes.First().Name;
            var item = new ComponentTypeDTO
            {
                Id = expectedItemId,
                Name = $"NewName-{DateTime.Now}"
            };

            // act
            ComponentTypeService.Update(item);
            var itemAfterUpdate = moqComponentTypeRepository.ComponentTypes.First();

            // assert
            Assert.AreEqual(expectedItemId, itemAfterUpdate.Id);
            Assert.AreEqual(itemAfterUpdate.Name, item.Name);
            Assert.AreNotEqual(nameToChange, itemAfterUpdate.Name);
        }

        [TestMethod]
        public void DeleteMethodRemovesItemIfItHasNotRelations()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.ComponentTypes.Count() - 1;
            var removedItemId = moqComponentTypeRepository.ComponentTypes.Last().Id;

            // act
            ComponentTypeService.Delete(removedItemId);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.ComponentTypes.Count());
            Assert.IsTrue(!moqComponentTypeRepository.ComponentTypes.Any(t => t.Id == removedItemId));
        }

        [TestMethod]
        [ExpectedException(typeof(HasRelationsException))]
        public void DeleteMethodThrowsExceptionIfItemHasRelations()
        {
            // act // assert
            ComponentTypeService.Delete(moqComponentTypeRepository.ComponentTypes.First().Id);
        }
    }
}
