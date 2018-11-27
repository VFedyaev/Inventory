using AutoMapper;
using Inventory.BLL.Services;
using Inventory.BLL.Tests.MappingProfiles;
using Inventory.BLL.Tests.MoqRepositories;
using Inventory.DAL.Interfaces;
using Moq;

namespace Inventory.BLL.Tests.Services.Tests
{
    public class BaseInit
    {
        public MoqComponentTypeRepository moqComponentTypeRepository;
        public MoqComponentRepository moqComponentRepository;
        public MoqEquipmentTypeRepository moqEquipmentTypeRepository;
        public MoqEquipmentRepository moqEquipmentRepository;
        public Mock<IUnitOfWork> moqUnitOfWork;

        public ComponentTypeService ComponentTypeService;
        public ComponentService ComponentService;

        public void ComponentAndComponentTypeInit()
        {
            ResetAndInitializeMapper();

            moqComponentTypeRepository = new MoqComponentTypeRepository();
            moqComponentRepository = new MoqComponentRepository(moqComponentTypeRepository.Types);

            moqEquipmentTypeRepository = new MoqEquipmentTypeRepository();
            moqEquipmentRepository = new MoqEquipmentRepository(moqEquipmentTypeRepository.Types);

            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork
                .Setup(u => u.ComponentTypes)
                .Returns(moqComponentTypeRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.Components)
                .Returns(moqComponentRepository.repository.Object);

            ComponentTypeService = new ComponentTypeService(moqUnitOfWork.Object);
            ComponentService = new ComponentService(moqUnitOfWork.Object);
        }

        public void ResetAndInitializeMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }
    }
}
