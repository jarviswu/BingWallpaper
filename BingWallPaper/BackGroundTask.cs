﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.UserProfile;
using BingWallpaper.Core;
using BingWallper.Helper;

namespace BingWallPaper
{
    public sealed class BackGroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Task Start");
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Debug.WriteLine("Task Start2");
            if (await DownloadAndSetWallpaperAsync(false)) return;
            Debug.WriteLine("Task Start3");
            deferral.Complete();
            Debug.WriteLine("Task End");
        }

        public static async Task<bool> DownloadAndSetWallpaperAsync(bool isManual)
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
            var filePath = await wallpaper.DownloadWallPaperAsync();

            //set wallpaper
            return await SetWallpaperAsync(filePath);
        }

        // Pass in a relative path to a file inside the local appdata folder 
        private static async Task<bool> SetWallpaperAsync(StorageFile file)
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
