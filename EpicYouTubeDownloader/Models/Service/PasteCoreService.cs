using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using EpicYouTubeDownloader.Models.Domain;
using MediaToolkit;
using MediaToolkit.Model;
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
        private ValidateLinkService _validateLinkService;

        #endregion

        #region Public Properties

        public BitmapImage bmp = new BitmapImage();
        public YTVideo video = new YTVideo();

        #endregion

        #region Constructor



        #endregion

        #region Methods

        public void getVideoData(string link)
        {
            _link = link;
            getThumbnail();
            getMetaData();
        }

        private byte getThumbnail()
        {
            //https://www.youtube.com/watch?v=P_SlAzsXa7E&list=RDdZX6Q-Bj_xg
            Uri uri = new Uri(_link);
            string id = ExtractVideoIdFromUri(uri);

            byte pic = Byte.MaxValue;

            WebClient cli = new WebClient();

            var imgBytes = cli.DownloadData("http://img.youtube.com/vi/" + id + "/3.jpg");

            using (var ms = new MemoryStream(imgBytes))
            {
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();

                video.Thumbnail = bmp;
            }
            //File.WriteAllBytes(@"D:\Musik\09-06-2017\thumb.jpg", imgBytes);
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

        private void getMetaData()
        {
            YouTube youtube = YouTube.Default;
            ReturnError results = new ReturnError();

            try
            {
                YouTubeVideo audio =
                    youtube.GetAllVideos(_link)
                        .Where(e => e.AudioFormat == AudioFormat.Aac && e.AdaptiveKind == AdaptiveKind.Audio)
                        .ToList()
                        .FirstOrDefault();

                string filename = audio.FullName.Replace(" - YouTube", "");
                
                MediaFile inputFile = new MediaFile { Filename = audio.GetUri() };
                
                using (Engine engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    video.VideoName = filename;

                    if (inputFile.Metadata.Duration.ToString().StartsWith("00"))
                    {
                        video.Length = inputFile.Metadata.Duration.ToString().Substring(3, 5);
                    }
                    else
                    {
                        video.Length = inputFile.Metadata.Duration.ToString().Substring(0, 8);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                results.errorNumber++;
                results.errorLinks.Add(_link);
            }
        }

        #endregion
    }
}