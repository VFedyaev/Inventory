using Inventory.BLL.Interfaces;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class SearchService : ISearchService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public SearchService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
    }
}
