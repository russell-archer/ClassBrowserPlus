using ClassBrowserPlus.Common;

namespace ClassBrowserPlus.DataModel
{
    public class PmeInfo : BindableBase
    {
        private string _name = string.Empty;
        private string _summary = string.Empty;
        private string _returns = string.Empty;

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

        public string Returns
        {
            get { return _returns; }
            set { this.SetProperty(ref this._returns, value); }
        }

        public override string ToString()
        {
            return Summary + ". " + Returns ;
        }
    }

}