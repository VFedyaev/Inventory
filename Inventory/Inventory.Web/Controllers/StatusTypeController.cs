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
    public class StatusTypeController : Controller
    {
        private const int ItemsPerPage = 10;

        private IStatusTypeService StatusTypeService;
        public StatusTypeController(IStatusTypeService statusTypeService)
        {
            StatusTypeService = statusTypeService;
        }

        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult AjaxStatusTypeList(int? page)
        {
            IEnumerable<StatusTypeDTO> statusTypeDTOs = StatusTypeService.GetListOrderedByName().ToList();
            IEnumerable<StatusTypeVM> statusTypeVMs = Mapper.Map<IEnumerable<StatusTypeVM>>(statusTypeDTOs);

            return PartialView(statusTypeVMs.ToPagedList(page ?? 1, ItemsPerPage));
        }

        // GET: StatusType
        [Authorize(Roles = "admin, manager")]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            List<StatusTypeDTO> statusTypeDTOs = StatusTypeService.GetListOrderedByName().ToList();
            IEnumerable<StatusTypeVM> statusTypeVMs = Mapper.Map<IEnumerable<StatusTypeVM>>(statusTypeDTOs);

            return View(statusTypeVMs.ToPagedList(page ?? 1, ItemsPerPage));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Details(Guid? id)
        {
            try
            {
                StatusTypeDTO statusTypeDTO = StatusTypeService.Get(id);
                StatusTypeVM statusTypeVM = Mapper.Map<StatusTypeVM>(statusTypeDTO);

                return View(statusTypeVM);
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

        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = Mapper.Map<StatusTypeDTO>(statusTypeVM);
                StatusTypeService.Add(statusTypeDTO);

                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                StatusTypeDTO statusTypeDTO = StatusTypeService.Get(id);
                StatusTypeVM statusTypeVM = Mapper.Map<StatusTypeVM>(statusTypeDTO);

                return View(statusTypeVM);
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
        public ActionResult Edit([Bind(Include = "Id,Name")] StatusTypeVM statusTypeVM)
        {
            if (ModelState.IsValid)
            {
                StatusTypeDTO statusTypeDTO = Mapper.Map<StatusTypeDTO>(statusTypeVM);
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