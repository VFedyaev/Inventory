using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqComponentTypeRepository
    {
        public Mock<IRepository<ComponentType>> repository;
        public List<ComponentType> ComponentTypes { get; }

        public MoqComponentTypeRepository()
        {
            repository = new Mock<IRepository<ComponentType>>();
            ComponentTypes = new List<ComponentType>
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

            repository
                .Setup(r => r.Create(It.IsAny<ComponentType>()))
                .Callback<ComponentType>(Create);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<ComponentType, bool>>()))
                .Returns((Func<ComponentType, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<ComponentType>()))
                .Callback((ComponentType item) => Update(item));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Create(ComponentType item)
        {
            item.Id = Guid.NewGuid();
            ComponentTypes.Add(item);
        }

        public ComponentType Get(Guid? id)
        {
            return ComponentTypes.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<ComponentType> GetAll()
        {
            return ComponentTypes;
        }

        public IEnumerable<ComponentType> Find(Func<ComponentType, bool> predicate)
        {
            return ComponentTypes.Where(predicate);
        }

        public void Update(ComponentType item)
        {
            var componentType = Get(item.Id);

            componentType.Name = item.Name;
            componentType.Components = item.Components;
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                ComponentTypes.Remove(item);
        }
    }
}
