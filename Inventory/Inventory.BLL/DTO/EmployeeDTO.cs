namespace Inventory.BLL.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Room { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }

        public PositionDTO Position { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}
