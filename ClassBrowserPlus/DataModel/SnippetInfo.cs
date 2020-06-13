using System;
using System.Collections.ObjectModel;
using ClassBrowserPlus.Common;

namespace ClassBrowserPlus.DataModel
{
    public class SnippetInfo : BindableBase
    {
        private static readonly Uri BaseUri = new Uri("ms-appx:///");

        private string _name = string.Empty;
        private string _summary = string.Empty;
        private string _snippetFile = string.Empty;
        
        private ObservableCollection<SnippetInfo> _snippets;

        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref this._name, value); }
        }

        public string Summary
        {
            get { return _summary; }
            set { this.SetProperty(ref this._summary, value); }
        }

        public string SnippetFile
        {
            get { return _snippetFile; }
            set { this.SetProperty(ref this._snippetFile, value); }
        }

        public string Image
        {
            get { return BaseUri + "Assets/Recipes.png"; }
        }

        public ObservableCollection<SnippetInfo> Snippets
        {
            get { return _snippets; }
            set { this.SetProperty(ref this._snippets, value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }

}