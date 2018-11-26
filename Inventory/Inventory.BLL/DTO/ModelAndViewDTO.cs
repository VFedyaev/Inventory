using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class ModelAndViewDTO
    {
        public IEnumerable<object> Model { get; set; }
        public string View { get; set; }
    }
}
