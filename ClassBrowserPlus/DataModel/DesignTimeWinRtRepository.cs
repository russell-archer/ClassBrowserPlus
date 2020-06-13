using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public class DesignTimeWinRtRepository : IWinRtRepository
    {
        private ObservableCollection<ClassSummary> _classSummaryList;
        private ObservableCollection<NsInfo> _nsList;

        public ObservableCollection<ClassSummary> ClassSummaryList
        {
            get
            {
                MockData();
                return _classSummaryList;
            }
        }

        public ObservableCollection<NsInfo> NsList
        {
            get 
            { 
                MockNsData();
                return _nsList;
            }
        }

        private void MockData()
        {
            if (_classSummaryList != null)
                return;

            _classSummaryList = new ObservableCollection<ClassSummary>
            {
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.DesignMode",
                    Summary = "Enables you to detect whether your app is in design mode in a visual designer."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.Package",
                    Summary = "Provides information about an app package."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.PackageId",
                    Summary = "Provides package identification info, such as name, version, and publisher."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.SuspendingDeferral",
                    Summary = "Manages a delayed app suspending operation."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.SuspendingEventArgs",
                    Summary = "Provides data for an app suspending event."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.SuspendingOperation",
                    Summary = "Provides info about an app suspending operation."
                },
                new ClassSummary
                {
                    Name = "Windows.ApplicationModel.PackageVersion",
                    Summary = "Represents the package version info."
                }
            };
        }

        private void MockNsData()
        {
            if (_nsList != null)
                return;

            _nsList = new ObservableCollection<NsInfo>
            {
                new NsInfo
                {
                    Name = "ABC", 
                    ParentName = "",
                    Summary = "ABC namespace",
                    ChildNamespaces = new ObservableCollection<NsInfo> 
                    {
                        new NsInfo
                        {
                            Name = "ABC.DEF",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "ABC.DEF namespace",
                            ChildNamespaces = null
                        },

                        new NsInfo
                        {
                            Name = "ABC.GHI",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "ABC.GHI namespace",
                            ChildNamespaces = null
                        },
                    }
                },

                new NsInfo
                {
                    Name = "DEF", 
                    ParentName = "",
                    Summary = "DEF namespace",
                    ChildNamespaces = new ObservableCollection<NsInfo> 
                    {
                        new NsInfo
                        {
                            Name = "DEF.ABC",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "DEF.ABC namespace",
                            ChildNamespaces = null
                        },

                        new NsInfo
                        {
                            Name = "DEF.DEF",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "DEF.DEF namespace",
                            ChildNamespaces = null
                        },
                    }
                },

                new NsInfo
                {
                    Name = "GHI", 
                    ParentName = "",
                    Summary = "GHI namespace",
                    ChildNamespaces = new ObservableCollection<NsInfo> 
                    {
                        new NsInfo
                        {
                            Name = "GHI.ABC",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "GHI.ABC namespace",
                            ChildNamespaces = null
                        },

                        new NsInfo
                        {
                            Name = "GHI.DEF",
                            ParentName = "Windows.ApplicationModel",
                            Summary = "GHI.DEF namespace",
                            ChildNamespaces = null
                        },
                    }
                }
            };
        }

        public async Task<ObservableCollection<ClassSummary>> GetAllAsync()
        {
            if (_classSummaryList != null)
                return _classSummaryList;

            try
            {
                await Task.Run(() => MockData());
            }
            catch
            {
                return null;
            }

            return _classSummaryList;
        }

        public async Task<PmeInfo> GetPmeMetaData(string fullName, WinRtRepository.PMEType pmeType)
        {
            return await Task.Run(() => 
                new PmeInfo{Name = "MockMethod", Returns = "Mock return text", Summary = "This is mock data for the method"});
        }

        public async Task<ObservableCollection<NsInfo>> GetAllNsAsync()
        {
            if (_nsList != null)
                return _nsList;

            try
            {
                await Task.Run(() => MockNsData());
            }
            catch
            {
                return null;
            }

            return _nsList;
        }
    }
}