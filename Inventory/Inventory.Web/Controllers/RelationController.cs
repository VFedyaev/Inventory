using IEqEmpRelServ = Inventory.BLL.Interfaces.IEquipmentEmployeeRelationService;
using IEqComRelServ = Inventory.BLL.Interfaces.IEquipmentComponentRelationService;
using System.Web.Mvc;
using System;
using System.Net;
using Inventory.Web.Models;
using Inventory.BLL.DTO;
using AutoMapper;
using Inventory.BLL.Infrastructure;

namespace Inventory.Web.Controllers
{
    public class RelationController : Controller
    {
        IEqEmpRelServ EqEmpService;
        IEqComRelServ EqComService;

        public RelationController(IEqEmpRelServ eqEmpService, IEqComRelServ eqComService)
        {
            EqEmpService = eqEmpService;
            EqComService = eqComService;
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOwnerHistory(Guid? equipmentId)
        {
            try
            {
                string[] employeeIds = Request.Form.GetValues("employeeId[]") ?? new string[0];
                EqEmpService.UpdateEquipmentRelations(equipmentId, employeeIds, Request.Form["ownerId"]);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                EqEmpService.DeleteRelationsByEquipmentId((Guid)equipmentId);
            }

            return RedirectToRoute(new
            {
                controller = "Equipment",
                action = "OwnerHistory",
                equipmentId
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateComponents(Guid? equipmentId)
        {
            string[] componentIds = Request.Form.GetValues("componentId[]") ?? new string[0];
            try
            {
                EqComService.UpdateEquipmentRelations(equipmentId, componentIds);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                EqComService.DeleteRelationsByEquipmentId((Guid)equipmentId);
            }

            return RedirectToRoute(new
            {
                controller = "Equipment",
                action = "Components",
                equipmentId
            });
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult EditEquipmentEmployeeRelation()
        {
            Guid? equipmentId = Guid.Parse(Request.QueryString["equipmentId"]);
            int? employeeId = int.Parse(Request.QueryString["employeeId"]);

            try
            {
                EquipmentEmployeeRelationDTO relationDTO = EqEmpService.GetByEquipmentAndEmployee(equipmentId, employeeId);
                EquipmentEmployeeRelationVM relationVM = Mapper.Map<EquipmentEmployeeRelationVM>(relationDTO);

                return View(relationVM);
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
        public ActionResult EditEquipmentEmployeeRelation([Bind(Include = "Id,EquipmentId,CreatedAt,UpdatedAt")] EquipmentEmployeeRelationVM relationVM)
        {
            if (ModelState.IsValid)
            {
                EquipmentEmployeeRelationDTO relationDTO = Mapper.Map<EquipmentEmployeeRelationDTO>(relationVM);
                EqEmpService.UpdateDates(relationDTO);
                TempData["success"] = "Изменения сохранены.";
            }
            else
                ModelState.AddModelError(null, "Что-то пошло не так. Не удалось сохранить изменения");

            return View(relationVM);
        }
    }
}