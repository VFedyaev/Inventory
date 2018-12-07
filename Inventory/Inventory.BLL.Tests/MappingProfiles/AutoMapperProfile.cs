using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;

namespace Inventory.BLL.Tests.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ComponentType, ComponentTypeDTO>(MemberList.None).ReverseMap();
            CreateMap<Component, ComponentDTO>(MemberList.None).ReverseMap();
        }
    }
}
