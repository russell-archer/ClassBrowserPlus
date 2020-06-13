using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public interface ISnippetRepository
    {
        Task<ObservableCollection<SnippetInfo>> GetAllAsync();
        Task<string> GetSnippetTextAsync(string snippetFile);
        Task<SnippetInfo> GetSnippetInfoAsync(string snippetFile);
    }
}