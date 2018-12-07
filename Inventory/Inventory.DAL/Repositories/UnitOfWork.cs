using Inventory.DAL.EF;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using CatalogEntities;
using System;
using Microsoft.AspNet.Identity;
using Inventory.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private InventoryContext context;

        private BaseRepository<Component> componentRepository;
        private BaseRepository<ComponentType> componentTypeRepository;
        private BaseRepository<Equipment> equipmentRepository;
        private BaseRepository<EquipmentComponentRelation> equipmentComponentRelationsRepository;
        private BaseRepository<EquipmentEmployeeRelation> equipmentEmployeeRelationsRepository;
        private BaseRepository<EquipmentType> equipmentTypeRepository;
        private BaseRepository<History> historyRepository;
        private BaseRepository<RepairPlace> repairPlaceRepository;
        private BaseRepository<StatusType> statusTypeRepository;

        private PartialRepository<Employee> employeeRepository;
        private PartialRepository<Position> positionRepository;
        private PartialRepository<Department> departmentRepository;
        private PartialRepository<Administration> administrationRepository;
        private PartialRepository<Division> divisionRepository;

        PasswordValidator passwordValidator = new PasswordValidator
        {
            RequiredLength = 8,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true
        };

        public UnitOfWork(string connectionString)
        {
            context = new InventoryContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            UserManager.PasswordValidator = passwordValidator;
            UserManager.UserValidator = new AppUserValidator(UserManager);
        }

        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        public IRepository<Component> Components
        {
            get
            {
                if (componentRepository == null)
                    componentRepository = new BaseRepository<Component>(context);
                return componentRepository;
            }
        }

        public IRepository<ComponentType> ComponentTypes
        {
            get
            {
                if (componentTypeRepository == null)
                    componentTypeRepository = new BaseRepository<ComponentType>(context);
                return componentTypeRepository;
            }
        }

        public IRepository<Equipment> Equipments
        {
            get
            {
                if (equipmentRepository == null)
                    equipmentRepository = new BaseRepository<Equipment>(context);
                return equipmentRepository;
            }
        }

        public IRepository<EquipmentComponentRelation> EquipmentComponentRelations
        {
            get
            {
                if (equipmentComponentRelationsRepository == null)
                    equipmentComponentRelationsRepository = new BaseRepository<EquipmentComponentRelation>(context);
                return equipmentComponentRelationsRepository;
            }
        }

        public IRepository<EquipmentEmployeeRelation> EquipmentEmployeeRelations
        {
            get
            {
                if (equipmentEmployeeRelationsRepository == null)
                    equipmentEmployeeRelationsRepository = new BaseRepository<EquipmentEmployeeRelation>(context);
                return equipmentEmployeeRelationsRepository;
            }
        }

        public IRepository<EquipmentType> EquipmentTypes
        {
            get
            {
                if (equipmentTypeRepository == null)
                    equipmentTypeRepository = new BaseRepository<EquipmentType>(context);
                return equipmentTypeRepository;
            }
        }

        public IRepository<History> History
        {
            get
            {
                if (historyRepository == null)
                    historyRepository = new BaseRepository<History>(context);
                return historyRepository;
            }
        }

        public IRepository<RepairPlace> RepairPlaces
        {
            get
            {
                if (repairPlaceRepository == null)
                    repairPlaceRepository = new BaseRepository<RepairPlace>(context);
                return repairPlaceRepository;
            }
        }

        public IRepository<StatusType> StatusTypes
        {
            get
            {
                if (statusTypeRepository == null)
                    statusTypeRepository = new BaseRepository<StatusType>(context);
                return statusTypeRepository;
            }
        }

        public IPartialRepository<Employee> Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new PartialRepository<Employee>(context);
                return employeeRepository;
            }
        }

        public IPartialRepository<Department> Departments
        {
            get
            {
                if (departmentRepository == null)
                    departmentRepository = new PartialRepository<Department>(context);
                return departmentRepository;
            }
        }

        public IPartialRepository<Administration> Administrations
        {
            get
            {
                if (administrationRepository == null)
                    administrationRepository = new PartialRepository<Administration>(context);
                return administrationRepository;
            }
        }

        public IPartialRepository<Division> Divisions
        {
            get
            {
                if (divisionRepository == null)
                    divisionRepository = new PartialRepository<Division>(context);
                return divisionRepository;
            }
        }

        public IPartialRepository<Position> Positions
        {
            get
            {
                if (positionRepository == null)
                    positionRepository = new PartialRepository<Position>(context);
                return positionRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
