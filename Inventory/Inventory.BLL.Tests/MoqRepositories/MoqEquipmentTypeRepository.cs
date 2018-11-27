using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqEquipmentTypeRepository
    {
        public Mock<IRepository<EquipmentType>> repository;

        public List<EquipmentType> Types { get; }

        public MoqEquipmentTypeRepository()
        {
            repository = new Mock<IRepository<EquipmentType>>();
            Types = new List<EquipmentType>
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

            repository
                .Setup(r => r.Create(It.IsAny<EquipmentType>()))
                .Callback<EquipmentType>(Create);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<EquipmentType, bool>>()))
                .Returns((Func<EquipmentType, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<EquipmentType>()))
                .Callback((EquipmentType item) => Update(item));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Create(EquipmentType item)
        {
            Types.Add(item);
        }

        public EquipmentType Get(Guid? id)
        {
            return Types.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<EquipmentType> GetAll()
        {
            return Types;
        }

        public IEnumerable<EquipmentType> Find(Func<EquipmentType, bool> predicate)
        {
            return Types.Where(predicate);
        }

        public void Update(EquipmentType item)
        {
            var type = Get(item.Id);

            type.Name = item.Name;
            type.Equipments = item.Equipments;
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                Types.Remove(item);
        }
    }
}
