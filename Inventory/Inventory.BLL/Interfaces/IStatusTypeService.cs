using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IStatusTypeService : IService<StatusTypeDTO>
    {
        IEnumerable<StatusTypeDTO> GetListOrderedByName();
        StatusTypeDTO Get(Guid? id);
    }
}
