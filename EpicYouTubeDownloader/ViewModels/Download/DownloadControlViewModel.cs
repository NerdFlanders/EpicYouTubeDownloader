using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using EpicYouTubeDownloader.Models.Domain;
using EpicYouTubeDownloader.Properties;
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
        private bool _isValidated;
        private string _destination;

        private readonly IEventAggregator _eventAggregator;
        

        #endregion

        #region Constructor

        public DownloadControlViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region Public Methods

        public void Paste()
        {
            _singleLink = Clipboard.GetText();

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += ValidateLink;
            backgroundWorker.RunWorkerCompleted += OnValidatedLink;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerAsync();
        }

        public void Download()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += DownloadVideo;
            backgroundWorker.RunWorkerCompleted += OnDownloadedVideo;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerAsync();
        }

        #endregion

        #region Private Methods

        private void ValidateLink(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _validateLinkService.validateLink(_singleLink);
        }

        private void OnValidatedLink(object sender, RunWorkerCompletedEventArgs eventargs)
        {
            _isValidated = _validateLinkService.Validated;
            if (_isValidated)
            {
                _links.Add(_singleLink);
                _pasteCoreService.getVideoData(_singleLink);
                _video = _pasteCoreService.video;

                Video = _video;

                _eventAggregator.Publish(new VideoAddedEventArgs(Video),
                    action => { Task.Factory.StartNew(action); });
            }
        }
        
        private void DownloadVideo(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            DownloadCoreService s = new DownloadCoreService();
            if (_links.Count > 0)
            {
                _destination = Properties.Settings.Default.Destination;
                s.DownloadMP3(_links, _destination);
            }
        }

        private void OnDownloadedVideo(object sender, RunWorkerCompletedEventArgs eventargs)
        {

        }

        #endregion

    }
}
