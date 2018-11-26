using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class BaseController : Controller
    {
        public IEquipmentService EquipmentService;
        public IEquipmentTypeService EquipmentTypeService;
        public IEmployeeService EmployeeService;
        public IComponentService ComponentService;
        public IComponentTypeService ComponentTypeService;
        public IHistoryService HistoryService;
        public IStatusTypeService StatusTypeService;
        public IRepairPlaceService RepairPlaceService;

        public BaseController(
            IHistoryService historyService,
            IStatusTypeService statusTypeService,
            IRepairPlaceService repairPlaceService,
            IEquipmentService equipmentService,
            IEmployeeService employeeService)
        {
            HistoryService = historyService;
            StatusTypeService = statusTypeService;
            RepairPlaceService = repairPlaceService;
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
        }

        public BaseController(
            IEquipmentService equipmentService,
            IEquipmentTypeService equipmentTypeService,
            IEmployeeService employeeService)
        {
            EquipmentService = equipmentService;
            EquipmentTypeService = equipmentTypeService;
            EmployeeService = employeeService;
        }

        public BaseController(
            IComponentService componentService,
            IComponentTypeService componentTypeService,
            IEquipmentService equipmentService)
        {
            ComponentService = componentService;
            ComponentTypeService = componentTypeService;
            EquipmentService = equipmentService;
        }

        public BaseController(IEquipmentTypeService equipmentTypeService)
        {
            EquipmentTypeService = equipmentTypeService;
        }

        public BaseController(IComponentTypeService compTypeService)
        {
            ComponentTypeService = compTypeService;
        }

        public SelectList GetEquipmentTypeIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(EquipmentTypeService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        public SelectList GetComponentTypeIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(ComponentTypeService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        public SelectList GetModelNameSelectList(string selectedValue = null)
        {
            return new SelectList(ComponentService.GetAll().ToList(), "ModelName", "ModelName", selectedValue);
        }

        public SelectList GetComponentNameSelectList(string selectedValue = null)
        {
            return new SelectList(ComponentService.GetAll().ToList(), "Name", "Name");
        }

        public SelectList GetStatusTypeIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(StatusTypeService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        public SelectList GetRepairPlaceIdSelectList(Guid? selectedValue = null)
        {
            return new SelectList(RepairPlaceService.GetAll().ToList(), "Id", "Name", selectedValue);
        }

        public SelectList GetEquipmentIdSelectList(Guid? selectedValue = null)
        {
            var equipmentDTOList = EquipmentService.GetAll().ToList();
            var equipmentVMList = Mapper.Map<IEnumerable<EquipmentVM>>(equipmentDTOList).ToList();

            return new SelectList(equipmentVMList, "Id", "EquipmentTypeWithNumber", selectedValue);
        }

        public SelectList GetEmployeeIdSelectList(int? selectedValue = null)
        {
            return new SelectList(EmployeeService.GetAll().ToList(), "EmployeeId", "EmployeeFullName", selectedValue);
        }
    }
}