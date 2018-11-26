using Inventory.BLL.DTO;

namespace Inventory.BLL.Interfaces
{
    public interface ISearchService
    {
        ModelAndViewDTO GetFilteredModelAndView(string input, string type);
    }
}
