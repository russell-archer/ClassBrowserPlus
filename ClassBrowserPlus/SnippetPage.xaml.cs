using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using ClassBrowserPlus.Common;
using ClassBrowserPlus.DataModel;

namespace ClassBrowserPlus
{
    public sealed partial class SnippetPage : LayoutAwarePage
    {
        private ISnippetRepository _repository;
        private SnippetInfo _snippet;
        private string _snippetText;
        private bool _registeredforShare;

        public SnippetPage()
        {
            this.InitializeComponent();
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            _repository = new SnippetRepository();

            try
            {
                if (navigationParameter != null)
                {
                    var snippetFile = navigationParameter as string;
                    if (string.IsNullOrEmpty(snippetFile))
                        throw new ArgumentException();

                    // Lookup the full detail of the selected snippet:
                    _snippet = await _repository.GetSnippetInfoAsync(snippetFile);
                    if (_snippet == null)
                        throw new ArgumentException();

                    // Set the WebView's source to be the html version of the snippet
                    webView.Source = new Uri("ms-appx-web:///Snippets/" + _snippet.SnippetFile + ".html");

                    // Register for DataRequested events (Share contract)
                    DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
                    _registeredforShare = true;

                    // Read the plain-text version of the snippet...
                    await GetSnippetText();
                }
            }
            catch
            {
                webView.Source = new Uri("ms-appx-web:///Snippets/error.html");
            }
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            // Deregister the DataRequested event handler
            if (_registeredforShare)
                DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;

            base.SaveState(pageState);
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            if (_snippet.SnippetFile == null)
                return;

            try
            {
                // Copy the plain-text snippet to the clipboard
                var dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(_snippetText);
                Clipboard.SetContent(dataPackage);
            }
            catch
            {
                // Throw away the non-critical exception
            }
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // Share...
            var request = args.Request;

            request.Data.Properties.Title = _snippet.Name;
            request.Data.Properties.Description = _snippet.Summary;

            // Share the text of the selected class
            request.Data.SetText(_snippetText);
        }

        private async Task<string> GetSnippetText()
        {
            // Read the plain-text version of the snippet 
            if (string.IsNullOrEmpty(_snippetText))
                _snippetText = await _repository.GetSnippetTextAsync(_snippet.SnippetFile);

            return _snippetText;
        }
    }
}
