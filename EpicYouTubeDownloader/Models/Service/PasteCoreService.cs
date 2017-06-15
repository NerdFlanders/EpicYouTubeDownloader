using System;
using System.IO;
using System.Net;
using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using System.Linq;
using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using EpicYouTubeDownloader.Models.Domain;
using VideoLibrary;

namespace EpicYouTubeDownloader
{
    public class PasteCoreService
    {
        #region private Properties

        private const string YoutubeLinkRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
        private static Regex regexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);
        private static string[] validAuthorities = { "youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be" };
        private string _link;

        #endregion

        #region Constructor

        public PasteCoreService(string link)
        {
            _link = link;
        }

        #endregion

        #region Methods

        public void doStuff()
        {
            if (verifyLink())
            {
                getThumbnail();
            }
        }

        private byte getThumbnail()
        {
            //https://www.youtube.com/watch?v=P_SlAzsXa7E&list=RDdZX6Q-Bj_xg
            Uri uri = new Uri(_link);
            string id = ExtractVideoIdFromUri(uri);

            byte pic = Byte.MaxValue;

            WebClient cli = new WebClient();

            var imgBytes = cli.DownloadData("http://img.youtube.com/vi/" + id + "/3.jpg");

            File.WriteAllBytes(@"D:\Musik\09-06-2017\thumb.jpg", imgBytes);

            return pic;
        }

        public string ExtractVideoIdFromUri(Uri uri)
        {
            try
            {
                string authority = new UriBuilder(uri).Uri.Authority.ToLower();

                //check if the url is a youtube url
                if (validAuthorities.Contains(authority))
                {
                    //and extract the id
                    var regRes = regexExtractId.Match(uri.ToString());
                    if (regRes.Success)
                    {
                        return regRes.Groups[1].Value;
                    }
                }
            }
            catch { }
            
            return null;
        }

        public bool verifyLink()
        {
            bool isVerified = false;

            if (_link.StartsWith("https://www.youtube.com/watch?v="))
            {
                isVerified = true;
            }

            if (isVerified)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_link);
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        isVerified = true;
                    }
                }
                catch (Exception exception)
                {
                    isVerified = false;
                }
            }
            return isVerified;
        }

        #endregion
    }
}