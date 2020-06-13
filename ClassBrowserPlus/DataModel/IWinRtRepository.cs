using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public interface IWinRtRepository
    {
        Task<ObservableCollection<ClassSummary>> GetAllAsync();
        Task<PmeInfo> GetPmeMetaData(string fullName, WinRtRepository.PMEType pmeType);
        Task<ObservableCollection<NsInfo>> GetAllNsAsync();
    }
}
