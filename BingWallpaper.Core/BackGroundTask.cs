using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.UserProfile;
using BingWallper.Helper;


namespace BingWallpaper.Core
{
    public sealed class BackGroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            if (await DownloadAndSetWallpaperAsync(false)) return;   
            deferral.Complete();
        }

        public static IAsyncOperation<bool> DownloadAndSetWallpaperAsync(bool isManual)
        {
            return DownloadAndSetWallpaperHelper(isManual).AsAsyncOperation();
        }

        private static async Task<bool> DownloadAndSetWallpaperHelper(bool isManual)
        {
            //load localSetting
            var localSetting = ApplicationData.Current.LocalSettings;
            var folder = localSetting.Values[Consts.StrFolder] as string;
            var isOn = localSetting.Values[Consts.StrIsOn] as bool?;
            var resolution = localSetting.Values[Consts.StrResolution] as int?;

            if (!isManual && !isOn.Value)
            {
                return false;
            }

            //download
            var wallpaper = new WallPaper(Consts.DicResolution[resolution.Value], Consts.Region.ZhCn, folder);
            var file = await wallpaper.DownloadWallPaperAsync();

            //set wallpaper
            return await SetWallpaperAsync(file);
        }

        public static IAsyncOperation<bool> SetWallpaperAsync(StorageFile file)
        {
            return SetWallpaperHelper(file).AsAsyncOperation();
        }

        // Pass in a relative path to a file inside the local appdata folder 
        private static async Task<bool> SetWallpaperHelper(StorageFile file)
        {
            bool success = false;
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var newFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);
                UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
                success = await profileSettings.TrySetWallpaperImageAsync(newFile);
            }

            return success;
        }
    }

}
