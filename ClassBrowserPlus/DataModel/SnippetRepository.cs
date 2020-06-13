using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Newtonsoft.Json;

namespace ClassBrowserPlus.DataModel
{
    public class SnippetRepository : ISnippetRepository
    {
        private ObservableCollection<SnippetInfo> _snippets;

        public async Task<ObservableCollection<SnippetInfo>> GetAllAsync()
        {
            if (_snippets != null)
                return _snippets;

            try
            {
                var file = await Package.Current.InstalledLocation.GetFileAsync("Data\\snippets.txt");
                var txt = await FileIO.ReadTextAsync(file);
                _snippets = await JsonConvert.DeserializeObjectAsync<ObservableCollection<SnippetInfo>>(txt);
            }
            catch
            {
                return null;
            }

            return _snippets;
        }

        public async Task<string> GetSnippetTextAsync(string snippetFile)
        {
            string snippetText;

            try
            {
                var file = await Package.Current.InstalledLocation.GetFileAsync("Snippets\\" + snippetFile + ".txt");
                snippetText = await FileIO.ReadTextAsync(file);
            }
            catch
            {
                return null;
            }

            return snippetText;
        }

        public async Task<SnippetInfo> GetSnippetInfoAsync(string snippetFile)
        {
            try
            {
                if (_snippets == null)
                    await GetAllAsync();

                if (_snippets == null)
                    return null;

                // Look in each group of snippets for the requested snippet
                foreach (var snippetInfoGroup in _snippets)
                {
                    var result = snippetInfoGroup.Snippets.Where(s => s.SnippetFile.Equals(snippetFile)).ToList();
                    if (result.Any())
                        return result[0];  // Return the SnippetInfo object
                }
            }
            catch
            {
            }

            return null;
        }
    }
}