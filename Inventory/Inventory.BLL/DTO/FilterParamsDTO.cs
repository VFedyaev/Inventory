namespace Inventory.BLL.DTO
{
    public class FilterParamsDTO
    {
        public string ComponentTypeId { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }

        public string EquipmentId { get; set; }
        public string EmployeeId { get; set; }
        public string RepairPlaceId { get; set; }
        public string StatusTypeId { get; set; }
    }
}
