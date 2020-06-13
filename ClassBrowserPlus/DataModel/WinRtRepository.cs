using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Newtonsoft.Json;

namespace ClassBrowserPlus.DataModel
{
    public class WinRtRepository : IWinRtRepository
    {
        private XmlDocument _windowsXmlMetaData;
        private ObservableCollection<ClassSummary> _classSummaryList;
        private ObservableCollection<NsInfo> _nsList;

        public enum PMEType
        {
            Property,
            Method,
            Event
        };

        /// <summary>
        /// Returns a collection of ClassSummary objects that summarizes each class, interface, delegate and enum in the WinRT namespace
        /// </summary>
        /// <returns>Returns a collection of ClassSummary objects</returns>
        public async Task<ObservableCollection<ClassSummary>> GetAllAsync()
        {
            if (_classSummaryList != null) 
                return _classSummaryList;

            try
            {
                // For reasons of performance, here we use a pre-written subset of the Windows.xml file
                // that contains only class info
                var file = await Package.Current.InstalledLocation.GetFileAsync("Data\\classes.txt");
                var txt = await FileIO.ReadTextAsync(file);
                _classSummaryList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<ClassSummary>>(txt);
            }
            catch
            {
                return null;
            }

            return _classSummaryList;
        }

        /// <summary>
        /// Get Property, Method and Event (PME) data for the specified class
        /// </summary>
        /// <param name="fullName">The full namespace-qualified class name for which PME data is required</param>
        /// <param name="pmeType">The type of PME data return (Property, Method, or Event)</param>
        /// <returns>Returns PME metadata for the specified class</returns>
        public async Task<PmeInfo> GetPmeMetaData(string fullName, PMEType pmeType)
        {
            try
            {
                var doc = await ReadWindowsXml("Data");
                var xml = doc.GetXml();
                var xdoc = XDocument.Parse(xml); // Create an XDocument which we can use with LINQ-to-XML

                string searchText;
                if (pmeType == PMEType.Property)
                    searchText = "P:";
                else if (pmeType == PMEType.Method)
                    searchText = "M:";
                else
                    searchText = "E:";

                var pmeMetaData =
                    from member in xdoc.Descendants("member")
                    where member.Attribute("name").Value.StartsWith(searchText + fullName)
                    select new PmeInfo
                    {
                        Name = member.Attribute("name").Value.Substring(2),
                        // Chop off the "P:", "M:" or "E:"
                        Summary = member.Element("summary") == null ? "" : member.Element("summary").Value,
                        Returns = member.Element("returns") == null ? "" : "Returns: " + member.Element("returns").Value
                    };

                return pmeMetaData.ToList()[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<NsInfo>> GetAllNsAsync()
        {
            if (_nsList != null)
                return _nsList;

            try
            {
                // For reasons of performance, here we use a pre-written subset of the Windows.xml file
                // that contains only namespace info
                var file = await Package.Current.InstalledLocation.GetFileAsync("Data\\namespaces.txt");
                var txt = await FileIO.ReadTextAsync(file);
                _nsList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<NsInfo>>(txt);
            }
            catch
            {
                return null;
            }

            return _nsList;
        }


        /// <summary>
        /// Reads the Windows.xml file that contains metadata on each class, interface, delegate and enum in the WinRT namespace 
        /// </summary>
        /// <param name="folder">The path (which is relative to the installed location of the app) to the folder containing the file</param>
        /// <param name="file">The name of file to read. Defaults to "Windows.xml"</param>
        /// <returns></returns>
        private async Task<XmlDocument> ReadWindowsXml(string folder, string file = "Windows.xml")
        {
            if (_windowsXmlMetaData != null)
                return _windowsXmlMetaData;

            var storageFolder = await Package.Current.InstalledLocation.GetFolderAsync(folder);
            var storageFile = await storageFolder.GetFileAsync(file);

            _windowsXmlMetaData = await XmlDocument.LoadFromFileAsync(storageFile);
            return _windowsXmlMetaData;
        }
    }
}
