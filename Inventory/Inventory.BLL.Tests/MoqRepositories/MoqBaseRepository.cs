using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqBaseRepository<T> : IRepository<T> where T : class
    {
        public Mock<IRepository<T>> repository;
        public List<T> Items { get; }

        public MoqBaseRepository(List<T> items)
        {
            repository = new Mock<IRepository<T>>();
            Items = items;

            repository
                .Setup(r => r.Create(It.IsAny<T>()))
                .Callback<T>(Create);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<T, bool>>()))
                .Returns((Func<T, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<T>()))
                .Callback((T item) => Update(item));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Create(T item)
        {
            Items.Add(item);
        }

        public T Get(Guid? id)
        {
            return Items.Where(i => GetItemIdValue(i) == id).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return Items;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return Items.Where(predicate);
        }

        public void Update(T item)
        {
            var entity = Get(GetItemIdValue(item));
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                property.SetValue(entity, property.GetValue(item));
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                Items.Remove(item);
        }

        private Guid GetItemIdValue(T item)
        {
            return (Guid)item.GetType().GetProperties().Where(p => p.Name == "Id").First().GetValue(item);
        }
    }
}
