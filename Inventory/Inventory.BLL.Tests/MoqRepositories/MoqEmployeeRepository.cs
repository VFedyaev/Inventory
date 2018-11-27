using CatalogEntities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqEmployeeRepository
    {
        public Mock<IPartialRepository<Employee>> repository;
        public List<Employee> Employees { get; }

        public MoqEmployeeRepository()
        {
            repository = new Mock<IPartialRepository<Employee>>();
            Employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    EmployeeFullName = "",
                    EmployeeRoom = "",
                    EmployeePhone = "",
                    EmployeeEmail = "",
                    PositionId = 0,
                    DepartmentId = 0
                }
            };

            repository
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((int id) => Get(id));
            repository
                .Setup(r => r.Find(It.IsAny<Func<Employee, bool>>()))
                .Returns((Func<Employee, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
        }

        public Employee Get(int? id)
        {
            return Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
        }

        public IEnumerable<Employee> Find(Func<Employee, bool> predicate)
        {
            return Employees.Where(predicate);
        }

        public IEnumerable<Employee> GetAll()
        {
            return Employees;
        }
    }
}
