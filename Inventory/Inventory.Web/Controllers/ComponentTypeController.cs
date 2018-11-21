﻿using AutoMapper;
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
    public class ComponentTypeController : Controller
    {
        private IComponentTypeService ComponentTypeService;
        public ComponentTypeController(IComponentTypeService componentTypeService)
        {
            ComponentTypeService = componentTypeService;
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxComponentTypeList(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService
               .GetAll()
               .ToList();
            IEnumerable<ComponentTypeVM> componentTypeVMs = Mapper.Map<IEnumerable<ComponentTypeVM>>(componentTypeDTOs);

            return PartialView(componentTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<ComponentTypeDTO> componentTypeDTOs = ComponentTypeService
                .GetAll()
                .ToList();
            IEnumerable<ComponentTypeVM> componentTypeVMs = Mapper.Map<IEnumerable<ComponentTypeVM>>(componentTypeDTOs);

            return View(componentTypeVMs.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentTypeDTO componentTypeDTO = ComponentTypeService.Get((Guid)id);
            if (componentTypeDTO == null)
                return HttpNotFound();

            ComponentTypeVM componentTypeVM = Mapper.Map<ComponentTypeVM>(componentTypeDTO);

            return View(componentTypeVM);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ComponentTypeVM componentTypeVM)
        {
            if (ModelState.IsValid)
            {
                ComponentTypeDTO componentTypeDTO = Mapper.Map<ComponentTypeDTO>(componentTypeVM);
                ComponentTypeService.Add(componentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComponentTypeDTO componentTypeDTO = ComponentTypeService.Get((Guid)id);
            if (componentTypeDTO == null)
                return HttpNotFound();

            ComponentTypeVM componentTypeVM = Mapper.Map<ComponentTypeVM>(componentTypeDTO);

            return View(componentTypeVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ComponentTypeVM componentTypeVM)
        {
            if (ModelState.IsValid)
            {
                ComponentTypeDTO componentTypeDTO = Mapper.Map<ComponentTypeDTO>(componentTypeVM);
                ComponentTypeService.Update(componentTypeDTO);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try { ComponentTypeService.Delete(id); }
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