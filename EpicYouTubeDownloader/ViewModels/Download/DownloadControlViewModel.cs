using System;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using MediaToolkit;
using MediaToolkit.Model;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadControlViewModel : Screen
    {
        #region Public Poperties

        #endregion

        #region Private Properties

        private string link;

        #endregion

        public DownloadControlViewModel()
        {
            
        }

        public void Paste()
        {
            link = Clipboard.GetText();
            if (verifyLink(link))
            {
                
            }
        }

        private bool verifyLink(string link)
        {
            bool isVerified = false;

            if (link.StartsWith("https://www.youtube.com/watch?v="))
            {
                isVerified = true;
            }
            if (link.StartsWith("https://www.youtube.com/watch?v="))
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
