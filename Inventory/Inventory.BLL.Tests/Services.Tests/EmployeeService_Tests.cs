using CatalogEntities;
using Inventory.BLL.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.Services.Tests
{
    [TestClass]
    public class EmployeeService_Tests : BaseInit
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.PartialInitialize();
        }

        [TestMethod]
        public void Get_Method_Returns_Item_Of_EmployeeDTO_Type()
        {
            // arrange
            var entityType = typeof(Employee);
            var employees = moqEmployeeRepository.Items;
            // act
            var employee = EmployeeService.Get(moqEmployeeRepository.Items.First().EmployeeId) as EmployeeDTO;

            // assert
            Assert.AreNotEqual(entityType, employee.GetType());
        }

        [TestMethod]
        public void Get_Method_Returns_Expected_Item()
        {
            // arrange
            var expectedName = moqEmployeeRepository.Items.First().EmployeeFullName;

            // act
            var item = EmployeeService.Get(moqEmployeeRepository.Items.First().EmployeeId);

            // assert
            Assert.AreEqual(expectedName, item.EmployeeFullName);
        }

        [TestMethod]
        public void GetAll_Method_Returns_All_Items_List()
        {
            // arrange
            int expectedItemsCount = moqEmployeeRepository.Items.Count();

            // act
            var items = EmployeeService.GetAll() as IEnumerable<EmployeeDTO>;

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_With_Empty_Name_Returns_Empty_List()
        {
            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName("") as IEnumerable<OwnerInfoDTO>;

            // assert
            Assert.IsTrue(items.Count() == 0);
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_Finds_Employees_By_Name()
        {
            // arrange
            string name = moqEmployeeRepository.Items.First().EmployeeFullName.Split(' ')[1];
            int expectedItemsCount = moqEmployeeRepository.Items.Where(e => e.EmployeeFullName.Contains(name)).Count();

            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName(name);

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.IsTrue(items.All(i => i.FullName.Contains(name)));
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_Finds_Employees_By_Name_And_LastName()
        {
            // arrange
            string fullName = moqEmployeeRepository.Items.First().EmployeeFullName;
            string lastName = fullName.Split(' ')[0];
            string name = fullName.Split(' ')[1];
            int expectedItemsCount = moqEmployeeRepository.Items.Where(e => e.EmployeeFullName.Contains(name) && e.EmployeeFullName.Contains(lastName)).Count();

            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName($"{name} {lastName}");

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.IsTrue(items.All(i => i.FullName.Contains(name) && i.FullName.Contains(lastName)));
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_Finds_Employees_By_Three_Params()
        {
            // arrange
            string fullName = moqEmployeeRepository.Items.First().EmployeeFullName;
            string lastName = fullName.Split(' ')[0];
            string name = fullName.Split(' ')[1];
            string middleName = fullName.Split(' ')[2];
            int expectedItemsCount = moqEmployeeRepository.Items.Where(e =>
            {
                return e.EmployeeFullName.Contains(lastName) &&
                    e.EmployeeFullName.Contains(name) &&
                    e.EmployeeFullName.Contains(middleName);
            }).Count();

            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName(fullName);

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.IsTrue(items.All(i =>
            {
                return i.FullName.Contains(lastName) && i.FullName.Contains(name) && i.FullName.Contains(middleName);
            }));
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_Ignores_LowerCase()
        {
            // arrange
            string name = moqEmployeeRepository.Items.First().EmployeeFullName.Split(' ')[1];
            int expectedItemCount = moqEmployeeRepository.Items.Where(e => e.EmployeeFullName.Contains(name)).Count();

            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName(name.ToLower());

            // assert
            Assert.AreEqual(expectedItemCount, items.Count());
            Assert.IsTrue(items.All(i => i.FullName.Contains(name)));
        }

        [TestMethod]
        public void ValidateNameAndGetEmployeesByName_Method_Ignores_UpperCase()
        {
            // arrange
            string name = moqEmployeeRepository.Items.First().EmployeeFullName.Split(' ')[1];
            int expectedItemsCount = moqEmployeeRepository.Items.Where(e => e.EmployeeFullName.Contains(name)).Count();

            // act
            var items = EmployeeService.ValidateNameAndGetEmployeesByName(name.ToUpper());

            // assert
            Assert.AreEqual(expectedItemsCount, items.Count());
            Assert.IsTrue(items.All(i => i.FullName.Contains(name)));
        }
    }
}
