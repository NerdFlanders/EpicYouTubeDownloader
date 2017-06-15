using System;
using System.Collections.Generic;
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

        private List<string> _links = new List<string>();
        private string _singleLink;
        private VerifyLinkService _verifyLinkService = new VerifyLinkService();
        private PasteCoreService _pasteCoreService = new PasteCoreService();

        #endregion

        #region Constructor

        public DownloadControlViewModel()
        {
            
        }

        #endregion

        public void Paste()
        {
            _singleLink = Clipboard.GetText();
            if (_verifyLinkService.verifyLink(_singleLink))
            {
                _links.Add(_singleLink);
                _pasteCoreService.getVideoData(_singleLink);
            }
                 
            
            //verify link and show error
        }

        public void Download()
        {
            //add link to downloadlist view with thumbnail, name
            DownloadCoreService s = new DownloadCoreService();
            if(_links.Count > 0)
                s.DownloadMP3(_links, @"D:\Musik\09-06-2017");
        }
    }
}
