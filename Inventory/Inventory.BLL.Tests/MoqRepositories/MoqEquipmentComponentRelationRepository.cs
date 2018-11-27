using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Tests.MoqRepositories
{
    public class MoqEquipmentComponentRelationRepository
    {
        public Mock<IRepository<EquipmentComponentRelation>> repository;
        public List<EquipmentComponentRelation> Relations { get; }

        public MoqEquipmentComponentRelationRepository(List<ComponentType> Equipments, List<Component> Components)
        {
            repository = new Mock<IRepository<EquipmentComponentRelation>>();
            Relations = new List<EquipmentComponentRelation>
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
        }

        public void Create(EquipmentComponentRelation item)
        {
            Relations.Add(item);
        }

        public EquipmentComponentRelation Get(Guid? id)
        {
            return Relations.Where(r => r.Id == id).FirstOrDefault();
        }

        public IEnumerable<EquipmentComponentRelation> GetAll()
        {
            return Relations;
        }

        public IEnumerable<EquipmentComponentRelation> Find(Func<EquipmentComponentRelation, bool> predicate)
        {
            return Relations.Where(predicate);
        }

        public void Update(EquipmentComponentRelation item)
        {
            var relation = Get(item.Id);

            relation.EquipmentId = item.EquipmentId;
            relation.ComponentId = item.ComponentId;
            relation.CreatedAt = item.CreatedAt;
            relation.UpdatedAt = item.UpdatedAt;
            relation.IsActual = item.IsActual;

            relation.Equipment = item.Equipment;
            relation.Component = item.Component;
        }

        public void Delete(Guid id)
        {
            var item = Get(id);
            if (item != null)
                Relations.Remove(item);
        }
    }
}
