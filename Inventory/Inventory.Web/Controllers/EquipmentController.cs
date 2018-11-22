﻿using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models;
using Inventory.Web.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class EquipmentController : Controller
    {
        private IEquipmentService EquipmentService;
        private IEquipmentTypeService EquipmentTypeService;
        private IEmployeeService EmployeeService;

        public EquipmentController
            (IEquipmentService equipmentService,
            IEmployeeService employeeService,
            IEquipmentTypeService equipmentTypeService)
        {
            EquipmentService = equipmentService;
            EmployeeService = employeeService;
            EquipmentTypeService = equipmentTypeService;
        }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxEquipmentList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService
             .GetAll()
             .ToList();
            IEnumerable<EquipmentVM> equipmentVMs = WebEquipmentMapper.DtoToVm(equipmentDTOs);

            return PartialView(equipmentVMs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<EquipmentDTO> equipmentDTOs = EquipmentService
                .GetAll()
                .ToList();
            IEnumerable<EquipmentVM> equipmentVMs = WebEquipmentMapper.DtoToVm(equipmentDTOs);

            return View(equipmentVMs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            IEnumerable<OwnerInfoDTO> ownerHistory = EquipmentService.GetOwnerHistory((Guid)id);
            ViewBag.OwnerHistory = ownerHistory.ToList();
            EquipmentVM equipmentVM = WebEquipmentMapper.DtoToVm(equipmentDTO);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Create()
        {
            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager, user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = WebEquipmentMapper.VmToDto(equipmentVM);
                Guid equipmentId = EquipmentService.AddAndGetId(equipmentDTO);

                return RedirectToAction("Index");
            }

            ViewBag.EquipmentTypeId = new SelectList(
                    EquipmentTypeService.GetAll(),
                    "Id",
                    "Name",
                    equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            EquipmentVM equipmentVM = WebEquipmentMapper.DtoToVm(equipmentDTO);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EquipmentTypeId,InventNumber,QRCode,Price,Supplier")] EquipmentVM equipmentVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentDTO equipmentDTO = WebEquipmentMapper
                    .VmToDto(equipmentVM);
                EquipmentService.Update(equipmentDTO);
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения.");

            EquipmentDTO dto = EquipmentService.Get(equipmentVM.Id);
            equipmentVM = WebEquipmentMapper.DtoToVm(dto);

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);

            return View(equipmentVM);
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Copy(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EquipmentDTO equipmentDTO = EquipmentService.Get((Guid)id);
            if (equipmentDTO == null)
                return HttpNotFound();

            EquipmentVM equipmentVM = WebEquipmentMapper.DtoToVm(equipmentDTO);
            equipmentVM.InventNumber = null;

            ViewBag.EquipmentTypeId = new SelectList(
                EquipmentTypeService.GetAll(),
                "Id",
                "Name",
                equipmentVM.EquipmentTypeId);

            return View("Create", equipmentVM);
        }

        public ActionResult OwnerHistory(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;
            IEnumerable<OwnerInfoDTO> ownerHistoryDTO = EquipmentService
                .GetOwnerHistory((Guid)equipmentId)
                .ToList();
            List<OwnerInfoVM> ownerHistoryVM = WebOwnerInfoMapper
                .DtoToVm(ownerHistoryDTO)
                .ToList();

            return View(ownerHistoryVM);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult OwnerInfo(Guid? equipmentId, int employeeId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;

            OwnerInfoDTO ownerInfoDTO = EquipmentService
                .GetOwnerInfo((Guid)equipmentId, employeeId);
            OwnerInfoVM ownerInfoVM = WebOwnerInfoMapper.DtoToVm(ownerInfoDTO);

            return View(ownerInfoVM);
        }

        [Authorize(Roles = "admin, manager, user")]
        public ActionResult Components(Guid? equipmentId)
        {
            if (equipmentId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.EquipmentId = equipmentId;

            IEnumerable<ComponentDTO> components = EquipmentService
                .GetComponents((Guid)equipmentId)
                .ToList();
            IEnumerable<ComponentVM> componentVMs = WebComponentMapper
                .DtoToVm(components);

            return View(componentVMs);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string imagePath = Request.MapPath($"/Content/Images/{id}.jpg");
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                EquipmentService.Delete(id);
            }
            catch (NotFoundException) { return HttpNotFound(); }
            catch (HasRelationsException) { return Content("Удаление невозможно."); }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult FindEmployees(string name)
        {
            const int MaxNumberOfWordsInFullName = 3;

            name = name.Trim();
            if (name.Length <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string[] nameParts = name.Split(' ');
            int wordsAmount;
            if (nameParts.Length < MaxNumberOfWordsInFullName)
                wordsAmount = nameParts.Length;
            else
                wordsAmount = MaxNumberOfWordsInFullName;

            IEnumerable<OwnerInfoDTO> employees = Enumerable.Empty<OwnerInfoDTO>();
            if (wordsAmount == 1)
                employees = EmployeeService.GetEmployeesByName(nameParts.First());
            else if (wordsAmount == 2)
                employees = EmployeeService.GetEmployeesByName(nameParts[0], nameParts[1]);
            else if (wordsAmount == 3)
                employees = EmployeeService.GetEmployeesByName(nameParts[0], nameParts[1], nameParts[2]);

            return PartialView(employees.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}