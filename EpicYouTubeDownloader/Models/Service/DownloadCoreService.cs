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
    public class DownloadCoreService
    {
        #region Private Properties

        private int _songCount;
        private int convertedSong;
        private string link;

        private const string YoutubeLinkRegex =
            "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";

        private static Regex regexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);
        private static string[] validAuthorities = {"youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be"};

        #endregion

        #region Public Properties


        #endregion

        #region Methods

        public ReturnError DownloadMP3(List<string> links, string destPath)
        {
            _songCount = links.Count;
            if (_songCount == 0)
                throw new Exception("No links available.");
            if (!Directory.Exists(destPath))
                throw new DirectoryNotFoundException("Destination path does not exist.");

            YouTube youtube = YouTube.Default;
            convertedSong = 0;
            ReturnError results = new ReturnError();
            results.errorNumber = 0;
            results.errorLinks = new List<string>();
            foreach (string link in links)
            {
                try
                {
                    YouTubeVideo audio =
                        youtube.GetAllVideos(link)
                            .Where(e => e.AudioFormat == AudioFormat.Aac && e.AdaptiveKind == AdaptiveKind.Audio)
                            .ToList()
                            .FirstOrDefault();

                    string filename =
                        Path.ChangeExtension(Path.Combine(destPath, Path.GetFileNameWithoutExtension(audio.FullName)),
                            "mp3");
                    filename = filename.Replace(" - YouTube", "");

                    MediaFile inputFile = new MediaFile {Filename = audio.GetUri()};
                    MediaFile outputFile = new MediaFile {Filename = filename};

                    getThumbnail(link);

                    using (Engine engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);
                        engine.Convert(inputFile, outputFile);
                        engine.ConvertProgressEvent += engine_ConvertProgressEvent;
                    }
                }
                catch (NullReferenceException e)
                {
                    results.errorNumber++;
                    results.errorLinks.Add(link);
                }
            }
            return results;
        }

        public ReturnError downloadPlaylist(string playlistID, string destPath)
        {
            List<string> partLinks = new List<string>();
            List<string> totalLinks = new List<string>();
            List<string> tmpLinks = new List<string>();
            string[] playlistURLs = playlistID.Split(new string[] {"list="}, StringSplitOptions.None);
            ReturnError errorRes = new ReturnError();

            YTResponse ytResponse = null;
            string url = @"https://www.googleapis.com/youtube/v3/playlistItems";
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["part"] = "contentDetails";
            query["maxResults"] = "50";
            query["playlistId"] = playlistURLs[1];
            query["key"] = "AIzaSyCtzP23AG4P_K5WNIjqb7AOFnhWNyLIkoE";
            while (ytResponse == null || !string.IsNullOrWhiteSpace(ytResponse.nextPageToken))
            {
                if (ytResponse != null && !string.IsNullOrWhiteSpace(ytResponse.nextPageToken))
                    query["pageToken"] = ytResponse.nextPageToken;
                uriBuilder.Query = query.ToString();
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uriBuilder.ToString());
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    ytResponse = new JavaScriptSerializer().Deserialize<YTResponse>(reader.ReadToEnd());
                }
                string textResponse = string.Join(Environment.NewLine,
                    ytResponse.items.Select(x => x.contentDetails.videoId).ToArray());
                partLinks.Add(textResponse);
            }
            foreach (string response in partLinks)
            {
                tmpLinks = response.Split(new string[] {Environment.NewLine}, StringSplitOptions.None).ToList();
                totalLinks = totalLinks.Concat(tmpLinks).ToList();
            }
            List<string> youtubesongs = totalLinks.ToList();
            for (int i = 0; i < youtubesongs.Count; i++)
            {
                youtubesongs[i] = "https://www.youtube.com/watch?v=" + youtubesongs[i];
            }
            errorRes = DownloadMP3(youtubesongs, destPath);
            return errorRes;
        }

        private void engine_ConvertProgressEvent(object sender, ConvertProgressEventArgs eventArgs)
        {
            //if ((convertedSong * 100 + (int)((eventArgs.ProcessedDuration.TotalSeconds / eventArgs.TotalDuration.TotalSeconds) * 100)) < )
            //{
            //    convertedSong++;
            //}
            int i  = eventArgs.TotalDuration.Seconds;
            int k = eventArgs.ProcessedDuration.Seconds;
        }


        private byte getThumbnail(string link)
        {
            //https://www.youtube.com/watch?v=P_SlAzsXa7E&list=RDdZX6Q-Bj_xg
            Uri uri = new Uri(link);
            string id = ExtractVideoIdFromUri(uri);

            byte pic = Byte.MaxValue;

            WebClient cli = new WebClient();

            var imgBytes = cli.DownloadData("http://img.youtube.com/vi/" + id + "/3.jpg");

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
            catch
            {
            }

            return null;
        }
        
        #endregion
    }
}
