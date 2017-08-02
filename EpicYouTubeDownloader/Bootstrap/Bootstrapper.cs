using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using EpicYouTubeDownloader.ViewModels;
using Ninject;


namespace EpicYouTubeDownloader.Bootstrap
{
    public class Bootstrapper : BootstrapperBase
    {
        private IKernel _ninject;
        private InitialSettingsService _initialSettingsService = new InitialSettingsService();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _ninject = new StandardKernel();

            _ninject.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            _ninject.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();

            _initialSettingsService.setupFoders();
        }

        protected override void OnExit(object sender, EventArgs eventArgs)
        {
            _ninject.Dispose();
            base.OnExit(sender, eventArgs);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            return _ninject.Get(serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _ninject.GetAll(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _ninject.Inject(instance);
        }
    }
}
