using System;
using EpicYouTubeDownloader.Models.Domain;

namespace EpicYouTubeDownloader.ViewModels.EventArguments
{
    public class VideoAddedEventArgs : EventArgs
    {
         public YTVideo Video { get; set; }

        public VideoAddedEventArgs(YTVideo video)
        {
            Video = video;
        }
    }
}