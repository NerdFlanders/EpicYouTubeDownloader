using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using System.Linq;
using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using EpicYouTubeDownloader.Models.Domain;
using Google.YouTube;
using VideoLibrary;
using Video = VideoLibrary.Video;

namespace EpicYouTubeDownloader
{
    public class DownloadCoreService
    {
        #region Private Properties

        private int songNumber;
        private int convertedSong;
        private string link;
        #endregion

        #region Public Properties


        #endregion

        #region Methods

        public ReturnError DownloadMP3(string[] links, string destPath)
        {
            songNumber = links.Length;
            if (links.Length == 0)
                throw new Exception("Il file txt è vuoto");
            if (!Directory.Exists(destPath))
                throw new DirectoryNotFoundException("Il path di destinazione non esiste");
            YouTube youtube = YouTube.Default;
            convertedSong = 0;
            ReturnError results = new ReturnError();
            results.errorNumber = 0;
            results.errorLinks = new List<string>();
            foreach (string link in links)
            {
                try
                {
                    //var video = youtube.GetVideo(link);

                    YouTubeVideo audio =
                        youtube.GetAllVideos("http://www.youtube.com/watch?v=eZUSxaFW8Lk")
                            .Where(e => e.AudioFormat == AudioFormat.Aac && e.AdaptiveKind == AdaptiveKind.Audio)
                            .ToList()
                            .FirstOrDefault();
                    
                    string filename =
                        Path.ChangeExtension(Path.Combine(destPath, Path.GetFileNameWithoutExtension(audio.FullName)),
                            "mp3");
                    filename = filename.Replace(" - YouTube", "");

                    MediaFile inputFile = new MediaFile {Filename = audio.GetUri()};
                    MediaFile outputFile = new MediaFile {Filename = filename};

                    using (Engine engine = new Engine())
                    {

                        engine.GetMetadata(inputFile);

                        engine.Convert(inputFile, outputFile);
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
            string[] playlistURLs = playlistID.Split(new string[] { "list=" }, StringSplitOptions.None);
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriBuilder.ToString());
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    ytResponse = new JavaScriptSerializer().Deserialize<YTResponse>(reader.ReadToEnd());
                }
                string textResponse = string.Join(Environment.NewLine, ytResponse.items.Select(x => x.contentDetails.videoId).ToArray());
                partLinks.Add(textResponse);
            }
            foreach (string response in partLinks)
            {
                tmpLinks = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                totalLinks = totalLinks.Concat(tmpLinks).ToList();
            }
            string[] youtubesongs = totalLinks.ToArray();
            for (int i = 0; i < youtubesongs.Length; i++)
            {
                youtubesongs[i] = "https://www.youtube.com/watch?v=" + youtubesongs[i];
            }
            errorRes = DownloadMP3(youtubesongs, destPath);
            return errorRes;
        }

        #endregion
    }
}
