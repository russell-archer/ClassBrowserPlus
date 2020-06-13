using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using ClassBrowserPlus.Common;
using ClassBrowserPlus.DataModel;

namespace ClassBrowserPlus
{
    public sealed partial class NsListPage : LayoutAwarePage
    {
        private IWinRtRepository _repository;
        private NsInfo _selectedNs;
        private int _selectedIndex;

        public NsListPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Restore state
            if (pageState != null && pageState.ContainsKey("SelectedNamespace"))
            {
                try
                {
                    _selectedIndex = int.Parse(pageState["SelectedNamespace"].ToString());
                }
                catch
                {
                }
            }

            InitGrid();

            // Register for DataRequested events (Share contract)
            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            // Deregister the DataRequested event handler
            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;
            
            // Save state
            pageState["SelectedNamespace"] = nsListView.SelectedIndex;

            base.SaveState(pageState);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // Share...
            var request = args.Request;
            var item = (NsInfo)nsListView.SelectedItem;
            if(item != null)
            {
                request.Data.Properties.Title = item.Name;
                request.Data.Properties.Description = item.Summary;

                // Share the text of the selected namespace
                var selectedNs = "WinRT Namespace: " + item.ToString();
                request.Data.SetText(selectedNs);
            }
        }

        private async void InitGrid()
        {
            _repository = new WinRtRepository();
            var items = await _repository.GetAllNsAsync();
            if (items == null)
                return;

            nsListView.ItemsSource = items;

            if (_selectedIndex != -1)
                nsListView.SelectedIndex = _selectedIndex;
        }

        private void NamespaceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            _selectedNs = e.AddedItems[0] as NsInfo;
            if (_selectedNs == null)
                return;

            nsGridView.ItemsSource = _selectedNs.ChildNamespaces;
        }

        private void ChildNamespaceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            _selectedNs = e.AddedItems[0] as NsInfo;
            if (_selectedNs == null)
                return;

            Frame.Navigate(typeof(ClassListPage), _selectedNs.Name);
        }

    }
}
