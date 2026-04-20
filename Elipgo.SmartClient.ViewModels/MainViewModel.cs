using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Implement;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.SignalR.Connection;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace Elipgo.SmartClient.ViewModels
{
    public delegate void OnAlarmUpdate(int qty);
    public delegate void OnCameraState();

    public class MainViewModel : ReactiveObject, IScreen, IDisposable
    {
        private const float HEADER_HEIGHT = 72;

        // Cache estático para evitar reflexión costosa en cada alarma
        private static readonly Dictionary<string, string> _alarmTypeAuthorizationCache = new Dictionary<string, string>();
        private static readonly object _alarmTypeCacheLock = new object();
        private static bool _alarmTypeCacheInitialized = false;

        public SidebarElementDTO SelectSidebarElement;
        public int ObjectGroupId;
        public DateTime? SelectedDateTime;
        public OnAlarmUpdate AlarmUpdate;
        public OnCameraState CameraState;
        //public CatalogDTO Catalog = null;

        private string _logoText;
        private string _userToken = "";
        private UserDTO _user = new UserDTO();
        private bool _activeModule;
        private string _nameModule;
        private bool _dataLoaded = false;
        private bool _spinner = false;
        private float _tableLayoutHeight = 0;
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private readonly IAlarmService _alarmService = Locator.Current.GetService<IAlarmService>();
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly IAuditService _auditService = Locator.Current.GetService<IAuditService>();
        private readonly IVisualSearchService _visualSearchService = Locator.Current.GetService<IVisualSearchService>();
        private IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly ICatalogService _catalogService = Locator.Current.GetService<ICatalogService>();
        private readonly IAlarmCounterService _alarmCounterService = Locator.Current.GetService<IAlarmCounterService>();
        private CatalogDTO _catalog = new CatalogDTO();
        public int _dvfid = -1;
        private int _lastNotifiedAlarmCount = -1;

        public CatalogDTO Catalog
        {
            get
            {
                _catalog = _catalogService.CurrentCatalog;
                return _catalog;
            }
            set
            {
                _catalogService.CurrentCatalog = value;
                this.RaiseAndSetIfChanged(ref _catalog, value); // Solo si necesitas disparar eventos UI
            }
        }
        public bool Spinner
        {
            get => _spinner;
            set => this.RaiseAndSetIfChanged(ref _spinner, value);
        }

        public string UserToken
        {
            get => _userToken;
            set => this.RaiseAndSetIfChanged(ref _userToken, value);
        }

        public string LogoText
        {
            get => _logoText;
            set => this.RaiseAndSetIfChanged(ref _logoText, value);
        }

        public UserDTO User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }

        public bool ActiveModule
        {
            get => this._activeModule;
            set => this.RaiseAndSetIfChanged(ref this._activeModule, value);
        }

        public string NameModule
        {
            get => this._nameModule;
            set => this.RaiseAndSetIfChanged(ref this._nameModule, value);
        }

        private Guid _id;
        public Guid ID
        {
            get => this._id;
            set => this.RaiseAndSetIfChanged(ref this._id, value);
        }

        private Apps _activeApp = Apps.None;
        public Apps ActiveApp
        {
            get => this._activeApp;
            set => this.RaiseAndSetIfChanged(ref this._activeApp, value);
        }

        private CardDto _cardAlarm;
        public CardDto CardAlarm
        {
            get => this._cardAlarm;
            set => this.RaiseAndSetIfChanged(ref this._cardAlarm, value);
        }

        private string _applicationTitle;
        public string ApplicationTitle
        {
            get => _applicationTitle;
            set => this.RaiseAndSetIfChanged(ref _applicationTitle, value);
        }

        private bool _showLastLogin = false;
        public bool ShowLastLogin
        {
            get => _showLastLogin;
            set => this.RaiseAndSetIfChanged(ref _showLastLogin, value);
        }

        private bool _isInitialRun = false;
        public bool IsInitialRun
        {
            get => _isInitialRun;
            set => this.RaiseAndSetIfChanged(ref _isInitialRun, value);
        }
        public RoutingState Router { get; }

        public MainViewModel()
        {
            // Create router for IScreen
            Router = new RoutingState();
            // Set properties
            ApplicationTitle = Resources.TitleApp;
            // NOTA: EnableSignalREvents() se movió a FinishLoad() para evitar
            // que las alarmas interfieran durante el inicio de la aplicación
        }

        private void EnableSignalREvents()
        {
            // NOTA: AlarmEventAction, OnReconected y RefreshAlarmsEventAction
            // ahora son manejados por AlarmCounterService (servicio centralizado)
            // para evitar llamadas duplicadas a GetUnattended
            // Solo mantenemos AlarmEvent para actualizar la UI de cards
            Signal.AlarmEventAction += AlarmEvent;
            // NO suscribir: Signal.OnReconected += AlarmReconnect; (causa doble GetUnattended)
            // NO suscribir: Signal.RefreshAlarmsEventAction += RefreshAlarmsEventAction; (causa doble GetUnattended)
        }

        //private void RefreshAlarmsEventAction(dynamic idAlarm)
        //{
        //    //aprovecho para actualizar las cantidad de alarmas cada vez que se crea un modulo
        //    this._listAlarm = this._alarmService.GetUnattended(UserToken);
        //    // Mantener VM y binding consistentes
        //    this.ListAlarm = this._listAlarm;
        //    Logger.Log($"FinishLoad App: {NameModule} cantidad actual: {_listAlarm}", LogPriority.Information);
        //    this.AlarmUpdate?.Invoke(_listAlarm);
        //}

        //private void RefreshAlarmsEventAction(dynamic idAlarm)
        //{
        //    // Si hay incrementos pendientes, no sobrescribir
        //    int pending = Interlocked.CompareExchange(ref _pendingIncrements, 0, 0);
        //    if (pending > 0)
        //    {
        //        Logger.Log($"RefreshAlarmsEventAction: Ignorando refresh porque hay {pending} incrementos pendientes", LogPriority.Information);
        //        return;
        //    }

        //    this._listAlarm = this._alarmService.GetUnattended(UserToken);
        //    this.ListAlarm = this._listAlarm;
        //    Logger.Log($"RefreshAlarmsEventAction App: {NameModule} cantidad actual: {_listAlarm}", LogPriority.Information);
        //    this.AlarmUpdate?.Invoke(_listAlarm);
        //}

        private async Task RefreshAlarmsEventAction(dynamic idAlarm)
        {
            // NOTA: Este método ya no se usa - AlarmCounterService maneja RefreshAlarms
            var newCount = await this._alarmService.GetUnattended(UserToken);

            // Solo actualizar si el valor cambió
            if (this._listAlarm == newCount && this._lastNotifiedAlarmCount == newCount)
            {
                return; // Valor sin cambios, no hacer nada
            }

            this._listAlarm = newCount;
            this.ListAlarm = this._listAlarm;
            Logger.Log($"RefreshAlarmsEventAction App: {NameModule} cantidad actual: {_listAlarm}", LogPriority.Information);

            // Solo notificar si realmente cambió
            if (_lastNotifiedAlarmCount != newCount)
            {
                _lastNotifiedAlarmCount = newCount;
                this.AlarmUpdate?.Invoke(newCount);
            }
        }

        //TODO
        private async Task AlarmReconnect(bool connected)
        {
            if (connected)
            {
                await GetAlarms();
            }
        }

        public VmonitoringManagerConnection Signal { get; } = Locator.Current.GetService<VmonitoringManagerConnection>();

        private void ListenSignal()
        {
            Task.Run(async () =>
            {
                await Signal.ConnectoToServer(UserToken);
                Signal.OnClosed += Signal_OnClosed;
            });
        }

        private void Signal_OnClosed(bool disconnected)
        {
            // NOTA: No llamar GetAlarms() aquí porque AlarmCounterService
            // maneja la obtención de alarmas de forma centralizada.
            // El evento OnClosed se dispara con disconnected=false cuando
            // la conexión inicial se establece exitosamente (RaiseOnClosed(false)),
            // lo que causaba una segunda llamada a GetUnattended.

            if (!disconnected)
            {
                // Conexión exitosa - el contador ya está manejado por AlarmCounterService
                Logger.Log("Signal_OnClosed: Conexión establecida, AlarmCounterService maneja alarmas", LogPriority.Information);
            }
            else
            {
                // Desconexión - AlarmCounterService sincronizará al reconectar
                Logger.Log("Signal_OnClosed: Desconectado", LogPriority.Information);
            }
        }

        private void ShowLive()
        {
            Spinner = true;
            //GetSiteInfoAsync();
            // Navigate to LiveViewModel
            var liveModel = new LiveViewModel
            {
                Parent = ID,
                MainView = this
            };

            Router
                .Navigate
                .Execute(liveModel)
                .Subscribe();
        }

        private void ShowRetail()
        {
            var RetailViewModel = new RetailViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(RetailViewModel)
                .Subscribe();
        }

        private void ShowInvoice()
        {
            var InvoiceViewModel = new InvoiceViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(InvoiceViewModel)
                .Subscribe();
        }

        private void ShowSecondView()
        {
            var SecondViewModel = new SecondViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(SecondViewModel)
                .Subscribe();
        }

        private void ShowFace()
        {
            var FaceViewModel = new FaceViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(FaceViewModel)
                .Subscribe();
        }

        private void ShowWelcome()
        {
            var welcomeView = new WelcomeViewModel
            {
                //Catalog = Catalog,
                Fondo = PictureFondo,
                MainView = this
            };

            Router
                .NavigateAndReset
                .Execute(welcomeView)
                .Subscribe();
        }

        private void ShowAlarm()
        {
            var alarmModel = new AlarmViewModel(this)
            {
                //Catalog = Catalog,
                Parent = ID,
                CardAlarm = this.CardAlarm
            };

            Router
                .Navigate
                .Execute(alarmModel)
                .Subscribe();
        }

        private void ShowAlarmPanelHeader()
        {
            var alarmModel = new AlarmViewModel(this)
            {
                //Catalog = Catalog,
                Parent = ID,
            };

            Router
                .Navigate
                .Execute(alarmModel)
                .Subscribe();
        }

        private void ShowPlayback()
        {
            Spinner = true;
            //GetSiteInfoAsync();
            var playbackModel = new PlaybackViewModel
            {
                //Catalog = Catalog,
                Parent = ID,
                MainView = this
            };

            Router
                .Navigate
                .Execute(playbackModel)
                .Subscribe();
        }

        private void ShowLpr()
        {
            var lprViewModel = new LprViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(lprViewModel)
                .Subscribe();
        }

        private void ShowOcr()
        {
            var ocrViewModel = new OcrViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(ocrViewModel)
                .Subscribe();
        }

        private void ShowNav()
        {
            Router
                .Navigate
                .Execute(new NavViewModel())
                .Subscribe();
        }

        private void GoBack()
        {
            // Navigate back in NavigationStack 
            // Note: You have to check the count to prevent an ArgumentOutOfRangeException or navigate to empty
            if (Router.NavigationStack.Count > 0)
            {
                Router
                    .NavigateBack
                    .Execute()
                    .Subscribe();
            }
        }

        private void ShowVault()
        {
            var vaultModel = new VaultViewModel
            {
                //Catalog = Catalog,
                Parent = ID,
                MainView = this
            };

            Router
                .Navigate
                .Execute(vaultModel)
                .Subscribe();
        }

        private void ShowVisualSearch()
        {
            var visualSearchModel = new VisualSearchViewModel
            {
                //Catalog = Catalog,
                Parent = ID,
                MainView = this,
                SelectSidebarElement = this.SelectSidebarElement,
                SelectedDateTime = this.SelectedDateTime
            };

            Router
                .Navigate
                .Execute(visualSearchModel)
                .Subscribe();
        }

        private void ShowSeproban()
        {
            var seprobanModel = new SeprobanViewModel
            {
                Parent = ID,
                MainView = this,
            };

            Router
                .Navigate
                .Execute(seprobanModel)
                .Subscribe();
        }

        private void ShowReport()
        {
            var reportViewModel = new ReportViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(reportViewModel)
                .Subscribe();
        }

        private void ShowAccessControl()
        {
            var accessControlViewModel = new AccessControlViewModel //SupremaViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(accessControlViewModel)
                .Subscribe();
        }

        private void ShowAvl()
        {
            var avlViewModel = new AvlViewModel //AvlViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(avlViewModel)
                .Subscribe();
        }

        private void ShowSyncroBack()
        {
            var syncroBackViewModel = new SyncroBackViewModel //SyncroBackViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(syncroBackViewModel)
                .Subscribe();
        }

        private void ShowCRM()
        {
            var crmViewModel = new CRMViewModel
            {
                MainView = this,
            };

            Router
                .Navigate
                .Execute(crmViewModel)
                .Subscribe();
        }

        private void ShowCAD()
        {
            var cadViewModel = new CADViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(cadViewModel)
                .Subscribe();
        }

        private void ShowFortigate()
        {
            var forigateViewModel = new FortigateViewModel
            {
                MainView = this,
            };

            Router
                .Navigate
                .Execute(forigateViewModel)
                .Subscribe();
        }

        private void ShowDAC()
        {
            var dacViewModel = new DACViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(dacViewModel)
                .Subscribe();
        }

        private void ShowClientManager()
        {
            var clientManagerViewModel = new ClientManagerViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(clientManagerViewModel)
                .Subscribe();
        }

        private void ShowSmartEvent()
        {
            var smartEventViewModel = new SmartEventViewModel
            {
                MainView = this
            };

            Router
                .Navigate
                .Execute(smartEventViewModel)
                .Subscribe();
        }

        public void GoToLive()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Live";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Live;
                this.CameraState?.Invoke();
                ShowLive();
            }
        }

        public void GoToRetail()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Retail";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Retail;
                ShowRetail();
            }
        }

        public void GoToInvoice()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Invoice";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Invoice;
                ShowInvoice();
            }
        }

        public void GoToSecondView()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Second View";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.SecondView;
                ShowSecondView();
            }
        }

        public void GoToFace()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Face";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.FaceRecognition;
                ShowFace();
            }
        }

        public void GoToPlayback()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Playback";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Playback;
                this.CameraState?.Invoke();
                ShowPlayback();
            }
        }

        public void GoToLpr()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Lpr";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Lpr;
                ShowLpr();
            }
        }

        public void GoToOcr()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Ocr";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Ocr;
                ShowOcr();
            }
        }

        public void GoToWelcome()
        {
            ShowWelcome();
        }

        public void DisableSignalREvents()
        {
            Dispose();
        }

        public void HideButtonsTitlebar(bool jump = false)
        {
            _container.HideButtonsTitlebar(this.ID, jump);
        }

        public void EnableSignalEvents()
        {
            EnableSignalREvents();
        }

        public bool CloseOtherTabs()
        {
            return _container.CloseOtherTabs(this.ID, UserToken);
        }

        public void GoToAlarmFromMenu(CardDto itemAlarm = null)
        {
            if (itemAlarm != null)
            {
                _container.JumpToApp(app: Apps.Alarms, card: itemAlarm);
            }
            else
            {
                _container.JumpToApp(app: Apps.Alarms);
            }

        }

        public void GoToLiveFromMenu(CardDto itemAlarm, SidebarElementDTO element, int groupId = 0)
        {
            if (itemAlarm != null || groupId > 0)
            {
                _container.JumpToApp(app: Apps.Live, element, card: itemAlarm, groupId: groupId);
            }
            else
            {
                _container.JumpToApp(app: Apps.Live);
            }
        }

        public void GoToAlarm()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Alarms";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Alarms;
                ShowAlarm();
            }
        }

        public void GoToVault()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Vault";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Vault;
                ShowVault();
            }
        }

        public void GotoReport()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Report";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Report;
                ShowReport();
            }
        }

        public void GotoAccessControl()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "AccessControl";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.AccessControl;
                ShowAccessControl();
            }
        }

        public void GotoAvl()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Avl";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Avl;
                ShowAvl();
            }
        }

        public void GotoSyncroBack()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "SyncroBack";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.SyncroBack;
                ShowSyncroBack();
            }
        }

        public void GotoCRM()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "CRM";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.CRM;
                ShowCRM();
            }
        }


        public void GoToVisualSearch()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "VisualSearch";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.VisualSearch;
                ShowVisualSearch();
            }
        }

        public void GoToSeproban()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Seproban";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Seproban;
                ShowSeproban();
            }
        }

        public void GoToCAD()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "CAD";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.CAD;
                ShowCAD();
            }
        }

        public void GoToFortigate()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Fortigate";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.Fortigate;
                ShowFortigate();
            }
        }
        public void GoToDAC()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "DAC";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.DAC;
                ShowDAC();
            }
        }

        public void GotoClientManager()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "Client Manager";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.ClientManager;
                ShowClientManager();
            }
        }
        public void GoToSmartEvent()
        {
            if (User.Email != "" && _dataLoaded)
            {
                NameModule = "SmartEvent";
                ActiveModule = true;
                TabletLayoutHeight = HEADER_HEIGHT;
                ActiveApp = Apps.SmartEvent;
                ShowSmartEvent();
            }
        }
        //public void GetCatalog()
        //{
        //    if (Catalog == null && _userToken != "")
        //        Catalog = Vmon5Service.GetUserInfo(_userToken);
        //    else
        //        Catalog = null;
        //}

        public void FinishLoad(bool canload)
        {
            _dataLoaded = true;
            ApplicationTitle += " *";

            // Suscribirse al servicio centralizado de alarmas
            if (_alarmCounterService != null)
            {
                _alarmCounterService.AlarmCountChanged += OnAlarmCounterServiceChanged;
                _alarmCounterService.NewAlarmReceived += OnNewAlarmFromService;
            }

            if (canload)
            {
                // Conectar a SignalR (solo para otros eventos, no alarmas)
                ListenSignal();
                // NO llamar EnableSignalREvents() - el servicio centralizado maneja alarmas

                // Inicializar el servicio centralizado de alarmas (solo una vez)
                Task.Run(async () =>
                {
                    try
                    {
                        if (_alarmCounterService != null && !_alarmCounterService.IsInitialized)
                        {
                            string userRoles = _user?.UserRoles ?? "";
                            await _alarmCounterService.Initialize(this.UserToken, userRoles);
                        }

                        // Obtener el contador actual del servicio
                        int alarmCount = _alarmCounterService?.CurrentCount ?? 0;
                        this._listAlarm = alarmCount;
                        this.ListAlarm = alarmCount;
                        this._lastNotifiedAlarmCount = alarmCount;
                        Logger.Log($"FinishLoad App: {this.NameModule} current quantity: {this._listAlarm}", LogPriority.Information);
                        this.AlarmUpdate?.Invoke(alarmCount);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Excepcion FinishLoad {ex.Message}", LogPriority.Fatal);
                    }
                });
            }
            else
            {
                // Tab secundario: obtener el contador actual del servicio
                if (_alarmCounterService != null && _alarmCounterService.IsInitialized)
                {
                    int alarmCount = _alarmCounterService.CurrentCount;
                    this._listAlarm = alarmCount;
                    this.ListAlarm = alarmCount;
                    this._lastNotifiedAlarmCount = alarmCount;
                    this.AlarmUpdate?.Invoke(alarmCount);
                }
            }
        }

        private void OnAlarmCounterServiceChanged(int count)
        {
            // Actualizar contador local cuando el servicio notifica
            if (this._listAlarm != count)
            {
                this._listAlarm = count;
                this.ListAlarm = count;
            }
            if (_lastNotifiedAlarmCount != count)
            {
                _lastNotifiedAlarmCount = count;
                this.AlarmUpdate?.Invoke(count);
            }
        }

        private void OnNewAlarmFromService(object alarmData)
        {
            // Actualizar Card cuando llega una nueva alarma
            try
            {
                if (alarmData is CardDto cardDto)
                {
                    Card = cardDto;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"OnNewAlarmFromService error: {ex.Message}", LogPriority.Warning);
            }
        }

        #region TableLayout
        public float TabletLayoutHeight
        {
            get => _tableLayoutHeight;
            set => this.RaiseAndSetIfChanged(ref this._tableLayoutHeight, value);
        }
        #endregion

        #region AlarmEvents
        private int _listAlarm = 0;
        public int ListAlarm
        {
            get => this._listAlarm;
            set => this.RaiseAndSetIfChanged(ref this._listAlarm, value);
        }

        private bool _enableAlarm = false;
        public bool EnableAlarm
        {
            get => _enableAlarm;
            set => this.RaiseAndSetIfChanged(ref this._enableAlarm, value);
        }

        //private void AlarmEvent(dynamic d)
        //{
        //    try
        //    {
        //        if (_container.FirstTabMainViewId == this.ID)
        //        {
        //            if (d != null)
        //            {
        //                var t = Newtonsoft.Json.JsonConvert.DeserializeObject<CardDto>(d.ToString());
        //                Type type = typeof(AlarmType);
        //                MemberInfo mi = type.GetMember((t as CardDto).Type).FirstOrDefault(m => m.GetCustomAttribute(typeof(AlarmTypeOf)) != null);
        //                if (mi != null)
        //                {
        //                    AlarmTypeOf subAttr = (AlarmTypeOf)mi.GetCustomAttribute(typeof(AlarmTypeOf));
        //                    GroupAlarmType groupAlarmType = subAttr.AlarmType;
        //                    string authorization = groupAlarmType.GetDescription();
        //                    if (_appAuthorization == null)
        //                    {
        //                        _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        //                    }
        //                    if (_appAuthorization != null)
        //                    {
        //                        if (_appAuthorization.Exist(authorization))
        //                        {
        //                            Threads.RunInOtherThread(new Action[]{() => {
        //                                if (AddNewAlarm(t))
        //                                {
        //                                    Logger.Log("AlarmEvent Main", LogPriority.Information);
        //                                    // En ráfaga: acumular y publicar en batch
        //                                    Interlocked.Increment(ref _pendingIncrements);
        //                                    Card = t;
        //                                }
        //                                else
        //                                {
        //                                    Logger.Log($"Alarm discated: {(t ?? "")}", LogPriority.Information);
        //                                }
        //                            }}, null);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Logger.Log("appAuthorization is null", LogPriority.Fatal);
        //                    }
        //                }
        //                else
        //                {
        //                    throw new ArgumentException($"Group alarm type {t.Type} has no category.");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log($"MainViewModel AlarmEvent Exception: {ex}", LogPriority.Fatal);
        //    }
        //}

        /// <summary>
        /// Inicializa el cache de tipos de alarma para evitar reflexión en cada alarma
        /// </summary>
        private static void InitializeAlarmTypeCache()
        {
            if (_alarmTypeCacheInitialized) return;

            lock (_alarmTypeCacheLock)
            {
                if (_alarmTypeCacheInitialized) return;

                try
                {
                    Type type = typeof(AlarmType);
                    foreach (var member in type.GetMembers())
                    {
                        var attr = member.GetCustomAttribute(typeof(AlarmTypeOf)) as AlarmTypeOf;
                        if (attr != null)
                        {
                            string authorization = attr.AlarmType.GetDescription();
                            _alarmTypeAuthorizationCache[member.Name] = authorization;
                        }
                    }
                    _alarmTypeCacheInitialized = true;
                }
                catch (Exception ex)
                {
                    Logger.Log($"InitializeAlarmTypeCache error: {ex.Message}", LogPriority.Warning);
                }
            }
        }

        /// <summary>
        /// Obtiene la autorización para un tipo de alarma usando cache (sin reflexión)
        /// </summary>
        private static string GetAlarmTypeAuthorization(string alarmType)
        {
            // Inicializar cache si es necesario (solo una vez)
            if (!_alarmTypeCacheInitialized)
                InitializeAlarmTypeCache();

            // Buscar en cache O(1)
            if (_alarmTypeAuthorizationCache.TryGetValue(alarmType, out string authorization))
                return authorization;

            return null; // Tipo no encontrado
        }

        private void AlarmEvent(dynamic d)
        {
            try
            {
                // Si no hay app activa, descartar inmediatamente
                if (ActiveApp == Apps.None)
                    return;

                if (_container.FirstTabMainViewId != this.ID)
                    return;

                if (d == null)
                    return;

                // Deserializar
                var t = Newtonsoft.Json.JsonConvert.DeserializeObject<CardDto>(d.ToString());
                if (t == null) return;

                // Usar cache en lugar de reflexión
                string authorization = GetAlarmTypeAuthorization(t.Type);

                if (authorization != null)
                {
                    if (_appAuthorization == null)
                    {
                        _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
                    }

                    if (_appAuthorization != null && _appAuthorization.Exist(authorization))
                    {
                        if (AddNewAlarm(t))
                        {
                            // Actualizar UI con la nueva card
                            // NOTA: El contador es manejado por AlarmCounterService
                            Task.Run(() =>
                            {
                                try
                                {
                                    Card = t;
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log($"AlarmEvent Card assignment error: {ex.Message}", LogPriority.Warning);
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"MainViewModel AlarmEvent Exception: {ex}", LogPriority.Fatal);
            }
        }

        public Guid FirstTabMainViewId => _container.FirstTabMainViewId;

        private bool AddNewAlarm(CardDto card)
        {
            bool result = true;
            try
            {
                int[] profiles = Array.ConvertAll(_user.UserRoles.Substring(1, _user.UserRoles.Length - 2).Split(','), s => int.Parse(s));
                result = profiles.ToList().Exists(r => card.Profile != null && card.Profile.Contains(r));
            }
            catch
            {
            }
            return result;
        }

        private CardDto _card;
        public CardDto Card
        {
            get => _card;
            set
            {
                this.RaiseAndSetIfChanged(ref this._card, value);
                this._card = null;
            }
        }

        public async Task<IList<SiteCatalogVscDTO>> GetVSCElements()
        {
            return await this._visualSearchService.Get(this.UserToken);
        }

        public async Task<System.Net.Http.HttpResponseMessage> GetCapture(string url)
        {
            return await this._visualSearchService.GetCapture(url, this.UserToken);
        }

        //public void Dispose()
        //{
        //    Signal.AlarmEventAction -= AlarmEvent;
        //    Signal.OnReconected -= AlarmReconnect;
        //    Signal.OnClosed -= Signal_OnClosed;
        //}
        public void Dispose()
        {
            // Desuscribirse del servicio centralizado
            if (_alarmCounterService != null)
            {
                _alarmCounterService.AlarmCountChanged -= OnAlarmCounterServiceChanged;
                _alarmCounterService.NewAlarmReceived -= OnNewAlarmFromService;
            }

            // Desconectar eventos de SignalR suscritos en EnableSignalREvents
            Signal.AlarmEventAction -= AlarmEvent;
            Signal.OnClosed -= Signal_OnClosed;
        }

        public async Task GetAlarms()
        {
            if (_container.FirstTabMainViewId == this.ID)
            {
                try
                {
                    this._listAlarm = await this._alarmService.GetUnattended(this.UserToken);
                    Logger.Log($"GetAlarms App: {this.NameModule} cantidad actual: {this._listAlarm}", LogPriority.Information);
                    this.AlarmUpdate?.Invoke(_listAlarm);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Excepcion GetAlarms {ex.Message}", LogPriority.Fatal);
                }
            }
        }

        public async Task<int> UpdateAlarms()
        {
            try
            {
                var qty = await this._alarmService.GetUnattended(this.UserToken);
                Logger.Log($"UpdateAlarms From Vmon5 App: {this.NameModule} current quantity: {qty}", LogPriority.Information);
                return qty;
            }
            catch (Exception ex)
            {
                Logger.Log($"Excepcion GetAlarms {ex.Message}", LogPriority.Fatal);
                //retorno la ultima cantidad que tenia es lo mejor que puedo hacer
                return this._listAlarm;
            }
        }

        public void AddAudits(IEnumerable<AuditDTO> request)
        {
            try
            {
                this._auditService.AddRange(request, this.UserToken);
            }
            catch (Exception ex)
            {
                Logger.Log("AddAudit Exception: " + ex.Message, LogPriority.Fatal);
            }
        }

        //public void GetCarouselInfo()
        //{
        //    try
        //    {
        //        var data = Vmon5Service.GetCarouselInfo(this.UserToken);
        //        Catalog.Sites = data.Sites;
        //        Logger.Log($"GetCarouselInfo From Vmon5 App: {this.NameModule}", LogPriority.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log($"Excepcion GetAlarms {ex.Message}", LogPriority.Fatal);
        //    }
        //}

        //public void UpdateAlarmBrocast(int quality)
        //{
        //    this._listAlarm = quality;
        //    // Aplicar de forma atómica
        //    Interlocked.Exchange(ref this._listAlarm, quality);
        //    this.ListAlarm = this._listAlarm;
        //}

        public void UpdateAlarmBrocast(int quality)
        {
            // Solo actualizar si el valor cambió
            if (this._listAlarm == quality && this._lastNotifiedAlarmCount == quality)
            {
                return;
            }

            Interlocked.Exchange(ref this._listAlarm, quality);
            this.ListAlarm = this._listAlarm;

            if (_lastNotifiedAlarmCount != quality)
            {
                _lastNotifiedAlarmCount = quality;
                this.AlarmUpdate?.Invoke(quality);
            }
        }

        public void AddAudit(AuditDTO request)
        {
            try
            {
                this._auditService.Add(request, this.UserToken);
            }
            catch (Exception ex)
            {
                Logger.Log($"AddAudit Exception: {ex.Message}", LogPriority.Fatal);
            }
        }

        public async Task DownloadManual()
        {
            try
            {
                string smartClientManualPath = VariableResources.SmartClientManualPath;
                Configuration _config = SmartClientEnvironmentUtils.GetConfiguration();
                string vmon = _config.AppSettings.Settings["VMON5_URL"].Value;
                using (var client = new WebClient())
                {
                    client.Headers.Add("Authorization", $"Bearer {this.UserToken}");
                    await client.DownloadFileTaskAsync(new Uri($"{vmon}/v2/documents/smart-client"), smartClientManualPath);
                }

                if (System.IO.File.Exists(smartClientManualPath))
                {
                    Process.Start(smartClientManualPath);
                }
                else
                {
                    _notification.Show(Resources.DownloadManualFail, null);
                }
            }
            catch (IOException)
            {
                _notification.Show(Resources.CannotAccessFile, null);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogPriority.Information);
                _notification.Show(Resources.DownloadManualFail, null);
            }
        }

        #endregion
        #region ContextBarRegion
        System.Windows.Forms.Control _containerContextBar;

        public System.Windows.Forms.Control ContainerContextBar
        {
            get => this._containerContextBar;
            set => this.RaiseAndSetIfChanged(ref this._containerContextBar, value);
        }

        private System.Windows.Forms.Control _mainBarControl;
        public System.Windows.Forms.Control MainBarControl
        {
            get => this._mainBarControl;
            set => this.RaiseAndSetIfChanged(ref this._mainBarControl, value);
        }

        private System.Windows.Forms.Label _labelMainBar;
        public System.Windows.Forms.Label LabelMainBar
        {
            get => this._labelMainBar;
            set => this.RaiseAndSetIfChanged(ref this._labelMainBar, value);
        }

        #endregion

        #region PictureLogo
        System.Drawing.Image _image;
        public System.Drawing.Image PictureLogo
        {
            get => this._image;
            set => this.RaiseAndSetIfChanged(ref this._image, value);
        }

        public System.Drawing.Image PictureFondo
        {
            get => this._image;
            set => this.RaiseAndSetIfChanged(ref this._image, value);
        }
        #endregion

        public List<KeyValuePair<Guid, Apps>> GetChildrensByParent()
        {
            return _container.AppsActives.Where(x => x.Key == ID).ToList();
        }

        public void RemoveAppsByParent()
        {
            var result = _container.AppsActives.Where(x => x.Key == ID).ToList();
            if (result != null)
            {
                foreach (var item in result)
                {
                    _container.AppsActives.Remove(item.Key);
                }
            }
        }

        public void SetApp(Apps app)
        {
            if (_container.AppsActives.ContainsKey(ID) == false)
            {
                _container.AppsActives.Add(ID, app);
                var n = _container.AppsActives.Where(x => x.Value == app).Count();
                ApplicationTitle = app.ToString();
            }
        }

        public void GoToHome()
        {
            ShowWelcome_Reset();
        }

        private void ShowWelcome_Reset()
        {
            this.ActiveApp = Apps.None;
            this.ApplicationTitle = Resources.WelcomeHome;
            this._container.AppsActives.Remove(ID);
            var welcomeView = new WelcomeViewModel
            {
                //Catalog = Catalog,
                Fondo = PictureFondo,
                MainView = this
            };

            Router
                .NavigateAndReset
                .Execute(welcomeView)
                .Subscribe();
        }

        public bool JumpToHome()
        {
            var existHome = _container.JumpToHome(this.ID);
            return existHome;
        }

        public bool IsTabSelected()
        {
            return _container.IsSelectedTab(this.ID);
        }

        public void UpdateUserPreferences()
        {
            _container.UpdateUserPreferences();
        }
    }
}
