

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Data.Json;
using Windows.Foundation;
using BingWallper.Helper;

namespace BingWallpaper.Core
{
    public sealed class WallPaper
    {
        //public string ImageUrl { get; set; }

        public string Resolution { get; set; }

        public string Region { get; set; }

        public string FilePath { get; set; }

        private string JsonUrl
        {
            get
            { 
                return $"{Consts.BingJsonUrl}?format=js&idx={0}&n={1}&mkt={Region}";
            }
        }
         
        public WallPaper(string resolution, string region, string filePath)
        {
            Region = region;
            Resolution = resolution;
            FilePath = filePath;
        }

        public IAsyncOperation<StorageFile> DownloadWallPaperAsync()
        {
            return DownloadWallPaperHelper().AsAsyncOperation();
        }

        private async Task<StorageFile> DownloadWallPaperHelper()
        {
            Uri bingUri;
            Uri.TryCreate(JsonUrl, UriKind.Absolute, out bingUri);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(bingUri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var wallpaperInfo = JsonObject.Parse(json);
            var urlBase = wallpaperInfo.GetNamedArray("images")[0].GetObject().GetNamedString("urlbase");
            var imageUrl = $"{Consts.BingBaseUrl}{urlBase}_{Resolution}.jpg";

            var fileName = GetFileNameFromUrl(imageUrl);
            //get folder
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.PicturesLibrary);
            var targetFolfer =
                await storageFolder.CreateFolderAsync("MyBingWallpaper", CreationCollisionOption.OpenIfExists);
            //check if already exist
            var wallpaperFile = await targetFolfer.TryGetItemAsync(fileName);
            if (wallpaperFile != null)
            {
                return (StorageFile)wallpaperFile;
            }

            Uri bingDownUri;
            Uri.TryCreate(imageUrl, UriKind.Absolute, out bingDownUri);
            var rsp = await httpClient.GetAsync(bingDownUri);
            var buffer = await rsp.Content.ReadAsBufferAsync();

            
            
            //create file
            var file = await targetFolfer.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            //write file
            await FileIO.WriteBufferAsync(file, buffer);

            return file;
        }

        private static string GetFileNameFromUrl(String url)
        {
            return System.IO.Path.GetFileName(url);
        }
    }
}
