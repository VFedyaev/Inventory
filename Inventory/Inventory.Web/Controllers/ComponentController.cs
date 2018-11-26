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
    public class ComponentController : BaseController
    {
        private const int ItemsPerPage = 10;

        public ComponentController(
            IComponentService compService,
            IComponentTypeService compTypeService,
            IEquipmentService equipService) : base(compService, compTypeService, equipService) { }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxComponentList(int? page)
        {
            string componentTypeId = Request.QueryString["ComponentTypeId"];
            string modelName = Request.QueryString["ModelName"];
            string name = Request.QueryString["Name"];

            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(
                string.IsNullOrEmpty(componentTypeId) ? (Guid?)null : Guid.Parse(componentTypeId));
            ViewBag.ModelName = GetModelNameSelectList(modelName);
            ViewBag.Name = GetComponentNameSelectList(name);

            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = componentTypeId,
                ModelName = modelName,
                Name = name
            };

            IEnumerable<ComponentDTO> filteredComponentDTOList = ComponentService.GetFilteredList(parameters).ToList();
            IEnumerable<ComponentVM> filteredComponentVMList = Mapper.Map<IEnumerable<ComponentVM>>(filteredComponentDTOList);

            return View(filteredComponentVMList.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager, user")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            string componentTypeId = Request.QueryString["ComponentTypeId"];
            string modelName = Request.QueryString["ModelName"];
            string name = Request.QueryString["Name"];

            IEnumerable<ComponentDTO> componentDTOs = ComponentService
                .GetAll()
                .ToList();
            IEnumerable<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(componentDTOs);

            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(
                string.IsNullOrEmpty(componentTypeId) ? (Guid?)null : Guid.Parse(componentTypeId));
            ViewBag.ModelName = GetModelNameSelectList(modelName);
            ViewBag.Name = GetComponentNameSelectList(name);

            FilterParamsDTO parameters = new FilterParamsDTO
            {
                ComponentTypeId = componentTypeId,
                ModelName = modelName,
                Name = name
            };

            IEnumerable<ComponentDTO> filteredComponentDTOList = ComponentService.GetFilteredList(parameters).ToList();
            IEnumerable<ComponentVM> filteredComponentVMList = Mapper.Map<IEnumerable<ComponentVM>>(filteredComponentDTOList);

            return View(filteredComponentVMList.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                ComponentDTO componentDTO = ComponentService.Get((Guid)id);
                ComponentVM componentVM = Mapper.Map<ComponentVM>(componentDTO);

                return View(componentVM);
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
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager, user")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = Mapper.Map<ComponentDTO>(componentVM);
                ComponentService.Add(componentDTO);

                return RedirectToAction("Index");
            }
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList();

            return View(componentVM);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                ComponentDTO componentDTO = ComponentService.Get(id);
                ComponentVM componentVM = Mapper.Map<ComponentVM>(componentDTO);
                ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(componentVM.ComponentTypeId);

                return View(componentVM);
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
        public ActionResult Edit([Bind(Include = "Id,ComponentTypeId,ModelName,Name,Description,Price,InventNumber,Supplier")] ComponentVM componentVM)
        {
            if (ModelState.IsValid)
            {
                ComponentDTO componentDTO = Mapper.Map<ComponentDTO>(componentVM);
                ComponentService.Update(componentDTO);

                return RedirectToAction("Index");
            }
            
            ViewBag.ComponentTypeId = GetComponentTypeIdSelectList(componentVM.ComponentTypeId);

            return View(componentVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                ComponentService.Delete(id);
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult FindComponents(string value, string type)
        {
            value = value.Trim().ToLower();

            List<ComponentDTO> componentDTOs = ComponentService
                .GetComponentsBy(type, value)
                .ToList();

            List<ComponentVM> componentVMs = Mapper.Map<IEnumerable<ComponentVM>>(componentDTOs).ToList();

            return PartialView(componentVMs);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}