using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using ClassBrowserPlus.Common;
using ClassBrowserPlus.DataModel;

namespace ClassBrowserPlus
{
    public sealed partial class MainPage : LayoutAwarePage
    {
        private IMainPageTileRepository _repository;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            _repository = new MainPageTileRepository();
            var tiles = await _repository.GetAllAsync();
            this.DefaultViewModel["MainPageTiles"] = tiles;
        }

        private void ItemClicked(object sender, ItemClickEventArgs e)
        {
            var tile = (MainPageTile)e.ClickedItem;
            if (tile == null)
                return;

            var t = Type.GetType("ClassBrowserPlus." + tile.Page);
            if (t == null)
                return;

            Frame.Navigate(t);
        }
    }
}
