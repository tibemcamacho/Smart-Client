using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reactive;

namespace Elipgo.SmartClient.ViewModels
{
    public class WelcomeViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private MainViewModel _mainView = null;
        //private CatalogDTO _catalog = null;
        private Image _fondo;
        private WebClient _vmon5ApiClient;
        private readonly string _smartClientManualPath = VariableResources.SmartClientManualPath;
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();

        public ReactiveCommand<Unit, Unit> ShowLiveCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowPlaybackCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFaceCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowRetailCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowLprCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowOcrCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowAlarmCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowVaultCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowReportCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowVisualSearchCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSeprobanCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowInvoiceCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSecondViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowCRMViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowZktecoCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowAccessControlCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowAvlCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSyncroBackCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowCADCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowFortigateCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowDACCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowClientManagerCommand { get; }
		public ReactiveCommand<Unit, Unit> ShowSmartEventCommand { get; }

        //public CatalogDTO Catalog
        //{
        //    get => this._catalog;
        //    set => this.RaiseAndSetIfChanged(ref this._catalog, value);
        //}

        public Image Fondo
        {
            get => this._fondo;
            set => this.RaiseAndSetIfChanged(ref this._fondo, value);
        }

        public MainViewModel MainView
        {
            get => this._mainView;
            set => this.RaiseAndSetIfChanged(ref this._mainView, value);
        }

        public WelcomeViewModel()
        {
            ShowLiveCommand = ReactiveCommand.Create(ShowLive);
            ShowPlaybackCommand = ReactiveCommand.Create(ShowPlayback);
            ShowRetailCommand = ReactiveCommand.Create(ShowRetail);
            ShowFaceCommand = ReactiveCommand.Create(ShowFace);
            ShowLprCommand = ReactiveCommand.Create(ShowLpr);
            ShowOcrCommand = ReactiveCommand.Create(ShowOcr);
            ShowAlarmCommand = ReactiveCommand.Create(ShowAlarm);
            ShowVaultCommand = ReactiveCommand.Create(ShowVault);
            ShowReportCommand = ReactiveCommand.Create(ShowReport);
            ShowVisualSearchCommand = ReactiveCommand.Create(ShowVisualSearch);
            ShowSeprobanCommand = ReactiveCommand.Create(ShowSeproban);
            ShowInvoiceCommand = ReactiveCommand.Create(ShowInvoice);
            ShowSecondViewCommand = ReactiveCommand.Create(ShowSecondView);
            ShowCRMViewCommand = ReactiveCommand.Create(ShowCRM);
            ShowAccessControlCommand = ReactiveCommand.Create(ShowAccessControl);
            ShowAvlCommand = ReactiveCommand.Create(ShowAvl);
            ShowSyncroBackCommand = ReactiveCommand.Create(ShowSyncroBack);
            ShowCADCommand = ReactiveCommand.Create(ShowCAD);
            ShowFortigateCommand = ReactiveCommand.Create(ShowFortigate);
            ShowDACCommand = ReactiveCommand.Create(ShowDAC);
            ShowClientManagerCommand = ReactiveCommand.Create(ShowClienManager);
            ShowSmartEventCommand = ReactiveCommand.Create(ShowSmartEvent);
        }

        public void Dispose()
        {
            _vmon5ApiClient?.Dispose();
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        private void ShowPlayback()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToPlayback();
        }

        private void ShowLive()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToLive();
        }

        private void ShowRetail()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToRetail();
        }

        private void ShowInvoice()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToInvoice();
        }

        private void ShowFace()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToFace();
        }

        private void ShowLpr()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToLpr();
        }

        private void ShowOcr()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToOcr();
        }

        private void ShowSecondView()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToSecondView();
        }

        private void ShowAlarm()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToAlarm();
        }

        private void ShowVault()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToVault();
        }

        private void ShowReport()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoReport();
        }

        private void ShowVisualSearch()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToVisualSearch();
        }

        private void ShowSeproban()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToSeproban();
        }

        private void ShowCRM()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoCRM();
        }

        private void ShowAccessControl()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoAccessControl();
        }

        private void ShowAvl()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoAvl();
        }

        private void ShowSyncroBack()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoSyncroBack();
        }

        private void ShowClienManager()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GotoClientManager();
        }

        private void ShowCAD()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToCAD();
        }

        private void ShowDAC()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToDAC();
        }
        
        private void ShowSmartEvent()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToSmartEvent();
        }

        private void ShowFortigate()
        {
            var main = _mainView ?? Locator.Current.GetService<IScreen>() as MainViewModel;
            main.GoToFortigate();
        }

        public void DownloadManual()
        {
            var config = SmartClientEnvironmentUtils.GetConfiguration();
            string vmon5Url = config.AppSettings.Settings["VMON5_URL"].Value;
            using (_vmon5ApiClient = new WebClient())
            {
                _vmon5ApiClient.Headers.Add("Authorization", "Bearer " + _mainView.UserToken);
                _vmon5ApiClient.DownloadFileCompleted += Vmon5ApiClient_DownloadFileCompleted;
                _vmon5ApiClient.DownloadFileTaskAsync(new Uri($"{vmon5Url}/v2/documents/smart-client"), _smartClientManualPath);
            }
        }

        private void Vmon5ApiClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Log(e.Error.Message, LogPriority.Information);
                if (e.Error.InnerException is IOException)
                {
                    _notification.Show(Resources.CannotAccessFile, null);
                }
                else
                {
                    _notification.Show(Resources.DownloadManualFail, null);
                }
            }
            else if (!File.Exists(_smartClientManualPath))
            {
                _notification.Show(Resources.DownloadManualFail, null);
            }
            else
            {
                Process.Start(_smartClientManualPath);
            }
            _vmon5ApiClient.DownloadFileCompleted -= Vmon5ApiClient_DownloadFileCompleted;
        }

        public void UpdateUserPreferences()
        {
            _container.UpdateUserPreferences();
        }
    }
}
