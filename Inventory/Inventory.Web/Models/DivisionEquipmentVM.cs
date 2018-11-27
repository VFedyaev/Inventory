using System;
using System.Collections.Generic;

namespace Inventory.Web.Models
{
    public class DivisionEquipmentVM
    {
        public int Divisionid { get; set; }
        public string DivisionName { get; set; }
        public IEnumerable<AdministrationEquipmentVM> Administrations { get; set; }
    }

    public class AdministrationEquipmentVM
    {
        public int AdministrationId { get; set; }
        public string AdministrationName { get; set; }
        public IEnumerable<DepartmentEquipmentVM> Departments { get; set; }
    }

    public class DepartmentEquipmentVM
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public IEnumerable<StructuredEquipmentVM> Equipments { get; set; }
    }

    public class StructuredEquipmentVM
    {
        public Guid Id { get; set; }
        public string EquipmentType { get; set; }
        public string InventNumber { get; set; }
        public string Supplier { get; set; }
        public IEnumerable<ComponentVM> Components { get; set; }
        public IEnumerable<OwnerInfoVM> Owners { get; set; }
    }
}