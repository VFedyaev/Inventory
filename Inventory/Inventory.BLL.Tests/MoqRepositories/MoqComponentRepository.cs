using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqComponentRepository
    {
        public Mock<IRepository<Component>> repository;
        public List<Component> Components { get; }

        public MoqComponentRepository(List<ComponentType> ComponentTypes)
        {
            repository = new Mock<IRepository<Component>>();
            Components = new List<Component>
            {
                new Component
                {
                    Id = Guid.NewGuid(),
                    ComponentTypeId = ComponentTypes.Where(c => c.Name == "мышь").First().Id,
                    ModelName = "Genius m-01",
                    Name = "Genuis",
                    Description = "None",
                    Price = 1,
                    InventNumber = "0001",
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
                    InventNumber = "0002",
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
                    InventNumber = "0003",
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
                    InventNumber = "0004",
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
                    InventNumber = "0005",
                    Supplier = "AcerTech"
                }
            };

            repository
                .Setup(r => r.Create(It.IsAny<Component>()))
                .Callback<Component>(Create);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<Component, bool>>()))
                .Returns((Func<Component, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<Component>()))
                .Callback((Component item) => Update(item));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Create(Component item)
        {
            item.Id = Guid.NewGuid();
            Components.Add(item);
        }

        public Component Get(Guid? id)
        {
            return Components.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<Component> GetAll()
        {
            return Components;
        }

        public IEnumerable<Component> Find(Func<Component, bool> predicate)
        {
            return Components.Where(predicate);
        }

        public void Update(Component item)
        {
            var component = Get(item.Id);

            component.ComponentTypeId = item.ComponentTypeId;
            component.ModelName = item.ModelName;
            component.Name = item.Name;
            component.Description = item.Description;
            component.Price = item.Price;
            component.InventNumber = item.InventNumber;
            component.Supplier = item.Supplier;

            component.EquipmentComponentRelations = item.EquipmentComponentRelations;
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                Components.Remove(item);
        }
    }
}
