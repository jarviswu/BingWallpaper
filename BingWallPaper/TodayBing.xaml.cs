using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BingWallPaper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodayBing : Page
    {
        public TodayBing()
        {
            this.InitializeComponent();
        }

        private async void ImgTodayBing_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.PicturesLibrary);

            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            files = files.OrderByDescending(p => p.DateCreated).ToList();
            foreach (var file in files)
            {
                if (file.FileType == ".jpg")
                {

                    //set home page
                    ImgTodayBing.Source = new BitmapImage(new Uri(file.Path));

                    break;
                    //set flag
                }
            }
            //if false set default
        }
    }
}
