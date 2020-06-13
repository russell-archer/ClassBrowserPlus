using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public class DesignTimeSnippetRepository : ISnippetRepository
    {
        private ObservableCollection<SnippetInfo> _snippets;

        public ObservableCollection<SnippetInfo> Snippets
        {
            get
            {
                MockData();
                return _snippets;
            }
        }

        public async Task<ObservableCollection<SnippetInfo>> GetAllAsync()
        {
            if (_snippets != null)
                return _snippets;

            try
            {
                await Task.Run(() => MockData());
            }
            catch
            {
                return null;
            }

            return _snippets;
        }

        public async Task<string> GetSnippetTextAsync(string snippetFile)
        {
            string txt = string.Empty;
            await Task.Run(() => txt = "This is a snippet for " + snippetFile);
            return txt;
        }

        public Task<SnippetInfo> GetSnippetInfoAsync(string snippetFile)
        {
            throw new NotImplementedException();
        }

        private void MockData()
        {
            if (_snippets != null)
                return;

            _snippets = new ObservableCollection<SnippetInfo>
            {
                new SnippetInfo
                {
                    Name = "Storage", 
                    Summary = "Storage snippets",
                    SnippetFile = "",
                    Snippets = new ObservableCollection<SnippetInfo> 
                    {
                        new SnippetInfo
                        {
                            Name = "FileOpen picker",
                            Summary = "FileOpen picker summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },

                        new SnippetInfo
                        {
                            Name = "FileSave picker",
                            Summary = "FileSave picker summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },
                    }
                },

                new SnippetInfo
                {
                    Name = "UI", 
                    Summary = "UI snippets",
                    SnippetFile = "",
                    Snippets = new ObservableCollection<SnippetInfo> 
                    {
                        new SnippetInfo
                        {
                            Name = "UI snippet 01",
                            Summary = "UI snippet 01 summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },

                        new SnippetInfo
                        {
                            Name = "UI snippet 02",
                            Summary = "UI snippet 02 summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },
                    }
                },

                new SnippetInfo
                {
                    Name = "Async", 
                    Summary = "Async snippets",
                    SnippetFile = "",
                    Snippets = new ObservableCollection<SnippetInfo> 
                    {
                        new SnippetInfo
                        {
                            Name = "Async snippet 01",
                            Summary = "Async snippet 01 summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },

                        new SnippetInfo
                        {
                            Name = "Async snippet 02",
                            Summary = "Async snippet 02 summary goes here",
                            SnippetFile = "",
                            Snippets = null
                        },
                    }
                }
            };
        }
    }
}