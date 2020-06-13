using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Callisto.Controls;
using ClassBrowserPlus.Common;

namespace ClassBrowserPlus
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();

            // Register handler for CommandsRequested events from the settings charm (for the About box)
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        protected async override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.NotRunning ||
                args.PreviousExecutionState == ApplicationExecutionState.ClosedByUser ||
                args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                var rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                Window.Current.Content = rootFrame;
            }

            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame == null)
            {
                frame = new Frame();
                SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }
            }

            frame.Navigate(typeof(ClassListPage), args.QueryText);
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add an About command using a Callisto SettingsFlyout
            var about = new SettingsCommand("about", "About", handler => new SettingsFlyout
            {
                Content = new AboutUserControl(),
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),  
                HeaderBrush = new SolidColorBrush(Color.FromArgb(255, 239, 182, 45)),
                HeaderText = "About", 
                SmallLogoImageSource = new BitmapImage { UriSource = new Uri("ms-appx:///Assets/SmallLogo.png") },
                IsOpen = true
            });

            args.Request.ApplicationCommands.Add(about);
        }
    }
}
