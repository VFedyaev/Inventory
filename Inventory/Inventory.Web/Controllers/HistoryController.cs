using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class HistoryController : BaseController
    {
        private const int ItemsPerPage = 10;
        public HistoryController(
            IHistoryService historyService,
            IStatusTypeService statusTypeService,
            IRepairPlaceService repairPlaceService,
            IEquipmentService equipmentService,
            IEmployeeService employeeService) : base(
            historyService, statusTypeService, repairPlaceService, equipmentService, employeeService) { }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page, string equipmentId, string employeeId, string repairPlaceId, string statusTypeId)
        {
            ViewBag.StatusTypeId = GetStatusTypeIdSelectList();
            ViewBag.RepairPlaceId = GetRepairPlaceIdSelectList();
            ViewBag.EquipmentId = GetEquipmentIdSelectList();
            ViewBag.EmployeeId = GetEmployeeIdSelectList();

            IEnumerable<HistoryDTO> historyDTOs = HistoryService.GetAll().ToList();

            IEnumerable<HistoryVM> historyVMs = Mapper.Map<IEnumerable<HistoryVM>>(historyDTOs);

            FilterParamsDTO parameters = new FilterParamsDTO
            {
                EquipmentId = Request.QueryString["equipmentId"],
                EmployeeId = Request.QueryString["emploeyeId"],
                RepairPlaceId = Request.QueryString["repairPlaceId"],
                StatusTypeId = Request.QueryString["statusTypeId"]
            };

            var filteredHistoryDTOList = HistoryService.GetFilteredList(parameters).ToList();
            var filteredHistoryVMList = Mapper.Map<IEnumerable<HistoryVM>>(filteredHistoryDTOList);

            return View(filteredHistoryVMList.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                HistoryDTO historyDTO = HistoryService.Get(id);
                HistoryVM historyVM = Mapper.Map<HistoryVM>(historyDTO);

                return View(historyVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Create()
        {
            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.StatusTypeId = GetStatusTypeIdSelectList();
            ViewBag.RepairPlaceId = GetRepairPlaceIdSelectList();
            ViewBag.EquipmentId = GetEquipmentIdSelectList();
            ViewBag.EmployeeId = GetEmployeeIdSelectList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager, user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentId,ChangeDate,EmployeeId,RepairPlaceId,StatusTypeId,Comments")] HistoryVM model)
        {
            if (ModelState.IsValid)
            {
                HistoryDTO historyDTO = Mapper.Map<HistoryDTO>(model);
                HistoryService.Add(historyDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.StatusTypeId = GetStatusTypeIdSelectList(model.StatusTypeId);
            ViewBag.RepairPlaceId = GetRepairPlaceIdSelectList(model.RepairPlaceId);
            ViewBag.EquipmentId = GetEquipmentIdSelectList(model.EquipmentId);
            ViewBag.EmployeeId = GetEmployeeIdSelectList(model.EmployeeId);

            return View(model);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                HistoryDTO historyDTO = HistoryService.Get(id);
                HistoryVM historyVM = Mapper.Map<HistoryVM>(historyDTO);

                ViewBag.ChangeDateNow = ((DateTime)historyVM.ChangeDate).ToString("dd.MM.yyyy HH:mm:ss");
                ViewBag.StatusTypeId = GetStatusTypeIdSelectList(historyVM.StatusTypeId);
                ViewBag.RepairPlaceId = GetRepairPlaceIdSelectList(historyVM.RepairPlaceId);
                ViewBag.EquipmentId = GetEquipmentIdSelectList(historyVM.EquipmentId);
                ViewBag.EmployeeId = GetEmployeeIdSelectList(historyVM.EmployeeId);

                return View(historyVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentId,ChangeDate,EmployeeId,RepairPlaceId,StatusTypeId,Comments")] HistoryVM historyVM)
        {
            if (ModelState.IsValid)
            {
                HistoryDTO historyDTO = Mapper.Map<HistoryDTO>(historyVM);
                HistoryService.Update(historyDTO);

                return RedirectToAction("Index");
            }

            ViewBag.ChangeDateNow = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.StatusTypeId = GetStatusTypeIdSelectList(historyVM.StatusTypeId);
            ViewBag.RepairPlaceId = GetRepairPlaceIdSelectList(historyVM.RepairPlaceId);
            ViewBag.EquipmentId = GetEquipmentIdSelectList(historyVM.EquipmentId);
            ViewBag.EmployeeId = GetEmployeeIdSelectList(historyVM.EmployeeId);

            return View(historyVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                HistoryService.Delete(id);
            }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}