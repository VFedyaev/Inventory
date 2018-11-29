using CatalogEntities;
using Inventory.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class TestData
    {
        public static List<Division> Divisions = new List<Division>
        {
            new Division
            {
                DivisionId = 0,
                DivisionName = "Баткенская область"
            },
            new Division
            {
                DivisionId = 1,
                DivisionName = "Город Бишкек"
            },
            new Division
            {
                DivisionId = 2,
                DivisionName = "Город Ош"
            },
            new Division
            {
                DivisionId = 3,
                DivisionName = "Чуйская область"
            }
        };

        public static List<Administration> Administrations = new List<Administration>
        {
            new Administration
            {
                AdministrationId = 0,
                AdministrationName = "Баткенское областное управление",
                DivisionId = 0
            },
            new Administration
            {
                AdministrationId = 1,
                AdministrationName = "Баткенский районный отдел",
                DivisionId = 0
            },
            new Administration
            {
                AdministrationId = 2,
                AdministrationName = "Бишкекское городское управление",
                DivisionId = 1
            },
            new Administration
            {
                AdministrationId = 3,
                AdministrationName = "Ошское городское управление",
                DivisionId = 2
            },
            new Administration
            {
                AdministrationId = 4,
                AdministrationName = "Ошское областное управление",
                DivisionId = 2
            },
            new Administration
            {
                AdministrationId = 5,
                AdministrationName = "Чуйское областное управление",
                DivisionId = 3
            }
        };

        public static List<Department> Departments = new List<Department>
        {
            new Department
            {
                DepartmentId = 0,
                DepartmentName = "Отдел статистики сельского хозяйства",
                AdministrationId = 0
            },
            new Department
            {
                DepartmentId = 1,
                DepartmentName = "Отдел сбора и обработки информации",
                AdministrationId = 0
            },
            new Department
            {
                DepartmentId = 2,
                DepartmentName = "Отдел статистики промышленности",
                AdministrationId = 1
            },
            new Department
            {
                DepartmentId = 3,
                DepartmentName = "Отдел взаимной торговли",
                AdministrationId = 1
            },
            new Department
            {
                DepartmentId = 4,
                DepartmentName = "Отдел бухгалтерского учета",
                AdministrationId = 5
            }
        };

        public static List<Position> Positions = new List<Position>
        {
            new Position
            {
                PositionId = 0,
                PositionName = "заведующий отделом"
            },
            new Position
            {
                PositionId = 1,
                PositionName = "главный специалист"
            },
            new Position
            {
                PositionId = 2,
                PositionName = "ведущий специалист"
            }
        };

        public static List<Employee> Employees = new List<Employee>
        {
            new Employee
            {
                EmployeeId = 0,
                EmployeeFullName = "Пашин Владилен Елизарович",
                EmployeeRoom = "120",
                EmployeePhone = "-",
                EmployeeEmail = "-",
                PositionId = 0,
                DepartmentId = 0
            },
            new Employee
            {
                EmployeeId = 1,
                EmployeeFullName = "Ермишина Ярослава Филипповна",
                EmployeeRoom = "122",
                EmployeePhone = "-",
                EmployeeEmail = "-",
                PositionId = 0,
                DepartmentId = 1
            },
            new Employee
            {
                EmployeeId = 2,
                EmployeeFullName = "Яманов Артем Прохорович",
                EmployeeRoom = "120",
                EmployeePhone = "-",
                EmployeeEmail = "-",
                PositionId = 1,
                DepartmentId = 0
            },
            new Employee
            {
                EmployeeId = 3,
                EmployeeFullName = "Лютова Оксана Родионовна",
                EmployeeRoom = "122",
                EmployeePhone = "-",
                EmployeeEmail = "-",
                PositionId = 1,
                DepartmentId = 1
            }
        };

        public static List<ComponentType> ComponentTypes = new List<ComponentType>
        {
            new ComponentType
            {
                Id = Guid.NewGuid(),
                Name = "мышь"
            },
            new ComponentType
            {
                Id = Guid.NewGuid(),
                Name = "клавиатура"
            },
            new ComponentType
            {
                Id = Guid.NewGuid(),
                Name = "монитор"
            },
            new ComponentType
            {
                Id = Guid.NewGuid(),
                Name = "жесткий диск"
            }
        };

        public static List<Component> Components = new List<Component>
        {
            new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = ComponentTypes.Where(c => c.Name == "мышь").First().Id,
                ModelName = "Genius m-01",
                Name = "Genuis",
                Description = "None",
                Price = 1,
                InventNumber = "C0001",
                Supplier = "Io"
            },
            new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = ComponentTypes.Where(c => c.Name == "мышь").First().Id,
                ModelName = "Acer m-01",
                Name = "Acer",
                Description = "Acer mouse",
                Price = 1.2m,
                InventNumber = "C0002",
                Supplier = "AcerTech"
            },
            new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = ComponentTypes.Where(c => c.Name == "клавиатура").First().Id,
                ModelName = "Acer k-01",
                Name = "Acer",
                Description = "Acer keyboard",
                Price = 1.1m,
                InventNumber = "C0003",
                Supplier = "AcerTech"
            },
            new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = ComponentTypes.Where(c => c.Name == "клавиатура").First().Id,
                ModelName = "Genius k-01",
                Name = "Genius",
                Description = "Genius keyboard",
                Price = 1.1m,
                InventNumber = "C0004",
                Supplier = "Io"
            },
            new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = ComponentTypes.Where(c => c.Name == "монитор").First().Id,
                ModelName = "Acer mtr-01",
                Name = "Acer",
                Description = "Acer monitor",
                Price = 2.1m,
                InventNumber = "C0005",
                Supplier = "AcerTech"
            }
        };

        public static List<EquipmentType> EquipmentTypes = new List<EquipmentType>
        {
            new EquipmentType
            {
                Id = Guid.NewGuid(),
                Name = "компьютер"
            },
            new EquipmentType
            {
                Id = Guid.NewGuid(),
                Name = "принтер"
            },
            new EquipmentType
            {
                Id = Guid.NewGuid(),
                Name = "сервер"
            },
            new EquipmentType
            {
                Id = Guid.NewGuid(),
                Name = "сканер"
            }
        };

        public static List<Equipment> Equipments = new List<Equipment>
        {
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "компьютер").First().Id,
                InventNumber = "E0001",
                QRCode = null,
                Price = 320.5m,
                Supplier = "Io"
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "компьютер").First().Id,
                InventNumber = "E0002",
                QRCode = null,
                Price = 310.3m,
                Supplier = "DAAD"
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "принтер").First().Id,
                InventNumber = "E0003",
                QRCode = null,
                Price = 120.7m,
                Supplier = "Io"
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "сервер").First().Id,
                InventNumber = "E0004",
                QRCode = null,
                Price = 990,
                Supplier = "DAAD"
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "сканер").First().Id,
                InventNumber = "E0005",
                QRCode = null,
                Price = 110,
                Supplier = "Io"
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentTypeId = EquipmentTypes.Where(t => t.Name == "сканер").First().Id,
                InventNumber = "E0006",
                QRCode = null,
                Price = 100,
                Supplier = "New"
            }
        };

        public static List<EquipmentComponentRelation> EquipmentComponentRelations = new List<EquipmentComponentRelation>
        {
            new EquipmentComponentRelation
            {
                Id = Guid.NewGuid(),
                EquipmentId = Equipments.First().Id,
                ComponentId = Components.First().Id,
                CreatedAt = DateTime.Today.AddDays(-1),
                UpdatedAt = DateTime.Today.AddDays(-1),
                IsActual = false
            },
            new EquipmentComponentRelation
            {
                Id = Guid.NewGuid(),
                EquipmentId = Equipments.First().Id,
                ComponentId = Components.Last().Id,
                CreatedAt = DateTime.Today.AddDays(-2),
                UpdatedAt = DateTime.Today.AddDays(-2),
                IsActual = false
            },
            new EquipmentComponentRelation
            {
                Id = Guid.NewGuid(),
                EquipmentId = Equipments.Last().Id,
                ComponentId = Components.Take(2).Last().Id,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today,
                IsActual = false
            }
        };

        public static List<EquipmentEmployeeRelation> EquipmentEmployeeRelations = new List<EquipmentEmployeeRelation>
        {
            new EquipmentEmployeeRelation
            {
                Id = Guid.NewGuid(),
                EmployeeId = Employees.First().EmployeeId,
                EquipmentId = Equipments.First().Id,
                CreatedAt = DateTime.Today.AddDays(-2),
                UpdatedAt = DateTime.Today.AddDays(-2),
                IsOwner = false
            },
            new EquipmentEmployeeRelation
            {
                Id = Guid.NewGuid(),
                EmployeeId = Employees.Last().EmployeeId,
                EquipmentId = Equipments.First().Id,
                CreatedAt = DateTime.Today.AddDays(-1),
                UpdatedAt = DateTime.Today.AddDays(-1),
                IsOwner = true
            },
            new EquipmentEmployeeRelation
            {
                Id = Guid.NewGuid(),
                EmployeeId = Employees.First().EmployeeId,
                EquipmentId = Equipments.Last().Id,
                CreatedAt = DateTime.Today.AddDays(-3),
                UpdatedAt = DateTime.Today.AddDays(-3),
                IsOwner = false
            },
            new EquipmentEmployeeRelation
            {
                Id = Guid.NewGuid(),
                EmployeeId = Employees.Last().EmployeeId,
                EquipmentId = Equipments.Last().Id,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today,
                IsOwner = true
            }
        };

        public static List<StatusType> StatusTypes = new List<StatusType>
        {
            new StatusType
            {
                Id = Guid.NewGuid(),
                Name = "в ремонте"
            },
            new StatusType
            {
                Id = Guid.NewGuid(),
                Name = "работает"
            },
            new StatusType
            {
                Id = Guid.NewGuid(),
                Name = "на складе"
            },
            new StatusType
            {
                Id = Guid.NewGuid(),
                Name = "сломан"
            }
        };

        public static List<RepairPlace> RepairPlaces = new List<RepairPlace>
        {
            new RepairPlace
            {
                Id = Guid.NewGuid(),
                Name ="ремонтное бюро"
            },
            new RepairPlace
            {
                Id = Guid.NewGuid(),
                Name = "сервисный центр"
            },
            new RepairPlace
            {
                Id = Guid.NewGuid(),
                Name = "на дому"
            }
        };

        public static List<History> Histories = new List<History>
        {
            new History
            {
                Id = Guid.NewGuid(),
                EquipmentId = Equipments.First().Id,
                EmployeeId = Employees.First().EmployeeId,
                RepairPlaceId = RepairPlaces.First().Id,
                StatusTypeId = StatusTypes.First().Id,
                ChangeDate = DateTime.Today.AddDays(-5),
                Comments = "-"
            },
            new History
            {
                Id = Guid.NewGuid(),
                EquipmentId = Equipments.Take(2).Last().Id,
                EmployeeId = Employees.Take(2).Last().EmployeeId,
                RepairPlaceId = RepairPlaces.Take(2).Last().Id,
                StatusTypeId = StatusTypes.Take(2).Last().Id,
                ChangeDate = DateTime.Today.AddDays(-1),
                Comments = "-"
            }
        };
    }
}
