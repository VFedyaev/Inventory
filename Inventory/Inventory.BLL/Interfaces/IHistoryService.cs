using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IHistoryService : IService<HistoryDTO>
    {
        HistoryDTO Get(Guid? id);
        IEnumerable<HistoryDTO> GetFilteredList(FilterParamsDTO parameters);
    }
}
