using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqEquipmentRepository
    {
        public Mock<IRepository<Equipment>> repository;
        public List<Equipment> Equipments { get; }

        public MoqEquipmentRepository(List<EquipmentType> Types)
        {
            repository = new Mock<IRepository<Equipment>>();
            Equipments = new List<Equipment>
            {
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "компьютер").First().Id,
                    InventNumber = "E0001",
                    QRCode = null,
                    Price = 320.5m,
                    Supplier = "Io"
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "компьютер").First().Id,
                    InventNumber = "E0002",
                    QRCode = null,
                    Price = 310.3m,
                    Supplier = "DAAD"
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "принтер").First().Id,
                    InventNumber = "E0003",
                    QRCode = null,
                    Price = 120.7m,
                    Supplier = "Io"
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "сервер").First().Id,
                    InventNumber = "E0004",
                    QRCode = null,
                    Price = 990,
                    Supplier = "DAAD"
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "сканер").First().Id,
                    InventNumber = "E0005",
                    QRCode = null,
                    Price = 110,
                    Supplier = "Io"
                },
                new Equipment
                {
                    Id = Guid.NewGuid(),
                    EquipmentTypeId = Types.Where(t => t.Name == "сканер").First().Id,
                    InventNumber = "E0006",
                    QRCode = null,
                    Price = 100,
                    Supplier = "New"
                }
            };

            repository
                .Setup(r => r.Create(It.IsAny<Equipment>()))
                .Callback<Equipment>(Create);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<Equipment, bool>>()))
                .Returns((Func<Equipment, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<Equipment>()))
                .Callback((Equipment item) => Update(item));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Create(Equipment item)
        {
            Equipments.Add(item);
        }

        public Equipment Get(Guid? id)
        {
            return Equipments.Where(e => e.Id == id).FirstOrDefault();
        }

        public IEnumerable<Equipment> GetAll()
        {
            return Equipments;
        }

        public IEnumerable<Equipment> Find(Func<Equipment, bool> predicate)
        {
            return Equipments.Where(predicate);
        }

        public void Update(Equipment item)
        {
            var equipment = Get(item.Id);

            equipment.EquipmentTypeId = item.EquipmentTypeId;
            equipment.InventNumber = item.InventNumber;
            equipment.QRCode = item.QRCode;
            equipment.Price = item.Price;
            equipment.Supplier = item.Supplier;

            equipment.EquipmentComponentRelations = item.EquipmentComponentRelations;
            equipment.EquipmentEmployeeRelations = item.EquipmentEmployeeRelations;
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                Equipments.Remove(item);
        }
    }
}
