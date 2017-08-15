using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Input;
using Caliburn.Micro;
using EpicYouTubeDownloader.Models.Domain;
using EpicYouTubeDownloader.ViewModels.EventArguments;

namespace EpicYouTubeDownloader.ViewModels.Download
{
    internal class DownloadListViewModel : Screen, IHandle<VideoAddedEventArgs>
    {
        #region Public Properties

        public string VideoName
        {
            get { return _videoName; }
            set
            {
                if (Equals(value, _videoName)) return;
                _videoName = value;
                NotifyOfPropertyChange(() => VideoName);
            }
        }

        public Bitmap Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                if (Equals(value, _thumbnail)) return;
                _thumbnail = value;
                NotifyOfPropertyChange(() => Thumbnail);
            }
        }

        public string Length
        {
            get { return _length; }
            set
            {
                if (Equals(value, _length)) return;
                _length = value;
                NotifyOfPropertyChange(() => Length);
            }
        }

        public IObservableCollection<YTVideo> Videos
        {
            get { return _videos; }
            set
            {
                if (Equals(value, _videos)) return;
                _videos = value;
                NotifyOfPropertyChange(() => Videos);
            }
        }
        #endregion


        #region Private Properties

        private YTVideo _video = new YTVideo();
        private string _videoName;
        private Bitmap _thumbnail;
        private string _length;
        private IObservableCollection<YTVideo> _videos; 

        private readonly IEventAggregator _eventAggregator;

        #endregion

        public void OpenFolder(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OpenFolder()
        {
            Process.Start(ConfigurationSettings.AppSettings.Get("Destination"));
        }

        public DownloadListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Videos = new BindableCollection<YTVideo>();

            _eventAggregator.Subscribe(this);
        }

        public void Handle(VideoAddedEventArgs message)
        {
            Videos.Add(message.Video);
        }
    }
}