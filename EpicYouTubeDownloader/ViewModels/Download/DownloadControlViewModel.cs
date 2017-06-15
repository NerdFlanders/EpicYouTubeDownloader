using System;
using System.Net;
using System.Windows;
using Caliburn.Micro;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadControlViewModel : Screen
    {
        #region Public Poperties

        #endregion

        #region Private Properties

        private string link;

        #endregion

        #region Constructor

        public DownloadControlViewModel()
        {
            
        }

        #endregion

        public void Paste()
        {
            link = Clipboard.GetText();
            //verify link and show error
        }

        public void Download()
        {
            if (verifyLink(link))
            {
                //add link to downloadlist view with thumbnail, name

                string[] links = new string[1];
                links[0] = link;
                DownloadCoreService s = new DownloadCoreService();
                s.DownloadMP3(links, @"D:\Musik\09-06-2017");
            }
        }

        private bool verifyLink(string link)
        {
            bool isVerified = false;

            if (link.StartsWith("https://www.youtube.com/watch?v="))
            {
                isVerified = true;
            }

            if (isVerified)
            {
                HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(link);
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
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
    }
}
