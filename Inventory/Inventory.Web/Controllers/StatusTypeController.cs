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
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class StatusTypeController : Controller
    {
        private IStatusTypeService StatusTypeService;
        public StatusTypeController(IStatusTypeService statusTypeService)
        {
            StatusTypeService = statusTypeService;
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxStatusTypeList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService
                .GetAll()
                .ToList();
            IEnumerable<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper
                .DtoToVm(statusTypeDTOs);
            return PartialView(statusTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        // GET: StatusType
        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService
                .GetAll()
                .ToList();
            IEnumerable<StatusTypeVM> statusTypeVMs = WebStatusTypeMapper
                .DtoToVm(statusTypeDTOs);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(statusTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            StatusTypeDTO statusTypeDTO = StatusTypeService.Get((Guid)id);
            if (statusTypeDTO == null)
                return HttpNotFound();

            StatusTypeVM statusTypeVM = WebStatusTypeMapper
                .DtoToVm(statusTypeDTO);
            return View(statusTypeVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = WebStatusTypeMapper
                    .VmToDto(statusTypeVM);
                StatusTypeService
                    .Add(statusTypeDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            StatusTypeDTO statusTypeDTO = StatusTypeService.Get((Guid)id);
            if (statusTypeDTO == null)
                return HttpNotFound();

            StatusTypeVM statusTypeVM = WebStatusTypeMapper.DtoToVm(statusTypeDTO);
            return View(statusTypeVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = WebStatusTypeMapper.VmToDto(statusTypeVM);
                StatusTypeService.Update(statusTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                StatusTypeService.Delete(id);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (HasRelationsException)
            {
                return Content("Удаление невозможно.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}