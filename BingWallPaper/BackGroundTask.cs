using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.UserProfile;

namespace BingWallPaper
{
    public sealed class BackGroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            //load localSetting
            var localSetting = ApplicationData.Current.LocalSettings;
            var folder = localSetting.Values[Consts.StrFolder] as string;
            var isOn = localSetting.Values[Consts.StrIsOn] as bool?;
            var resolution = localSetting.Values[Consts.StrResolution] as int?;

            if (!isOn.Value)
            {
                return;
            }

            //download
            var wallpaper = new WallPaper(Consts.DicResolution[resolution.Value], Consts.Region.ZhCn, folder);
            var filePath = await wallpaper.DownloadWallPaperAsync();

            //set wallpaper
            await SetWallpaperAsync(filePath);

            deferral.Complete();
        }

        // Pass in a relative path to a file inside the local appdata folder 
        public async Task<bool> SetWallpaperAsync(string path)
        {
            bool success = false;
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var uri = new Uri(path);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
                success = await profileSettings.TrySetWallpaperImageAsync(file);
            }

            return success;
        }
    }

}
