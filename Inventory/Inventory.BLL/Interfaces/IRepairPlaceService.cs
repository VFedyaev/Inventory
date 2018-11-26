using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IRepairPlaceService : IService<RepairPlaceDTO>
    {
        IEnumerable<RepairPlaceDTO> GetListOrderedByName();
        RepairPlaceDTO Get(Guid? id);
    }
}
