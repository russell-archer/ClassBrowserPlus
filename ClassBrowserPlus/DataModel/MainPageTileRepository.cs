using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClassBrowserPlus.DataModel
{
    public class MainPageTileRepository : IMainPageTileRepository   
    {
        private static readonly Uri BaseUri = new Uri("ms-appx:///");
        private ObservableCollection<MainPageTile> _tiles;

        // MainPageTiles property is used by XAML data binding
        public ObservableCollection<MainPageTile> MainPageTiles
        {
            get
            {
                if (_tiles != null)
                    return _tiles;

                CreateTiles();
                return _tiles;
            }
        }

        public async Task<ObservableCollection<MainPageTile>> GetAllAsync()
        {
            if (_tiles != null)
                return _tiles;

            await Task.Run(() => CreateTiles());
            return _tiles;
        }

        private void CreateTiles()
        {
            _tiles = new ObservableCollection<MainPageTile>
            {
                new MainPageTile {Name = "Classes", Desc = "Browsable list of classes", Image = BaseUri + "Assets/Classes.png", Page = "ClassListPage"},
                new MainPageTile {Name = "Namespaces", Desc = "Classes organized by namespace", Image = BaseUri + "Assets/Namespaces.png", Page = "NsListPage"},
                new MainPageTile {Name = "Snippets", Desc = "Collection of snippets organized by functional area", Image = BaseUri + "Assets/Recipes.png", Page = "SnippetListPage"}
            };
        }
    }
}