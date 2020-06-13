using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ClassBrowserPlus.Common;
using ClassBrowserPlus.DataModel;

namespace ClassBrowserPlus
{
    public sealed partial class ClassListPage : LayoutAwarePage
    {
        private IWinRtRepository _repository;
        private ClassSummary _selectedClassSummary;
        private ClassInfo _selectedClassInfo;
        private PmeInfo _selectedPmeInfo;

        public ClassListPage()
        {
            this.InitializeComponent();
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            // Deregister the DataRequested event handler
            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;

            base.SaveState(pageState);
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (navigationParameter != null)
            {
                // Page was opened to do a search (Search Charm)
                tbSearch.Text = navigationParameter as string;
                _repository = new WinRtRepository();
                btnSearchtapped(null, null);
            }
            else
            {
                // Page opened by normal user navigation
                InitGrid();
                
                // Register for DataRequested events (Share Charm)
                DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
            }

            InitPmeCtrls();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // Share...
            var request = args.Request;
            var item = (ClassSummary)classSummaryListView.SelectedItem;
            if(item == null) return;

            request.Data.Properties.Title = item.Name;
            request.Data.Properties.Description = item.Summary;

            // Share the text of the selected class
            var selectedClass = "WinRT Class: " + item.Name + ", Summary:" + item.Summary;
            request.Data.SetText(selectedClass);
        }

        private async void InitGrid()
        {
            _repository = new WinRtRepository();
            var items = await _repository.GetAllAsync();
            if (items == null)
                return;

            classSummaryListView.ItemsSource = items;
        }

        private void InitPmeCtrls()
        {
            classInfoUserControl.lbMethods.SelectionChanged += async (sender, args) =>
            {
                if (args.AddedItems.Count < 1 || args.AddedItems[0] == null) 
                    return;

                await DoGetPmeInfo(WinRtRepository.PMEType.Method, args.AddedItems[0].ToString());
                classInfoUserControl.lbEvents.SelectedIndex = -1;
                classInfoUserControl.lbProperties.SelectedIndex = -1;
            };

            classInfoUserControl.lbProperties.SelectionChanged += async (sender, args) =>
            {
                if (args.AddedItems.Count < 1 || args.AddedItems[0] == null) 
                    return;

                await DoGetPmeInfo(WinRtRepository.PMEType.Property, args.AddedItems[0].ToString());
                classInfoUserControl.lbEvents.SelectedIndex = -1;
                classInfoUserControl.lbMethods.SelectedIndex = -1;
            };

            classInfoUserControl.lbEvents.SelectionChanged += async (sender, args) =>
            {
                if (args.AddedItems.Count < 1 || args.AddedItems[0] == null) 
                    return;

                await DoGetPmeInfo(WinRtRepository.PMEType.Event, args.AddedItems[0].ToString());
                classInfoUserControl.lbMethods.SelectedIndex = -1;
                classInfoUserControl.lbProperties.SelectedIndex = -1;
            };            
        }

        private async Task DoGetPmeInfo(WinRtRepository.PMEType pmeType, string selectedPmeItemName)
        {
            try
            {
                var pmeName = _selectedClassInfo.ClassName + "." + selectedPmeItemName;
                _selectedPmeInfo = await _repository.GetPmeMetaData(pmeName, pmeType);
                classInfoUserControl.spPmeData.DataContext = _selectedPmeInfo ?? new PmeInfo { Name = "", Returns = "", Summary = "" };
            }
            catch
            {
                classInfoUserControl.spPmeData.DataContext = new PmeInfo { Name = "", Returns = "", Summary = "" };
            }
        }

        private void ClassSummaryListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            _selectedClassSummary = e.AddedItems[0] as ClassSummary;
            if (_selectedClassSummary == null)
                return;

            // Use reflection to get full class info...
            var assemblyQualifiedName = _selectedClassSummary.Name +
                ", Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime";

            _selectedClassInfo = new ClassInfo(_selectedClassSummary.Name, assemblyQualifiedName, _selectedClassSummary.Summary);
            classInfoUserControl.DataContext = _selectedClassInfo;
            classInfoUserControl.spPmeData.DataContext = new PmeInfo { Name = "", Returns = "", Summary = "" };
        }

        private async void btnSearchtapped(object sender, TappedRoutedEventArgs e)
        {
            if (tbSearch.Text.Length == 0)
                return;

            var items = await _repository.GetAllAsync();
            if (items == null)
                return;

            var searchResults = from results in items
                                where results.Name.ToLower().Contains(tbSearch.Text.ToLower())
                                select results;

            var count = searchResults.Count();
            if (count == 0)
            {
                var dlg = new MessageDialog("No matching classes found");
                await dlg.ShowAsync();
            }
            else
            {
                classSummaryListView.ItemsSource = searchResults;
                tbSearchResultsCount.Text = "Showing " + count.ToString() + " matches";
            }
        }

        private void btnClearSearchtapped(object sender, TappedRoutedEventArgs e)
        {
            InitGrid();
            tbSearch.Text = "";
            tbSearchResultsCount.Text = "";
        }

        private void tbSearch_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                btnSearchtapped(tbSearch, null);
                e.Handled = true;
            }
        }

        private void AppBarLoaded(object sender, RoutedEventArgs e)
        {
            tbSearch.Focus(FocusState.Programmatic);
        }
    }
}
