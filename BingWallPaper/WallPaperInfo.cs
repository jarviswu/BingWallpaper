using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingWallPaper
{
    public class WallPaperInfo
    {
        public class ImageItem
        {
            public class HsItem
            {
                [DataMember(Name = "desc")]
                public string Description;
                [DataMember(Name = "link")]
                public string Link;
                [DataMember(Name = "query")]
                public string Query;
                [DataMember(Name = "locx")]
                public string LocX;
                [DataMember(Name = "locy")]
                public string LocY;
            }

            public class MsgItem
            {
                [DataMember(Name = "title")]
                public string Title;
                [DataMember(Name = "link")]
                public string Link;
                [DataMember(Name = "text")]
                public string Text;
            }

            [DataMember(Name = "startdate")]
            public string StartDate;
            [DataMember(Name = "fullstartdate")]
            public string FullStartDate;
            [DataMember(Name = "enddate")]
            public string EndDate;
            [DataMember(Name = "url")]
            public string Url;
            [DataMember(Name = "urlbase")]
            public string UrlBase;
            [DataMember(Name = "copyright")]
            public string CopyRight;
            [DataMember(Name = "copyrightlink")]
            public string CopyRightLink;
            [DataMember(Name = "wp")]
            public Boolean Wp;
            [DataMember(Name = "hsh")]
            public string Hash;
            [DataMember(Name = "drk")]
            public int Drk;
            [DataMember(Name = "top")]
            public int Top;
            [DataMember(Name = "bot")]
            public int Bot;
            [DataMember(Name = "hs")]
            public HsItem[] Hs;
            [DataMember(Name = "msg")]
            public MsgItem[] Msg;
        }

        public class ToolTip
        {
            [DataMember(Name = "loading")]
            public string Loading;
            [DataMember(Name = "previous")]
            public string Previous;
            [DataMember(Name = "next")]
            public string Next;
            [DataMember(Name = "walle")]
            public string Walle;
            [DataMember(Name = "walls")]
            public string Walls;
        }

        [DataMember(Name = "images")]
        public ImageItem[] Image;
        [DataMember(Name = "tooltips")]
        public ToolTip Tooltip;
    }

}
