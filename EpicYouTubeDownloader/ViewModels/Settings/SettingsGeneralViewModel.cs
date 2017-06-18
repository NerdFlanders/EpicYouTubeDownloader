using System;
using System.Configuration;
using Caliburn.Micro;
using MediaToolkit.Options;

namespace EpicYouTubeDownloader.ViewModels.Settings
{
    public class SettingsGeneralViewModel : Screen
    {
        #region Public Properties

        public string Destination 
        {
            get { return _destination; }
            set
            {
                if (Equals(value, _destination)) return;
                _destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
        }

        public IObservableCollection<string> SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (Equals(value, _sampleRate)) return;
                _sampleRate = value;
                NotifyOfPropertyChange(() => SampleRate);
            }
        }

        public string DefaultSampleRate
        {
            get { return _defaultSapleRate; }
            set
            {
                if (Equals(value, _defaultSapleRate)) return;
                _defaultSapleRate = value;
                NotifyOfPropertyChange(() => DefaultSampleRate);
            }
        }

        #endregion

        #region Private Properties

        private string _destination;
        private IObservableCollection<string> _sampleRate;
        private string _defaultSapleRate;

        #endregion

        public SettingsGeneralViewModel()
        {
            SampleRate = new BindableCollection<string>();

            InitializeValues();
        }

        private void InitializeValues()
        {

            if (ConfigurationSettings.AppSettings.Count < 2)
            {
                _destination = Environment.SpecialFolder.MyMusic.ToString();
                Destination = _destination;
            }
            else
            {
                Destination = ConfigurationSettings.AppSettings.Get("Destination");
                DefaultSampleRate = ConfigurationSettings.AppSettings.Get("SampleRate");
            }
            GetEnumerables();
            
        }

        private void CreateFile()
        {
            ConfigurationSettings.AppSettings.Add("Destination", _destination);
            ConfigurationSettings.AppSettings.Add("SampleRate", SampleRate[0]);
        }

        private void SavetoFile()
        {
            
        }

        private void GetEnumerables()
        {
            SampleRate.Add(AudioSampleRate.Default.ToString());
            SampleRate.Add(AudioSampleRate.Hz22050.ToString());
            SampleRate.Add(AudioSampleRate.Hz44100.ToString());
            SampleRate.Add(AudioSampleRate.Hz48000.ToString());
        }
    }
}