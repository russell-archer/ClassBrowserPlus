using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public interface IMainPageTileRepository
    {
        Task<ObservableCollection<MainPageTile>> GetAllAsync();         
    }
}