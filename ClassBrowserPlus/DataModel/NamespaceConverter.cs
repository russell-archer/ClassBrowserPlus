using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace ClassBrowserPlus.DataModel
{
    public sealed class NamespaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.ToString().Length == 0)
                return "Assets/noNs.png";

            var ns = value.ToString();
            var imagePath = new StringBuilder("Assets/");

            if (ns.StartsWith("Windows.ApplicationModel"))
                imagePath.Append("Windows.ApplicationModelSm.png");
            else if (ns.StartsWith("Windows.Data"))
                imagePath.Append("Windows.DataSm.png");
            else if (ns.StartsWith("Windows.Devices"))
                imagePath.Append("Windows.DevicesSm.png");
            else if (ns.StartsWith("Windows.Foundation"))
                imagePath.Append("Windows.FoundationSm.png");
            else if (ns.StartsWith("Windows.Globalization"))
                imagePath.Append("Windows.GlobalizationSm.png");
            else if (ns.StartsWith("Windows.Graphics"))
                imagePath.Append("Windows.GraphicsSm.png");
            else if (ns.StartsWith("Windows.Management"))
                imagePath.Append("Windows.ManagementSm.png");
            else if (ns.StartsWith("Windows.Media"))
                imagePath.Append("Windows.MediaSm.png");
            else if (ns.StartsWith("Windows.Networking"))
                imagePath.Append("Windows.NetworkingSm.png");
            else if (ns.StartsWith("Windows.Security"))
                imagePath.Append("Windows.SecuritySm.png");
            else if (ns.StartsWith("Windows.Storage"))
                imagePath.Append("Windows.StorageSm.png");
            else if (ns.StartsWith("Windows.System"))
                imagePath.Append("Windows.SystemSm.png");
            else if (ns.StartsWith("Windows.UI.Xaml"))
                imagePath.Append("Windows.UI.XamlSm.png");
            else if (ns.StartsWith("Windows.UI"))
                imagePath.Append("Windows.UISm.png");
            else if (ns.StartsWith("Windows.Web"))
                imagePath.Append("Windows.WebSm.png");

            return imagePath.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
