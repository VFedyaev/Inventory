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
            base.Initialize();
        }

        [TestMethod]
        public void Get_Method_Returns_ComponentTypeDTO_Item()
        {
            // arrange
            var expectedItemId = moqComponentTypeRepository.Items.First().Id;

            // act
            var returnedItem = ComponentTypeService.Get(expectedItemId) as ComponentTypeDTO;

            // assert
            Assert.AreEqual(expectedItemId, returnedItem.Id);
        }

        [TestMethod]
        public void Get_Method_Returns_Item_With_ComponentTypeDTO_Type()
        {
            // arrange
            Type entityType = moqComponentTypeRepository.Items.First().GetType();

            // act
            var item = ComponentTypeService.Get(moqComponentTypeRepository.Items.First().Id) as ComponentTypeDTO;

            // assert
            Assert.AreNotEqual(entityType, item.GetType());
        }

        [TestMethod]
        public void Get_Method_Returns_Item_With_His_Fields()
        {
            // arrange
            var expectedName = moqComponentRepository.Items.First().ModelName;

            // act
            var item = ComponentService.Get(moqComponentRepository.Items.First().Id);

            // assert
            Assert.AreEqual(expectedName, item.ModelName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Method_With_Null_IdThrows_ArgumentNullException()
        {
            // act // assert
            ComponentTypeService.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Get_Method_With_Wrong_Id_Throws_NotFoundException()
        {
            // act // assert
            ComponentTypeService.Get((Guid?)Guid.NewGuid());
        }

        [TestMethod]
        public void GetAll_Method_Returns_All_Items_List()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Items.Count();

            // act
            var items = ComponentTypeService.GetAll() as IEnumerable<ComponentTypeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetListOrderedByName_Returns_Ordered_Items_List()
        {
            // arrange
            Guid expectedFirstItemId = moqComponentTypeRepository.Items.OrderBy(t => t.Name).First().Id;
            Guid expectedLastItemId = moqComponentTypeRepository.Items.OrderBy(t => t.Name).Last().Id;

            // act
            var items = ComponentTypeService.GetListOrderedByName();

            // assert
            Assert.AreEqual(expectedFirstItemId, items.First().Id);
            Assert.AreEqual(expectedLastItemId, items.Last().Id);
        }

        [TestMethod]
        public void GetListOrderedByName_Returns_All_Items()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Items.Count();

            // act
            var items = ComponentTypeService.GetListOrderedByName() as IEnumerable<ComponentTypeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void Add_Method_Creates_New_Item()
        {
            // arrange
            var item = new ComponentTypeDTO { Name = $"New-{DateTime.Now}" };

            // act
            ComponentTypeService.Add(item);

            // assert
            Assert.IsTrue(moqComponentTypeRepository.Items.Any(t => t.Name == item.Name));
        }

        [TestMethod]
        public void After_Add_Method_Number_Of_Items_Increases()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Items.Count() + 1;
            var item = new ComponentTypeDTO { Name = $"New-{DateTime.Now}" };

            // act
            ComponentTypeService.Add(item);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.Items.Count());
        }

        [TestMethod]
        public void Add_Method_Generates_Id_For_New_Item()
        {
            // arrange
            var item = new ComponentTypeDTO { Name = $"New-{DateTime.Now}" };

            // act
            ComponentTypeService.Add(item);

            // assert
            Assert.IsNotNull(moqComponentTypeRepository.Items.Where(t => t.Name == item.Name).First().Id);
        }

        [TestMethod]
        public void Update_Method_Updates_Item_Fields()
        {
            // arrange
            var expectedItemId = moqComponentTypeRepository.Items.First().Id;
            var nameToChange = moqComponentTypeRepository.Items.First().Name;
            var item = new ComponentTypeDTO
            {
                Id = expectedItemId,
                Name = $"NewName-{DateTime.Now}"
            };

            // act
            ComponentTypeService.Update(item);
            var itemAfterUpdate = moqComponentTypeRepository.Items.First();

            // assert
            Assert.AreEqual(expectedItemId, itemAfterUpdate.Id);
            Assert.AreEqual(item.Name, itemAfterUpdate.Name);
        }

        [TestMethod]
        public void Delete_Method_Removes_Item_If_It_Has_Not_Relations()
        {
            // arrange
            int expectedItemsCount = moqComponentTypeRepository.Items.Count() - 1;
            var removedItemId = moqComponentTypeRepository.Items.Last().Id;

            // act
            ComponentTypeService.Delete(removedItemId);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentTypeRepository.Items.Count());
            Assert.IsTrue(!moqComponentTypeRepository.Items.Any(t => t.Id == removedItemId));
        }

        [TestMethod]
        [ExpectedException(typeof(HasRelationsException))]
        public void Delete_Method_Throws_Exception_If_Item_Has_Relations()
        {
            // act // assert
            ComponentTypeService.Delete(moqComponentTypeRepository.Items.First().Id);
        }
    }
}
