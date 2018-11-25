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
    public class RepairPlaceController : Controller
    {
        private const int ItemsPerPage = 10;
        private IRepairPlaceService RepairPlaceService;
        public RepairPlaceController(IRepairPlaceService repairPlaceService)
        {
            RepairPlaceService = repairPlaceService;
        }

        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxRepairPlaceList(int? page)
        {
            IEnumerable<RepairPlaceDTO> repairPlaceDTOs = RepairPlaceService
             .GetListOrderedByName()
             .ToList();
            IEnumerable<RepairPlaceVM> repairPlaceVMs = Mapper.Map<IEnumerable<RepairPlaceVM>>(repairPlaceDTOs);

            return PartialView(repairPlaceVMs.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30,  Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            IEnumerable<RepairPlaceDTO> repairPlaceDTOs = RepairPlaceService.GetListOrderedByName().ToList();
            IEnumerable<RepairPlaceVM> repairPlaceVMs = Mapper.Map<IEnumerable<RepairPlaceVM>>(repairPlaceDTOs);

            return View(repairPlaceVMs.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                RepairPlaceDTO repairPlaceDTO = RepairPlaceService.Get(id);
                RepairPlaceVM repairPlaceVM = Mapper.Map<RepairPlaceVM>(repairPlaceDTO);

                return View(repairPlaceVM);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] RepairPlaceVM repairPlaceVM)
        {
            if (ModelState.IsValid)
            {
                RepairPlaceDTO repairPlaceDTO = Mapper.Map<RepairPlaceDTO>(repairPlaceVM);
                RepairPlaceService.Add(repairPlaceDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                RepairPlaceDTO repairPlaceDTO = RepairPlaceService.Get(id);
                RepairPlaceVM repairPlaceVM = Mapper.Map<RepairPlaceVM>(repairPlaceDTO);

                return View(repairPlaceVM);
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
        public ActionResult Edit([Bind(Include = "Id,Name")] RepairPlaceVM repairPlaceVM)
        {
            if (ModelState.IsValid)
            {
                RepairPlaceDTO repairPlaceDTO = Mapper.Map<RepairPlaceDTO>(repairPlaceVM);
                RepairPlaceService.Update(repairPlaceDTO);

                return RedirectToAction("Index");
            }

            return View(repairPlaceVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                RepairPlaceService.Delete(id);
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