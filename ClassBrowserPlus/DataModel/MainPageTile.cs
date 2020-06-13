using ClassBrowserPlus.Common;

namespace ClassBrowserPlus.DataModel
{
    public class MainPageTile : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref this._name, value); }
        }

        private string _desc;
        public string Desc
        {
            get { return _desc; }
            set { this.SetProperty(ref this._desc, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { this.SetProperty(ref this._image, value); }
        }

        private string _page;
        public string Page
        {
            get { return _page; }
            set { this.SetProperty(ref this._page, value); }
        }
    }
}
