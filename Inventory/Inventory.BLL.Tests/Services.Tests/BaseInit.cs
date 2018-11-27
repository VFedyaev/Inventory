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
        public Mock<IUnitOfWork> moqUnitOfWork;

        public ComponentTypeService ComponentTypeService;
        public ComponentService ComponentService;

        public void Init()
        {
            ResetAndInitializeMapper();
            SetupMoqRepositories();
            SetupMoqUnitOfWork();
        }

        public void ResetAndInitializeMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        private void SetupMoqRepositories()
        {
            moqComponentTypeRepository = new MoqComponentTypeRepository();
            moqComponentRepository = new MoqComponentRepository(moqComponentTypeRepository.ComponentTypes);
        }

        private void SetupMoqUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();

            moqUnitOfWork
                .Setup(u => u.Components)
                .Returns(moqComponentRepository.repository.Object);
            moqUnitOfWork
                .Setup(u => u.ComponentTypes)
                .Returns(moqComponentTypeRepository.repository.Object);
        }

        private void SetupServices()
        {
            ComponentTypeService = new ComponentTypeService(moqUnitOfWork.Object);
            ComponentService = new ComponentService(moqUnitOfWork.Object);
        }
    }
}
