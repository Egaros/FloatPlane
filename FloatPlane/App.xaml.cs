/* 
 * Copyright (C) 2017 Dominic Maas
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FloatPlane.Models;
using FloatPlane.Sources;
using FloatPlane.Views;
using Microsoft.Toolkit.Uwp.Helpers;

namespace FloatPlane
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(WelcomeView), e.Arguments);
                }

                SystemNavigationManager.GetForCurrentView().BackRequested +=
                    App_BackRequested;

                rootFrame.Navigated += RootFrame_Navigated;

                // Ensure the current window is active
                Window.Current.Activate();
            }


            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Set active window colors
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.BackgroundColor = Color.FromArgb(255, 21, 21, 21);
            titleBar.ButtonBackgroundColor = Color.FromArgb(255, 21, 21, 21);

            // Background task runs every 60 minutes to look for fresh content
            var registered = BackgroundTaskHelper.Register("NewContentChecker", new TimeTrigger(60, false));
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            var deferral = args.TaskInstance.GetDeferral();

            var helper = new LocalObjectStorageHelper();

            if (args.TaskInstance.Task.Name == "NewContentChecker")
            {
                // Process is enabled
                if (helper.KeyExists(EnableDownload) && helper.Read(EnableDownload, false))
                {
                    var lastCheckTime = helper.Read(LastCheckTime, DateTime.UtcNow);
                    var source = new RecentVideoSource();

                    // Check that a folder has been picked
                    var folder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(SaveLocationFolder);

                    if (folder != null)
                    {
                        // Get latest videos
                        var videos = await source.GetPagedItemsAsync(0, 10);

                        foreach (var videoModel in videos)
                        {
                            if (videoModel.Created <= lastCheckTime)
                                continue;

                            System.Diagnostics.Debug.WriteLine("NEW VIDEO: " + videoModel.Title);

                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Folder is null");

                    }
                }
            }

            // Save the key
            helper.Save(LastCheckTime, DateTime.UtcNow);

            deferral.Complete();
        }

        public static string LastCheckTime = "LastCheckTime";
        public static string EnableDownload = "EnableDownload";
        public static string SaveLocationFolder = "SaveLocation";
    }
}
