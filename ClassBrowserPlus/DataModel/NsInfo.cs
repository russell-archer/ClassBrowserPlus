using System;
using System.Collections.ObjectModel;
using ClassBrowserPlus.Common;

namespace ClassBrowserPlus.DataModel
{
    public class NsInfo : BindableBase
    {
        private static readonly Uri BaseUri = new Uri("ms-appx:///");

        private string _name = string.Empty;
        private string _parentName = string.Empty;
        private string _summary = string.Empty;
        private ObservableCollection<NsInfo> _childNamespaces;

        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref this._name, value); }
        }

        public string ParentName
        {
            get { return _parentName; }
            set { this.SetProperty(ref this._parentName, value); }
        }

        public string Summary
        {
            get { return _summary; }
            set { this.SetProperty(ref this._summary, value); }
        }

        public string Image
        {
            get {  return BaseUri + "Assets/" + _parentName + ".png"; }      
        }

        public ObservableCollection<NsInfo> ChildNamespaces
        {
            get { return _childNamespaces; }
            set { this.SetProperty(ref this._childNamespaces, value); }
        }

        public override string ToString()
        {
            var childCount = _childNamespaces == null ? 0 : _childNamespaces.Count;
            return Name + " (" + childCount.ToString() + " child namespaces). " + Summary;
        }
    }

}