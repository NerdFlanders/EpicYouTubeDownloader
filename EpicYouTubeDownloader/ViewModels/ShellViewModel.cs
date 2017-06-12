using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace EpicYouTubeDownloader.ViewModels
{
    internal class ShellViewModel : Conductor<object>
    {
        //public DownloadViewModel ArticleViewModel { get; set; }

        //public OptionsViewModel CustomerViewModel { get; set; }

        private const int ActiveItemMargin = -2;
        private const int InactiveItemMargin = 0;

        public ShellViewModel(//ArticleViewModel articleViewModel,
        //    CustomerViewModel customerViewModel,
        //    StatisticsViewModel statisticsViewModel,
        //    OrderViewModel orderViewModel)
        )
        {
            //ArticleViewModel = articleViewModel;
            //CustomerViewModel = customerViewModel;
            //OrderViewModel = orderViewModel;
            //StatisticsViewModel = statisticsViewModel;

            DisplayName = "EpicYouTubeDownloader";
        }

        public void OnTabSelectionChanged(TabControl sender)
        {
            foreach (TabItem item in sender.Items)
            {
                if (item.Equals(sender.SelectedItem))
                {
                    ChangeTabMargin(item, ActiveItemMargin);
                }
                else
                {
                    ChangeTabMargin(item, InactiveItemMargin);
                }
            }
        }

        private static void ChangeTabMargin(TabItem item, int value)
        {
            Thickness margin = item.Margin;
            margin.Bottom = value;
            item.Margin = margin;
        }
    }
}