using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using CatalogEntities;

namespace Inventory.DAL.EF
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentComponentRelation> EquipmentComponentRelations { get; set; }
        public DbSet<EquipmentEmployeeRelation> EquipmentEmployeeRelations { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<RepairPlace> RepairPlaces { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        
        public DbSet<Administration> Administrations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public InventoryContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<InventoryContext>(null);
        }
    }
}
