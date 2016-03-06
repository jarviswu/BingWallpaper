using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallPaper
{
    public class Consts
    {
        public static class Region
        {
            public const string ZhCn = "zh-CN";
            public const string EnUs = "en-US";
            public const string JaJp = "ja-JP";
            public const string EnAu = "en-AU";
            public const string EnUk = "en-UK";
            public const string DeDe = "de-DE";
            public const string EnNz = "en-NZ";
            public const string EnCa = "en-CA";
        }

        public static class Resolution
        {
            public const string HdLandScape = "1366x768";
            public const string HdPortrait = "768x1366";
            public const string FhdLandscape = "1920x1080";
            public const string FhdPortrait = "1080x1920";
        }

        public static readonly Dictionary<int, string> DicResolution = new Dictionary<int, string>
        {
            {1, "1366x768"},
            {2, "768x1366"},
            {3, "1920x1080"},
            {4, "1080x1920"}
        };

        public const string BingBaseUrl = "http://www.bing.com";
        public const string BingJsonUrl = "http://www.bing.com/HPImageArchive.aspx";

        public static readonly string StrFolder = "Folder";
        public static readonly string StrIsOn = "isOn";
        public static readonly string StrResolution = "resolution";

    }
}
