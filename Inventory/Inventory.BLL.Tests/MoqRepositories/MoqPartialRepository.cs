using CatalogEntities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqPartialRepository<T> : IPartialRepository<T> where T : class
    {
        public Mock<IPartialRepository<T>> repository;
        public List<T> Items { get; }

        public MoqPartialRepository(List<T> items)
        {
            repository = new Mock<IPartialRepository<T>>();
            Items = items;

            repository
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns((int id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<T, bool>>()))
                .Returns((Func<T, bool> predicate) => Find(predicate));
        }

        public T Get(int? id)
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

        private int GetItemIdValue(T item)
        {
            return GetValue(item);
        }

        private int GetValue(T item)
        {
            return (int)item.GetType().GetProperties().Where(p => p.Name == "Id").First().GetValue(item);
        }
    }
}
