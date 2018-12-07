using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            base.ComponentInitialize();
        }

        [TestMethod]
        public void Get_Method_Returns_ComponentDTO_Item()
        {
            // arrange
            var expectedItemId = moqComponentRepository.Items.First().Id;

            // act
            var item = ComponentService.Get(expectedItemId);

            // assert
            Assert.AreEqual(expectedItemId, item.Id);
        }

        [TestMethod]
        public void Get_Method_Returns_Item_With_ComponentDTO_Type()
        {
            // arrange
            var entityType = moqComponentRepository.Items.First().GetType();

            // act
            var item = ComponentService.Get(moqComponentRepository.Items.First().Id) as ComponentDTO;

            // assert
            Assert.AreNotEqual(entityType, item.GetType());
        }

        [TestMethod]
        public void Get_Method_Returns_Item_With_His_Fields()
        {
            // arrange
            var expectedItemInventNumber = moqComponentRepository.Items.First().InventNumber;

            // act
            var item = ComponentService.Get(moqComponentRepository.Items.First().Id);

            // assert
            Assert.AreEqual(expectedItemInventNumber, item.InventNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Method_With_Null_Id_Throws_ArgumentNullException()
        {
            // act // assert
            ComponentService.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Get_Method_With_Wrong_Id_Throws_NotFoundException()
        {
            // act //assert
            ComponentService.Get((Guid?)Guid.NewGuid());
        }

        [TestMethod]
        public void GetAll_Method_Returns_All_Items_List()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Items.Count();

            // act
            var items = ComponentService.GetAll() as IEnumerable<ComponentDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredList_Method_Returns_List_Filtered_By_ComponentTypeId()
        {
            // arrange
            Guid ComponentTypeId = moqComponentTypeRepository.Items.First().Id;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = ComponentTypeId.ToString()
            };
            int expectedItemsCount = moqComponentRepository.Items.Where(c => c.ComponentTypeId == ComponentTypeId).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.ComponentTypeId == ComponentTypeId));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void Get_Filtered_List_Method_Returns_List_Filtered_By_ModelName()
        {
            // arrange
            string ModelName = moqComponentRepository.Items.First().ModelName;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ModelName = ModelName
            };
            int expectedItemsCount = moqComponentRepository.Items.Where(c => c.ModelName == ModelName).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.ModelName == ModelName));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredList_Method_Returns_List_Filtered_By_Name()
        {
            // arrange
            string Name = moqComponentRepository.Items.Last().Name;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                Name = Name
            };
            int expectedItemsCount = moqComponentRepository.Items.Where(c => c.Name == Name).Count();

            // act
            var items = ComponentService.GetFilteredList(parameters) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.All(c => c.Name == Name));
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void GetFilteredList_Method_Returns_List_Filtered_By_All_Params()
        {
            // arrange
            string Name = moqComponentRepository.Items.Last().Name;
            string ModelName = moqComponentRepository.Items.Last().ModelName;
            Guid ComponentTypeId = moqComponentRepository.Items.Last().ComponentTypeId;
            FilterParamsDTO parameters = new FilterParamsDTO
            {
                Name = Name,
                ModelName = ModelName,
                ComponentTypeId = ComponentTypeId.ToString()
            };
            int expectedItemsCount = moqComponentRepository.Items.Where(c =>
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
        public void GetFilteredList_Method_Returns_List_Ordered_By_Name()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Items.Count();
            Guid expectedFirstItemId = moqComponentRepository.Items.OrderBy(c => c.Name).First().Id;
            Guid expectedLastItemId = moqComponentRepository.Items.OrderBy(c => c.Name).Last().Id;

            // act
            var items = ComponentService.GetFilteredList(new FilterParamsDTO()) as IEnumerable<ComponentDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.AreEqual(expectedFirstItemId, items.First().Id);
            Assert.AreEqual(expectedLastItemId, items.Last().Id);
        }

        [TestMethod]
        public void AddAndGetId_Method_Returns_Id()
        {
            // arrange
            ComponentDTO item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Items.First().Id,
                Name = $"Name-{DateTime.Now}",
                ModelName = $"ModelName-{DateTime.Now}",
                InventNumber = $"Number-{DateTime.Now}"
            };

            // act
            var id = ComponentService.AddAndGetId(item) as Guid?;

            // assert
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void AddAndGetId_Method_Creates_Ite()
        {
            // arrange
            ComponentDTO item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Items.First().Id,
                Name = $"Name-{DateTime.Now}",
                ModelName = $"Modelname-{DateTime.Now}",
                InventNumber = $"Number-{DateTime.Now}"
            };

            // act
            var createdItemId = ComponentService.AddAndGetId(item);

            // assert
            Assert.IsTrue(moqComponentRepository.Items.Any(c => c.Name == item.Name));
        }

        [TestMethod]
        public void Add_Method_Creates_Item()
        {
            // arrange
            ComponentDTO item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Items.First().Id,
                Name = $"Name-{DateTime.Now}",
                ModelName = $"ModelName-{DateTime.Now}",
                InventNumber = $"Number-{DateTime.Now}"
            };

            // act
            ComponentService.Add(item);

            // assert
            Assert.IsTrue(moqComponentRepository.Items.Any(c => c.Name == item.Name));
        }

        [TestMethod]
        public void After_Add_Method_Number_Of_Items_Increases()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Items.Count() + 1;
            var item = new ComponentDTO
            {
                ComponentTypeId = moqComponentTypeRepository.Items.First().Id,
                Name = $"Name-{DateTime.Now}",
                ModelName = $"ModelName-{DateTime.Now}",
                InventNumber = $"Number-{DateTime.Now}"
            };

            // act
            ComponentService.Add(item);
            var allItems = moqComponentRepository.Items;

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentRepository.Items.Count());
        }

        [TestMethod]
        public void Update_Method_Updates_Item_Fields()
        {
            // arrange
            var expectedItemId = moqComponentRepository.Items.First().Id;
            string modelNameThatWillBeUpdated = moqComponentRepository.Items.First().ModelName;
            string nameThatWillBeUpdated = moqComponentRepository.Items.First().Name;
            var item = new ComponentDTO
            {
                Id = expectedItemId,
                ModelName = $"NewModelName-{DateTime.Now}",
                Name = $"NewName-{DateTime.Now}"
            };

            // act
            ComponentService.Update(item);
            var updatedItem = moqComponentRepository.Items.Where(c => c.Id == expectedItemId).First();

            // assert
            Assert.AreEqual(expectedItemId, updatedItem.Id);
            Assert.AreNotEqual(modelNameThatWillBeUpdated, updatedItem.ModelName);
            Assert.AreNotEqual(nameThatWillBeUpdated, updatedItem.Name);
            Assert.IsTrue(updatedItem.Price == 0);
            Assert.IsNull(updatedItem.InventNumber);
        }

        [TestMethod]
        public void Delete_Method_Removes_Item_Without_Relations_With_Equipment()
        {
            // arrange
            int expectedItemsCount = moqComponentRepository.Items.Count() - 1;

            // act
            ComponentService.Delete(moqComponentRepository.Items.Where(i =>
            {
                return !moqEquipCompRelRepository.Items.Select(r => r.ComponentId).Contains(i.Id);
            }).First().Id);

            // assert
            Assert.AreEqual(expectedItemsCount, moqComponentRepository.Items.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(HasRelationsException))]
        public void Delete_Method_Throws_Exception_If_Item_Has_Relations_With_Equipment()
        {
            // act // assert
            ComponentService.Delete(moqEquipCompRelRepository.Items.First().ComponentId);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void Delete_Method_With_Wrong_Id_Throws_NotFoundException()
        {
            // act // assert
            ComponentService.Delete(Guid.NewGuid());
        }

        [TestMethod]
        public void GetComponentsBy_Method_Returns_Component_List_Filtered_By_InventNumber()
        {
            // arrange
            var inventNumber = moqComponentRepository.Items.First().InventNumber;
            var expectedItems = moqComponentRepository.Items.Where(c => c.InventNumber == inventNumber);

            // act
            var items = ComponentService.GetComponentsBy("number", inventNumber.ToLower()) as IEnumerable<ComponentDTO>;

            // assert
            Assert.AreEqual(expectedItems.Count(), items.Count());
            Assert.IsTrue(items.All(i => i.InventNumber == inventNumber));
        }

        [TestMethod]
        public void GetComponentsBy_Method_Returns_Component_List_Filtered_By_Type_Name()
        {
            // arrange
            var componentType = moqComponentTypeRepository.Items.First();
            var expectedItems = moqComponentRepository.Items.Where(c => c.ComponentTypeId == componentType.Id);

            // act
            var items = ComponentService.GetComponentsBy("type", componentType.Name.ToLower());

            // assert
            Assert.AreEqual(expectedItems.Count(), items.Count());
            Assert.IsTrue(items.All(i => i.ComponentTypeId == componentType.Id));
        }

        [TestMethod]
        public void GetComponentsBy_Method_Returns_Component_List_Filtered_By_ModelName()
        {
            // arrange
            var modelName = moqComponentRepository.Items.First().ModelName;
            var expectedItems = moqComponentRepository.Items.Where(c => c.ModelName == modelName);

            // act
            var items = ComponentService.GetComponentsBy("model", modelName.ToLower());

            // assert
            Assert.AreEqual(expectedItems.Count(), items.Count());
            Assert.IsTrue(items.All(i => i.ModelName == modelName));
        }

        [TestMethod]
        public void GetComponentsBy_Method_With_Wrong_InventNUmber_Returns_Empty_List()
        {
            // arrange
            string inventNumber = $"{DateTime.Now}";

            // act
            var items = ComponentService.GetComponentsBy("number", inventNumber) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.Count() == 0);
        }

        [TestMethod]
        public void GetComponentsBy_Method_With_Wrong_Type_Returns_Empty_List()
        {
            // arrange
            string typeName = $"{DateTime.Now}";

            // act
            var items = ComponentService.GetComponentsBy("type", typeName) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.Count() == 0);
        }

        [TestMethod]
        public void GetComponentsBy_Method_With_Wrong_ModelName_Returns_Empty_List()
        {
            // arrange
            string modelName = $"{DateTime.Now}";

            // act
            var items = ComponentService.GetComponentsBy("model", modelName) as IEnumerable<ComponentDTO>;

            // assert
            Assert.IsTrue(items.Count() == 0);
        }
    }
}
