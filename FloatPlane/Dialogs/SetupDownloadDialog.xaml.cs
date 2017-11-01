using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;

namespace FloatPlane.Dialogs
{
    public sealed partial class SetupDownloadDialog : ContentDialog
    {

        public SetupDownloadDialog()
        {
            InitializeComponent();

            var helper = new LocalObjectStorageHelper();
            EnableBackgroundDownloading.IsOn = helper.Read(App.EnableDownload, false);


            Loaded += SetupDownloadDialog_Loaded;

        }

        private  async void SetupDownloadDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var folder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(App.SaveLocationFolder);
            if (folder != null)
            {
                DownloadPath.Text = "Download Path: " + folder.Path;

            }
        }

        private void CancelDialog(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private void SaveDialog(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var helper = new LocalObjectStorageHelper();
            helper.Save(App.EnableDownload, EnableBackgroundDownloading.IsOn);


        }

        private async void PickStorageLocation(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            var saveLocation = await picker.PickSingleFolderAsync();

            if (saveLocation != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(App.SaveLocationFolder, saveLocation);
                DownloadPath.Text = "Download Path: " + saveLocation.Path;
            }
        }
    }
}
