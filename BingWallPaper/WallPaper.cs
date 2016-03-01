

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Data.Json;

namespace BingWallPaper
{
    public class WallPaper
    {
        //public string ImageUrl { get; set; }

        public string Resolution { get; set; }

        public string Region { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        private string JsonUrl
        {
            get
            { 
                return $"{Consts.BingJsonUrl}?format=js&idx={0}&n={1}&mkt={Region}";
            }
        }
         
        public WallPaper(string resolution, string region, string filePath, string fileName)
        {
            Region = region;
            Resolution = resolution;
            FileName = fileName;
            FilePath = filePath;
        }

        public async void DownloadWallPaperAsync()
        {
            Uri bingUri;
            Uri.TryCreate(JsonUrl, UriKind.Absolute, out bingUri);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(bingUri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();



            //var dateDir = DateTime.Now.ToString("yyyyMMdd") + "/";
            //if (!Directory.Exists(FilePath + dateDir))
            //{
            //    Directory.CreateDirectory(FilePath + dateDir);
            //}
            var wallpaperInfo = JsonObject.Parse(json);
            var urlBase = wallpaperInfo.GetNamedArray("images")[0].GetObject().GetNamedString("urlbase");
            var imageUrl = $"{Consts.BingBaseUrl}{urlBase}_{Resolution}.jpg";
            Uri bingUri2;
            Uri.TryCreate(imageUrl, UriKind.Absolute, out bingUri2);
            var rsp = await httpClient.GetAsync(bingUri2);
            var buffer = await rsp.Content.ReadAsBufferAsync();

            //get folder
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.PicturesLibrary);
            //create file
            var file = await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            //write file
            await FileIO.WriteBufferAsync(file, buffer);
        }


    }
}
