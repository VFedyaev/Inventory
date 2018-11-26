using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.BLL.Services
{
    public class SearchService : ISearchService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public SearchService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public ModelAndViewDTO GetFilteredModelAndView(string inputTitle, string type)
        {
            ModelAndViewDTO result = new ModelAndViewDTO
            {
                Model = Enumerable.Empty<object>(),
                View = "NotFound"
            };
            string title = inputTitle.Trim();
            if (title.Length <= 0)
                return result;

            string[] words = title.ToLower().Split(' ');

            switch(type)
            {
                case "equipment":
                    result = GetEquipmentFilteredListAndView(words);
                    break;
                case "equipmentType":
                    result = GetEquipmentTypeFilteredListAndView(words);
                    break;
                case "component":
                    result = GetComponentFilteredListAndView(words);
                    break;
                case "componentType":
                    result = GetComponentTypeFilteredListAndVew(words);
                    break;
                case "statusType":
                    result = GetStatusTypeFilteredListAndView(words);
                    break;
                case "repairPlace":
                    result = GetRepairPlaceFilteredListAndView(words);
                    break;
            }

            return result;
        }

        private ModelAndViewDTO GetEquipmentFilteredListAndView(string[] words)
        {
            var equipmentList = _unitOfWork.Equipments.GetAll().Where(e => words.All(e.InventNumber.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<EquipmentDTO>>(equipmentList),
                View = "Equipments"
            };
        }

        private ModelAndViewDTO GetEquipmentTypeFilteredListAndView(string[] words)
        {
            var equipmentTypeList = _unitOfWork.EquipmentTypes.GetAll().Where(t => words.All(t.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<EquipmentTypeDTO>>(equipmentTypeList),
                View = "EquipmentTypes"
            };
        }

        private ModelAndViewDTO GetComponentFilteredListAndView(string[] words)
        {
            var componentList = _unitOfWork.Components.GetAll().Where(c => c.InventNumber != null && words.All(c.InventNumber.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<ComponentDTO>>(componentList),
                View = "Components"
            };
        }

        private ModelAndViewDTO GetComponentTypeFilteredListAndVew(string[] words)
        {
            var componentTypeList = _unitOfWork.ComponentTypes.GetAll().Where(t => words.All(t.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<ComponentTypeDTO>>(componentTypeList),
                View = "ComponentTypes"
            };
        }

        private ModelAndViewDTO GetStatusTypeFilteredListAndView(string[] words)
        {
            var statusTypeList = _unitOfWork.StatusTypes.GetAll().Where(st => words.All(st.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<StatusTypeDTO>>(statusTypeList),
                View = "StatusTypes"
            };
        }

        private ModelAndViewDTO GetRepairPlaceFilteredListAndView(string[] words)
        {
            var repairPlaceList = _unitOfWork.RepairPlaces.GetAll().Where(rp => words.All(rp.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<RepairPlaceDTO>>(repairPlaceList),
                View = "RepairPlaces"
            };
        }
    }
}
