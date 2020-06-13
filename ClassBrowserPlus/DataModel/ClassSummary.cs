using ClassBrowserPlus.Common;

namespace ClassBrowserPlus.DataModel
{
    /// <summary>
    /// This class is used to hold summary infomation on a type. A collection of ClassSummary is used in
    /// ClassListPage to display a list of types.
    /// </summary>
    /// 

    public class ClassSummary : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref this._name, value); }
        }

        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set { this.SetProperty(ref this._summary, value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
