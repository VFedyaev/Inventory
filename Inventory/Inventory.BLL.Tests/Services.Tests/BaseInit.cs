using AutoMapper;
using Inventory.BLL.Services;
using Inventory.BLL.Tests.MappingProfiles;
using Inventory.BLL.Tests.MoqRepositories;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using CatalogEntities;

namespace Inventory.BLL.Tests.Services.Tests
{
    public class BaseInit
    {
        /* Moq Repositories */
        public MoqBaseRepository<ComponentType> moqComponentTypeRepository;
        public MoqBaseRepository<Component> moqComponentRepository;
        public MoqBaseRepository<EquipmentType> moqEquipmentTypeRepository;
        public MoqBaseRepository<Equipment> moqEquipmentRepository;

        public MoqBaseRepository<EquipmentComponentRelation> moqEquipCompRelRepository;

        public MoqPartialRepository<Division> moqDivisionRepository;
        public MoqPartialRepository<Administration> moqAdministrationRepository;
        public MoqPartialRepository<Department> moqDepartmentRepository;
        public MoqPartialRepository<Position> moqPositionRepository;
        public MoqPartialRepository<Employee> moqEmployeeRepository;

        /* Moq UnitOfWork */
        public Mock<IUnitOfWork> moqUnitOfWork;

        /* Services */
        public ComponentTypeService ComponentTypeService;
        public ComponentService ComponentService;

        public EquipmentTypeService EquipmentTypeService;
        public EquipmentService EquipmentService;
        public EquipmentComponentRelationService EquipCompRelService;

        public EmployeeService EmployeeService;

        public void ComponentInitialize()
        {
            SetupMapper();
            SetupComponentMoqRepositories();
            SetupComponenMoqUnitOfWork();
            SetupComponenServices();
        }

        public void PartialInitialize()
        {
            SetupMapper();
            SetupPartialMoqRepositories();
            SetupPartialMoqUnitOfWork();
            SetupPartialServices();
        }

        public void SetupMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        public void SetupComponentMoqRepositories()
        {
            moqComponentTypeRepository = new MoqBaseRepository<ComponentType>(TestData.ComponentTypes);
            moqComponentRepository = new MoqBaseRepository<Component>(TestData.Components);

            moqEquipCompRelRepository = new MoqBaseRepository<EquipmentComponentRelation>(TestData.EquipmentComponentRelations);
        }

        public void SetupComponenMoqUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            /* component */
            moqUnitOfWork
                .Setup(u => u.ComponentTypes)
                .Returns(moqComponentTypeRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Components)
                .Returns(moqComponentRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.EquipmentComponentRelations)
                .Returns(moqEquipCompRelRepository.repository.Object);
        }

        public void SetupComponenServices()
        {
            ComponentTypeService = new ComponentTypeService(moqUnitOfWork.Object);
            ComponentService = new ComponentService(moqUnitOfWork.Object);
            EquipCompRelService = new EquipmentComponentRelationService(moqUnitOfWork.Object);
        }

        public void SetupPartialMoqRepositories()
        {
            moqDivisionRepository = new MoqPartialRepository<Division>(TestData.Divisions);
            moqAdministrationRepository = new MoqPartialRepository<Administration>(TestData.Administrations);
            moqDepartmentRepository = new MoqPartialRepository<Department>(TestData.Departments);
            moqPositionRepository = new MoqPartialRepository<Position>(TestData.Positions);
            moqEmployeeRepository = new MoqPartialRepository<Employee>(TestData.Employees);
        }

        public void SetupPartialMoqUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork
                .Setup(u => u.Divisions)
                .Returns(moqDivisionRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Administrations)
                .Returns(moqAdministrationRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Departments)
                .Returns(moqDepartmentRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Positions)
                .Returns(moqPositionRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Employees)
                .Returns(moqEmployeeRepository.repository.Object);
        }

        public void SetupPartialServices()
        {
            EmployeeService = new EmployeeService(moqUnitOfWork.Object);
        }
    }
}
