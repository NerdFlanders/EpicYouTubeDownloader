using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using EpicYouTubeDownloader.Models.Domain;
using EpicYouTubeDownloader.ViewModels.EventArguments;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadControlViewModel : Screen
    {
        #region Public Poperties

        public YTVideo Video
        {
            get { return _video; }
            set
            {
                if (Equals(value, _video)) return;
                _video = value;
                NotifyOfPropertyChange(() => Video);
            }
        }

        #endregion

        #region Private Properties

        private List<string> _links = new List<string>();
        private string _singleLink;
        private ValidateLinkService _validateLinkService = new ValidateLinkService();
        private PasteCoreService _pasteCoreService = new PasteCoreService();
        private YTVideo _video = new YTVideo();

        private readonly IEventAggregator _eventAggregator;
        

        #endregion

        #region Constructor

        public DownloadControlViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        public void Paste()
        {
            _singleLink = Clipboard.GetText();
            if (_validateLinkService.validateLink(_singleLink))
            {
                _links.Add(_singleLink);
                _pasteCoreService.getVideoData(_singleLink);
                _video = _pasteCoreService.video;

                Video = _video;

                _eventAggregator.Publish(new VideoAddedEventArgs(Video),
                    action => { Task.Factory.StartNew(action); });
            }
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
