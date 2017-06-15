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
            PasteCoreService _pasteCoreService = new PasteCoreService(link);

            _pasteCoreService.verifyLink();
            //verify link and show error
        }

        public void Download()
        {
            //add link to downloadlist view with thumbnail, name
            string[] links = new string[1];
            links[0] = link;
            DownloadCoreService s = new DownloadCoreService();
            s.DownloadMP3(links, @"D:\Musik\09-06-2017");
        }
    }
}
