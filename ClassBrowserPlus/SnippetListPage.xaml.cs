using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using ClassBrowserPlus.Common;
using ClassBrowserPlus.DataModel;

namespace ClassBrowserPlus
{
    public sealed partial class SnippetListPage : LayoutAwarePage
    {
        private ISnippetRepository _repository;
        private SnippetInfo _selectedSnippet;
        private int _selectedIndex;

        public SnippetListPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Restore state
            if (pageState != null && pageState.ContainsKey("SelectedSnippet"))
            {
                try
                {
                    _selectedIndex = int.Parse(pageState["SelectedSnippet"].ToString());
                }
                catch
                {
                }
            }

            InitGrid();
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            // Save state
            pageState["SelectedSnippet"] = snippetListView.SelectedIndex;
            base.SaveState(pageState);
        }

        private async void InitGrid()
        {
            _repository = new SnippetRepository();
            var items = await _repository.GetAllAsync();
            if (items == null)
                return;

            snippetListView.ItemsSource = items;

            if (_selectedIndex != -1)
                snippetListView.SelectedIndex = _selectedIndex;
        }

        private void SnippetCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            _selectedSnippet = e.AddedItems[0] as SnippetInfo;
            if (_selectedSnippet == null)
                return;

            snippetGridView.ItemsSource = _selectedSnippet.Snippets;
        }

        private void SnippetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            _selectedSnippet = e.AddedItems[0] as SnippetInfo;
            if (_selectedSnippet == null)
                return;

            // Open the selected snippet in a web view
            // Note: At this point I'd like to pass a custom SnippetInfo object. However, if you try to pass no simple types (like ints, strings, etc.)
            // then when the app suspends you'll find the await SuspensionManager.SaveAsync(); statement fails in App.OnSuspending(). This is a known issue.
            // The only worksaround is to pass simple types. So, here we pass the (unique) snippet filename (this can be used to lookup additional info 
            // by the snippet page as required):
            Frame.Navigate(typeof(SnippetPage), _selectedSnippet.SnippetFile);
        }
    }
}

