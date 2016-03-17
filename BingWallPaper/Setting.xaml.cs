using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BingWallper.Helper;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BingWallPaper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setting : Page
    {
        private readonly ApplicationDataContainer _localSettings = null;

        public Setting()
        {
            this.InitializeComponent();
            _localSettings = ApplicationData.Current.LocalSettings;

            DisplaySetting();
        }

        private void DisplaySetting()
        {
            var folder = _localSettings.Values[Consts.StrFolder] as string;
            var isOn = _localSettings.Values[Consts.StrIsOn] as bool?;
            var resolution = _localSettings.Values[Consts.StrResolution] as int?;

            if (folder != null)
            {
                BtnFolder.Content = folder;
            }
            if (isOn != null)
            {
                SetWpSwitch.IsOn = isOn.Value;
            }
            if (resolution != null)
            {
                CbResolution.SelectedIndex = resolution.Value;
            }
        }

        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (SetWpSwitch.IsOn)
            {
                _localSettings.Values[Consts.StrIsOn] = true;

                //regist task
                var task = BackGroundTaskHelper.RegisterBackgroundTask("BingWallpaper.Core.BackGroundTask", "DownloadAndSetWallPaper",
                   new TimeTrigger(24*60, false), null);
                await task;
            }
            else
            {
                _localSettings.Values[Consts.StrIsOn] = false;
                BackGroundTaskHelper.UnregisterExistTask("BingWallpaper.Core.BackGroundTask");
            }
        }

        private async void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            // open file picker
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add(".jpg");
            folderPicker.FileTypeFilter.Add(".jpeg");
            folderPicker.FileTypeFilter.Add(".png");
            folderPicker.FileTypeFilter.Add(".raw");
            folderPicker.FileTypeFilter.Add(".exif");
            folderPicker.FileTypeFilter.Add(".gif");
            folderPicker.FileTypeFilter.Add(".bmp");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            //store folderName
            if (folder != null)
            {
                _localSettings.Values[Consts.StrFolder] = folder.Path;
                BtnFolder.Content = folder.Path;
            }
        }

        private void CbResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _localSettings.Values[Consts.StrResolution] = CbResolution.SelectedIndex;
        }

        private async void BtnSetNow_Click(object sender, RoutedEventArgs e)
        {
            var result = BackGroundTask.DownloadAndSetWallpaperAsync(true);

            pbSetting.IsActive = true;
            txtSetting.Visibility = Visibility.Visible;
            await result;
            pbSetting.IsActive = false;
            txtSetting.Visibility = Visibility.Collapsed;

            txtResult.Text = result.Result ? "设置成功" : "设置失败";
            txtResult.Visibility = Visibility.Visible;
        }
    }
}
