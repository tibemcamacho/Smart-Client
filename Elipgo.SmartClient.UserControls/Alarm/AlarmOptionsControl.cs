using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmOptionsControl : UserControl
    {
        private readonly IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly IAlarmService _alarmService = Locator.Current.GetService<IAlarmService>();
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();

        public event EventHandler<object> ButtonClicked;
        public event EventHandler<object> LostFocusAlarm;

        private FlowLayoutPanel dynamicFlowLayoutPanel = null;
        private List<CardDto> ListAlarm { get; set; }
        private List<CardDto> _listyAlarmsTemp;
        private List<int> _alarmsRemoveByMe;
        private ConcurrentDictionary<int, ItemAlarmHeader> _syncListAsync;
        //private bool _sentRemoveAllAlarms = false;
        private bool _painted = false;
        private readonly Configuration _config;

        public bool HaveAlarms { get; set; }
        public bool LoadAlarm { get; set; }
        public MainViewModel MainView { get; set; }

        private static readonly string DEFAULT_ALARM_IMAGE = "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";

        private int _syncing = 0; // compuerta de re-entrada para SyncList
        private volatile bool _pendingUIUpdateAfterSync = false; // Flag para UI update cuando handle no está creado

        // Card pendiente que disparó la apertura del panel (para ShowPanelQuickView)
        // Se agrega después del SyncList inicial para evitar duplicados
        private CardDto _pendingCardFromSignalR = null;

        public AlarmOptionsControl()
        {
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            InitializeComponent();
            _panelFooter.Visible = false;
            _syncListAsync = new ConcurrentDictionary<int, ItemAlarmHeader>();
            ListAlarm = new List<CardDto>();
            _alarmsRemoveByMe = new List<int>();

            this._switch.OffColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_OFF);
            this._switch.OnColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_ON);
            _panelFooter.BackColor = ColorTranslator.FromHtml("#222222");
            _panelHeader.BackColor = ColorTranslator.FromHtml("#313131");

            this.Leave += AlarmOptionsControl_Close;
            this.LostFocus += AlarmOptionsControl_Close;
            this.Paint += AlarmOptionsControl_Paint;
            this._buttonShowAll.Click += ButtonShowAll_Click;
            this._buttonCancelAll.Click += ButtonCancelAll_Click;
            this._switch.Click += Switch_Click;
            this.Resize += AlarmOptionsControl_Resize;
            this.Disposed += AlarmOptionsControl_Disposed;
            this.HandleCreated += AlarmOptionsControl_HandleCreated;
            LoadAlarm = true;
        }

        private void AlarmOptionsControl_HandleCreated(object sender, EventArgs e)
        {
            // Si hay una actualización de UI pendiente porque el handle no estaba creado
            if (_pendingUIUpdateAfterSync)
            {
                _pendingUIUpdateAfterSync = false;
                try
                {
                    UpdateUIAfterSync();
                }
                catch (Exception ex)
                {
                    Logger.Log($"AlarmOptionsControl_HandleCreated UpdateUI error: {ex.Message}", LogPriority.Warning);
                }
            }
        }

        private void AlarmOptionsControl_Disposed(object sender, EventArgs e)
        {
            // Limpiar suscripciones de eventos
            this.Leave -= AlarmOptionsControl_Close;
            this.LostFocus -= AlarmOptionsControl_Close;
            this.Paint -= AlarmOptionsControl_Paint;
            this._buttonShowAll.Click -= ButtonShowAll_Click;
            this._buttonCancelAll.Click -= ButtonCancelAll_Click;
            this._switch.Click -= Switch_Click;
            this.Resize -= AlarmOptionsControl_Resize;
            this.HandleCreated -= AlarmOptionsControl_HandleCreated;
            this.Disposed -= AlarmOptionsControl_Disposed;
        }

        private void AlarmOptionsControl_Close(object sender, EventArgs e)
        {
            if (this.LostFocusAlarm == null)
            {
                this.Dispose();
            }

            this.LostFocusAlarm?.Invoke(sender, e);
        }

        /// <summary>
        /// Guarda una card pendiente que disparó la apertura del panel (ShowPanelQuickView).
        /// La card se agregará después del SyncList inicial si no existe ya.
        /// </summary>
        public void SetPendingCardFromSignalR(CardDto card)
        {
            _pendingCardFromSignalR = card;
            Logger.Log($"AlarmOptionsControl.SetPendingCardFromSignalR: Card {card?.IdAlarm} guardada como pendiente", LogPriority.Information);
        }

        private void AlarmOptionsControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;
            CreatePanelContainer();
            SyncList();

            // Si hay una card pendiente que disparó la apertura, verificar si se agregó en SyncList
            // Si no existe, agregarla directamente (puede pasar si el servidor aún no la tiene)
            if (_pendingCardFromSignalR != null)
            {
                var pendingCard = _pendingCardFromSignalR;
                _pendingCardFromSignalR = null; // Limpiar para no re-agregar

                if (ListAlarm != null && !ListAlarm.Any(x => x.IdAlarm == pendingCard.IdAlarm))
                {
                    Logger.Log($"AlarmOptionsControl: Card pendiente {pendingCard.IdAlarm} no encontrada en SyncList, agregando directamente", LogPriority.Information);
                    AddSingleCardFromSignalR(pendingCard);
                }
            }
        }

        private void CreatePanelContainer()
        {
            if (dynamicFlowLayoutPanel == null)
            {
                var a = _panelHeader.Right + 5;
                dynamicFlowLayoutPanel = new FlowLayoutPanel
                {
                    Name = "FlowLayoutPanel1",
                    Location = new Point(0, _panelHeader.Bottom + 8),
                    Size = new Size(this.Width - 10, this.Height - (_panelHeader.Height + _panelFooter.Height)),
                    TabIndex = 0,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = true,
                    BackColor = ColorTranslator.FromHtml("#2A2A2A"),
                    Margin = new Padding(0, 0, 0, 0)
                };
                dynamicFlowLayoutPanel.Scroll += (s, e) => { Application.DoEvents(); };
                this.Controls.Add(dynamicFlowLayoutPanel);
                dynamicFlowLayoutPanel.Visible = false;
            }
        }

        private async Task GetDataAlarms(List<CardDto> list)
        {
            // Service
            try
            {
                if (_listyAlarmsTemp != null)
                {
                    if (!Convert.ToBoolean(WorkFolderUtils.GetUserSettings("ShowPanelQuickView", true)) && list.SequenceEqual(_listyAlarmsTemp))
                    {
                        return;
                    }
                }

                if (_listyAlarmsTemp != null)
                {
                    _listyAlarmsTemp.Clear();
                }

                var filter = GetFilter();
                _listyAlarmsTemp = await GetData(filter);
            }
            catch (Exception ex)
            {
                Logger.Log("GetDataAlarms Excepcion " + ex.Message, LogPriority.Fatal);
            }

        }

        /// <summary>
        /// Agrega una card directamente desde SignalR sin hacer llamada HTTP al servidor.
        /// La CardDto ya viene completa desde SignalR con toda la información necesaria.
        /// </summary>
        public void AddSingleCardFromSignalR(CardDto card)
        {
            if (this.IsDisposed || this.Disposing)
                return;

            // Si el panel aún no está creado, ignorar (se cargará en Paint)
            if (dynamicFlowLayoutPanel == null || !_painted)
                return;

            // Ejecutar en UI thread
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate { AddSingleCardFromSignalR(card); });
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                return;
            }

            try
            {
                // Verificar duplicado usando Dictionary (O(1) en lugar de O(n))
                if (_syncListAsync.ContainsKey(card.IdAlarm))
                    return;

                // Inicializar lista si es necesario
                if (ListAlarm == null)
                    ListAlarm = new List<CardDto>();

                // Agregar a la lista
                ListAlarm.Add(card);

                // Suspender layout para evitar recálculos múltiples (mejora rendimiento en ráfagas)
                dynamicFlowLayoutPanel.SuspendLayout();
                try
                {
                    // Crear la card directamente con los datos de SignalR
                    ItemAlarmHeader iCard = new ItemAlarmHeader(card)
                    {
                        Location = new Point(5, 8),
                        Visible = true,
                        Name = card.IdAlarm.ToString(),
                        Width = dynamicFlowLayoutPanel.ClientSize.Width - 30
                    };
                    iCard.AlarmDiagnostic += ICard_AlarmDiagnostic;
                    iCard.AlarmGeoLocation += ICard_AlarmGeoLocation;
                    iCard.AlarmCancelar += ICard_AlarmCancelar;
                    iCard.AlarmShowGroupLive += ICard_ShowGroupLive;

                    _syncListAsync[card.IdAlarm] = iCard;

                    // Agregar al panel al inicio (alarmas más recientes primero)
                    dynamicFlowLayoutPanel.Controls.Add(iCard);
                    dynamicFlowLayoutPanel.Controls.SetChildIndex(iCard, 0);

                    // Recortar mensaje si excede
                    TruncateMessageAndSetTooltip(iCard, (ListAlarm.Count < 3));
                }
                finally
                {
                    dynamicFlowLayoutPanel.ResumeLayout(true);
                }

                // Mostrar el panel si estaba oculto
                if (!dynamicFlowLayoutPanel.Visible)
                    dynamicFlowLayoutPanel.Visible = true;

                // Si el snapshot no viene en la CardDto, pedirlo en background
                if (string.IsNullOrEmpty(card.Snapshot))
                {
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var snapshot = await _alarmService.GetSnapshot(MainView?.UserToken, card.IdAlarm);
                            if (!string.IsNullOrEmpty(snapshot) && _syncListAsync.ContainsKey(card.IdAlarm))
                            {
                                if (this.InvokeRequired)
                                {
                                    this.BeginInvoke((MethodInvoker)delegate
                                    {
                                        if (_syncListAsync.ContainsKey(card.IdAlarm))
                                            _syncListAsync[card.IdAlarm].SetImage(snapshot);
                                    });
                                }
                                else
                                {
                                    _syncListAsync[card.IdAlarm].SetImage(snapshot);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"AlarmOptionsControl.AddSingleCardFromSignalR GetSnapshot error: {ex.Message}", LogPriority.Warning);
                        }
                    });
                }
                else
                {
                    // Ya viene el snapshot, usarlo directamente
                    _syncListAsync[card.IdAlarm].SetImage(card.Snapshot);
                }

                HaveAlarms = ListAlarm.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmOptionsControl.AddSingleCardFromSignalR Exception: {ex.Message}", LogPriority.Warning);
            }
        }

        /// <summary>
        /// Fuerza una sincronización inmediata (para cuando el usuario abre el panel manualmente)
        /// </summary>
        public void ForceSyncImmediate()
        {
            if (this.IsDisposed || this.Disposing)
                return;
            SyncList();
        }

        public void SyncList()
        {
            try
            {
                // Verificar si el control fue disposed
                if (this.IsDisposed || this.Disposing)
                    return;

                if (dynamicFlowLayoutPanel == null)
                    return;

                // Verificar si el FlowLayoutPanel fue disposed
                if (dynamicFlowLayoutPanel.IsDisposed || dynamicFlowLayoutPanel.Disposing)
                    return;

                // evita solapes si ya hay una ejecución en curso
                if (System.Threading.Interlocked.Exchange(ref _syncing, 1) == 1)
                    return;

                Logger.Log("syncList Entered", LogPriority.Information);

                // Ejecutar la llamada HTTP en background para NO bloquear la UI
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        // Verificar si fue disposed
                        if (this.IsDisposed || this.Disposing)
                        {
                            System.Threading.Interlocked.Exchange(ref _syncing, 0);
                            return;
                        }

                        // Llamada HTTP en background (NO bloquea UI) - AHORA ESPERA
                        await GetDataAlarms(ListAlarm);

                        // Verificar nuevamente si fue disposed después de la llamada HTTP
                        if (this.IsDisposed || this.Disposing)
                        {
                            System.Threading.Interlocked.Exchange(ref _syncing, 0);
                            return;
                        }

                        // Ahora actualizar la UI en el hilo correcto
                        if (dynamicFlowLayoutPanel != null && !dynamicFlowLayoutPanel.IsDisposed && !dynamicFlowLayoutPanel.Disposing)
                        {
                            try
                            {
                                dynamicFlowLayoutPanel.Invoke((MethodInvoker)(() => UpdateUIAfterSync()));
                            }
                            catch (ObjectDisposedException)
                            {
                                // Control disposed, ignorar
                            }
                            catch (InvalidOperationException)
                            {
                                // Handle not created - marcar para ejecutar cuando se cree
                                _pendingUIUpdateAfterSync = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"SyncList background task exception: {ex.Message}", LogPriority.Warning);
                    }
                    finally
                    {
                        System.Threading.Interlocked.Exchange(ref _syncing, 0);
                    }
                });
            }
            catch (Exception e)
            {
                Logger.Log("syncList: " + e.Message, LogPriority.Fatal);
                System.Threading.Interlocked.Exchange(ref _syncing, 0);
            }
        }

        /// <summary>
        /// Actualiza la UI después de obtener los datos (debe ejecutarse en UI thread)
        /// </summary>
        private void UpdateUIAfterSync()
        {
            try
            {
                // Verificar si el control fue disposed
                if (this.IsDisposed || this.Disposing)
                    return;

                if (dynamicFlowLayoutPanel == null || dynamicFlowLayoutPanel.IsDisposed || dynamicFlowLayoutPanel.Disposing)
                    return;

                // Si no hay alarmas, muestra el mensaje vacío
                if (_listyAlarmsTemp == null || _listyAlarmsTemp.Count == 0)
                {
                    HaveAlarms = false;
                    dynamicFlowLayoutPanel.Visible = false;
                    this.Height = 250;
                    _panelFooter.Visible = false;

                    var label = this.Controls.Find("lblEmptyListAlarms", true).FirstOrDefault() as Label;
                    if (label == null)
                    {
                        label = new Label()
                        {
                            Name = "lblEmptyListAlarms",
                            Text = Resources.EmptyNotifications,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = Common.Helpers.FontHelper.GetRobotoRegular(Common.Helpers.FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel),
                            ForeColor = Color.White,
                            Size = new Size(this.Width - 50, 20)
                        };
                        this.Controls.Add(label);
                        CenterXY(this, label);
                    }
                    label.Visible = true;
                    return;
                }
                else
                {
                    HaveAlarms = true;
                    dynamicFlowLayoutPanel.Visible = true;
                    this.Height = 500;
                    dynamicFlowLayoutPanel.Size = new Size(this.Width - 10, this.Height - (_panelHeader.Height + _panelFooter.Height));
                    _panelFooter.Visible = true;

                    var label = this.Controls.Find("lblEmptyListAlarms", true).FirstOrDefault() as Label;
                    if (label != null)
                        label.Visible = false;
                }

                // Eliminar alarmas que ya no están
                var removed = ListAlarm.Where(ds => !_listyAlarmsTemp.Any(db => db.IdAlarm == ds.IdAlarm)).ToList();
                foreach (var elements in removed)
                {
                    // Desuscribir eventos antes de remover para evitar memory leaks
                    if (_syncListAsync.TryGetValue(elements.IdAlarm, out var iCard))
                    {
                        iCard.AlarmDiagnostic -= ICard_AlarmDiagnostic;
                        iCard.AlarmGeoLocation -= ICard_AlarmGeoLocation;
                        iCard.AlarmCancelar -= ICard_AlarmCancelar;
                        iCard.AlarmShowGroupLive -= ICard_ShowGroupLive;
                        iCard.CustomDispose();
                    }
                    dynamicFlowLayoutPanel.Controls.RemoveByKey(elements.IdAlarm.ToString());
                    ListAlarm.Remove(elements);
                    _syncListAsync.TryRemove(elements.IdAlarm, out _);
                }

                // Agregar nuevas alarmas
                var newAlarms = _listyAlarmsTemp.Where(ds => !ListAlarm.Any(db => db.IdAlarm == ds.IdAlarm)).Distinct().ToList();
                foreach (var newElement in newAlarms)
                {
                    ListAlarm.Add(newElement);

                    // Crear nuevo Item para Panel
                    ItemAlarmHeader iCard = new ItemAlarmHeader(newElement)
                    {
                        Location = new Point(5, 8),
                        Visible = true,
                        Name = newElement.IdAlarm.ToString(),
                        //Width = dynamicFlowLayoutPanel.Width - ((ListAlarm.Count < 3) ? 8 : 30)
                        Width = dynamicFlowLayoutPanel.ClientSize.Width - 30
                    };
                    iCard.AlarmDiagnostic += ICard_AlarmDiagnostic;
                    iCard.AlarmGeoLocation += ICard_AlarmGeoLocation;
                    iCard.AlarmCancelar += ICard_AlarmCancelar;
                    iCard.AlarmShowGroupLive += ICard_ShowGroupLive;

                    _syncListAsync[newElement.IdAlarm] = iCard;
                    dynamicFlowLayoutPanel.Controls.Add(iCard);

                    // Recorto cadena si excede el largo de la card
                    TruncateMessageAndSetTooltip(iCard, (ListAlarm.Count < 3));
                }

                // Actualizar el orden de los controles
                var ordered = ListAlarm.OrderByDescending(x => x.IdAlarm).ToList();
                for (int idx = 0; idx < ordered.Count; idx++)
                {
                    var id = ordered[idx].IdAlarm;
                    if (_syncListAsync.TryGetValue(id, out var card))
                    {
                        dynamicFlowLayoutPanel.Controls.SetChildIndex(card, idx);
                    }
                }

                // Actualizar imágenes y grupos en segundo plano
                _ = Task.Run(async () =>
                {
                    // Verificar si fue disposed antes de ejecutar
                    if (this.IsDisposed || this.Disposing || MainView == null)
                        return;

                    try
                    {
                        var result = await GetSnapshot(ListAlarm.Select(x => x.IdAlarm).ToList());
                        if (result != null)
                            InsertAlarmImage(result);
                        else
                            Logger.Log("Image not available", LogPriority.Warning);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"SyncList GetSnapshot Task Exception: {ex.Message}", LogPriority.Warning);
                    }
                });

                _ = System.Threading.Tasks.Task.Run(() =>
                {
                    // Verificar si fue disposed antes de ejecutar
                    if (this.IsDisposed || this.Disposing || MainView == null)
                        return;

                    try
                    {
                        if (_syncListAsync != null)
                            DrawAlarmAssignedObjectGroup();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"SyncList DrawAlarmAssignedObjectGroup Task Exception: {ex.Message}", LogPriority.Warning);
                    }
                });
            }
            catch (Exception e)
            {
                Logger.Log("UpdateUIAfterSync: " + e.Message, LogPriority.Fatal);
            }
        }

        public void ItemAlarmHeaderDispose()
        {
            if (this.dynamicFlowLayoutPanel != null && !this.dynamicFlowLayoutPanel.IsDisposed)
            {
                try
                {
                    foreach (Control t in this.dynamicFlowLayoutPanel.Controls)
                    {
                        if (t is ItemAlarmHeader)
                        {
                            (t as ItemAlarmHeader).AlarmDiagnostic -= ICard_AlarmDiagnostic;
                            (t as ItemAlarmHeader).AlarmGeoLocation -= ICard_AlarmGeoLocation;
                            (t as ItemAlarmHeader).AlarmShowGroupLive -= ICard_ShowGroupLive;
                            (t as ItemAlarmHeader).AlarmCancelar -= ICard_AlarmCancelar;
                            (t as ItemAlarmHeader).Controls.Clear();
                            (t as ItemAlarmHeader).CustomDispose();
                        }
                    }
                    this._syncListAsync.Clear();
                    this.dynamicFlowLayoutPanel.Controls.Clear();
                    this.dynamicFlowLayoutPanel.Dispose();
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }

        private void TruncateMessageAndSetTooltip(ItemAlarmHeader iCard, bool haveScroll)
        {
            var label = iCard.Message;
            if (label.Text.Length >= 35 && !haveScroll)
            {
                var originalText = label.Text;
                label.Text = $"{originalText.Substring(0, 31)} ...";
                iCard.SetMessageTooltip(originalText);
            }
            else if (label.Text.Length >= 37 && haveScroll)
            {
                var originalText = label.Text;
                label.Text = $"{originalText.Substring(0, 35)} ...";
                iCard.SetMessageTooltip(originalText);
            }
        }

        private void ICard_AlarmDiagnostic(object sender, CardDto itemCard)
        {
            this.ButtonClicked?.Invoke(sender, 3);
            MainView.GoToAlarmFromMenu(itemCard);
        }

        private void ICard_AlarmGeoLocation(object sender, CardDto itemCard)
        {
            var element = new SidebarElementDTO()
            {
                DeviceType = ElementType.Geolocation_Alarm,
                ElementId = itemCard.IdAlarm,
                Name = itemCard.DeviceName,
                RecorderName = itemCard.Type + " " + itemCard.Time.ToString(),
                GroupName = "",
                Key = Guid.NewGuid(),
                DeviceTypeStr = "KPI",
                SiteId = itemCard.IdSite,
                ShowDvfId = true
            };
            MainView.GoToLiveFromMenu(itemCard, element);
        }

        private void ICard_ShowGroupLive(object sender, CardDto itemCard)
        {
            MainView.GoToLiveFromMenu(null, null, itemCard.ObjectGroupId);
        }

        private async void ICard_AlarmCancelar(object sender, CardDto itemcard)
        {
            bool aceptar = false;
            string comments = string.Empty;
            using (DiscardComments us = new DiscardComments())
            using (Form form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                form.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
                form.ClientSize = new System.Drawing.Size(us.Width, us.Height);
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                us.SetLabelDiscard(Resources.Motive);
                form.Controls.Add(us);
                form.TopMost = true;
                form.ShowDialog();
                aceptar = us.Aceptar == true;
                comments = us.Comments;
            }

            if (aceptar)
            {
                try
                {
                    Logger.Log("ICard_AlarmCancelar Entered", LogPriority.Information);
                    var alarm = await GetAlarm(itemcard.IdAlarm);
                    alarm.Comments = comments;
                    Discard(alarm);
                    DeleteAlarmDiagnosticated(alarm.Id, sender, true);
                    var control = this.Controls.Find($"{alarm.ObjectType}{alarm.Id}", false);
                    if (control.Any())
                    {
                        control.FirstOrDefault().Dispose();
                        this.Controls.Remove(control.FirstOrDefault());
                    }
                    MainView.AddAudit(new AuditDTO() { CodeAction = AuditAction.DISCARD_ALARMS.ToString(), Param01 = itemcard.IdAlarm.ToString() });
                }
                catch (Exception ex)
                {
                    Logger.Log("Excepcion ICard_AlarmCancelar " + ex.Message, LogPriority.Fatal);
                    _notification.Show("Operation Fail , Error: " + ex.Message, null);
                }
                Logger.Log("ICard_AlarmCancelar Header", LogPriority.Information);
            }
        }

        private void DeleteAlarmDiagnosticated(int idAlarm, object sender, bool isMyself = false)
        {
            if (idAlarm == 0)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DeleteAlarmDiagnosticated(idAlarm, sender, isMyself);
                });
                return;
            }
            try
            {
                Logger.Log("DeleteAlarmDiagnosticated Entered", LogPriority.Information);
                if (isMyself)
                {// ejecucion local
                    Logger.Log("DeleteAlarmDiagnosticated Local AlarmId: " + idAlarm, LogPriority.Information);

                    // Agregar a la lista de alarmas eliminadas por mí ANTES de cualquier operación
                    // para evitar race condition con el echo de SignalR
                    this._alarmsRemoveByMe.Add(idAlarm);

                    var elements = this.ListAlarm.Where(x => x.IdAlarm == idAlarm).FirstOrDefault();
                    if (elements != null)
                    {
                        this.dynamicFlowLayoutPanel.Controls.RemoveByKey(elements.IdAlarm.ToString());
                        _syncListAsync.TryRemove(elements.IdAlarm, out _);
                    }
                    // Elimino de la lista
                    this.ListAlarm.Remove(elements);

                    // NO llamar a SyncList() aquí - el sync se hace cuando llega el echo de SignalR
                    // Esto evita race condition donde el servidor devuelve la alarma antes de procesarla

                    this.ButtonClicked?.Invoke(sender, elements);
                }
                else
                {//echo del signalR
                    bool wasRemovedByMe = this._alarmsRemoveByMe.Contains(idAlarm);
                    if (wasRemovedByMe)
                    {
                        // Echo de mi propia eliminación - solo limpiar el flag
                        this._alarmsRemoveByMe.Remove(idAlarm);
                        Logger.Log("DeleteAlarmDiagnosticated Echo of my own removal AlarmId: " + idAlarm, LogPriority.Information);
                    }
                    else
                    {
                        Logger.Log("DeleteAlarmDiagnosticated SignalR Echo (other user) AlarmId: " + idAlarm, LogPriority.Information);

                        // Busco la alarma de la lista
                        var elements = this.ListAlarm.Where(x => x.IdAlarm == idAlarm).FirstOrDefault();
                        if (elements != null)
                        {
                            this.dynamicFlowLayoutPanel.Controls.RemoveByKey(elements.IdAlarm.ToString());
                            _syncListAsync.TryRemove(elements.IdAlarm, out _);

                            // Elimino de la lista
                            this.ListAlarm.Remove(elements);
                        }
                        this.ButtonClicked?.Invoke(sender, elements);
                    }

                    // Sincronizar SIEMPRE para llenar el hueco (ya sea mi eliminación o de otro usuario)
                    SyncList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Excepcion DeleteAlarmDiagnosticated " + ex.Message, LogPriority.Fatal);
                //Logger.Log(ex);
            }
            Logger.Log("DeleteAlarmDiagnosticated Leave", LogPriority.Information);
        }


        private void InsertAlarmImage(Dictionary<int, string> resultImages)
        {
            try
            {
                //lock (_lockAlamsSync)
                //{
                foreach (var it in resultImages)
                {
                    if (this._syncListAsync.Keys.Contains(it.Key))
                    {
                        this._syncListAsync[it.Key].SetImage(it.Value);
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                Logger.Log("Excepcion InsertAlarmImage " + ex.Message, LogPriority.Fatal);
                //Logger.Log(ex);
            }
        }

        private void DrawAlarmAssignedObjectGroup()
        {
            // Verificar que tenemos acceso a MainView y sus datos
            if (MainView == null || MainView.User == null || string.IsNullOrEmpty(MainView.UserToken))
                return;

            try
            {
                var userId = (int)MainView.User.UserId;
                var userToken = MainView.UserToken;

                // Ejecutar las 3 llamadas HTTP en paralelo con Task.WhenAll
                // NOTA: Parallel.Invoke con lambdas async NO espera las tareas (convierte a async void),
                // causando que las variables queden null y se produzca NullReferenceException.
                List<int> carouselesIds = new List<int>();
                List<int> groupsLiveIds = new List<int>();
                List<int> groupsPlaybackIds = new List<int>();

                var taskCarouseles = Task.Run(async () =>
                {
                    try { carouselesIds = (await Vmon5Service.GetsCarousel(userToken))?.Select(x => x.Id).ToList() ?? new List<int>(); }
                    catch (Exception ex) { Logger.Log($"GetsCarousel failed: {ex.Message}", LogPriority.Warning); carouselesIds = new List<int>(); }
                });
                var taskGroupsLive = Task.Run(async () =>
                {
                    try { groupsLiveIds = (await Vmon5Service.GetObjectsGroup(userToken, userId, (int)GroupType.LIVE))?.Select(x => x.Id).ToList() ?? new List<int>(); }
                    catch (Exception ex) { Logger.Log($"GetObjectsGroup LIVE failed: {ex.Message}", LogPriority.Warning); groupsLiveIds = new List<int>(); }
                });
                var taskGroupsPlayback = Task.Run(async () =>
                {
                    try { groupsPlaybackIds = (await Vmon5Service.GetObjectsGroup(userToken, userId, (int)GroupType.PLAYBACK))?.Select(x => x.Id).ToList() ?? new List<int>(); }
                    catch (Exception ex) { Logger.Log($"GetObjectsGroup PLAYBACK failed: {ex.Message}", LogPriority.Warning); groupsPlaybackIds = new List<int>(); }
                });

                Task.WaitAll(taskCarouseles, taskGroupsLive, taskGroupsPlayback);

                // Verificar nuevamente después de las llamadas HTTP (pudo haber sido disposed)
                if (this.IsDisposed || this.Disposing || MainView == null)
                    return;

                var allGroupIds = carouselesIds.Concat(groupsLiveIds).Concat(groupsPlaybackIds).ToList();

                // Verificar que _syncListAsync no fue limpiado
                if (_syncListAsync == null)
                    return;

                // Copiar las keys para evitar modificación durante iteración
                var keys = _syncListAsync.Keys.ToList();
                foreach (var key in keys)
                {
                    if (_syncListAsync.TryGetValue(key, out var item))
                    {
                        item.DrawAlarmAssignedObjectGroup(allGroupIds);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"DrawAlarmAssignedObjectGroup Exception: {ex.Message}", LogPriority.Warning);
            }
        }

        private FilterDTO GetFilter()
        {
            int take = Convert.ToInt32(WorkFolderUtils.GetUserSettings("TakeAlarmsQuickView", true));
            var filter = new FilterDTO
            {
                AlarmType = GetTypesAlarms(),
                Site = 0,
                Page = 1,
                Take = take
            };
            return filter;
        }

        public List<string> GetTypesAlarms()
        {
            List<string> lst = new List<string>();

            Type t = typeof(AlarmType);
            foreach (var item in t.GetMembers().Where(m => m.GetCustomAttribute(typeof(AlarmTypeOf)) != null))
            {
                var itemEnum = item.GetCustomAttribute<DisplayAttribute>();
                AlarmTypeOf subAttr = (AlarmTypeOf)item.GetCustomAttribute(typeof(AlarmTypeOf));
                GroupAlarmType groupAlarmType = subAttr.AlarmType;
                string authorization = groupAlarmType.GetDescription();

                if (_appAuthorization.Exist(authorization))
                {
                    lst.Add(item.Name);
                }
            }
            return lst;
        }

        public async Task<List<CardDto>> GetData(FilterDTO filter, bool mapMode = false)
        {
            if (MainView == null || string.IsNullOrEmpty(MainView.UserToken))
                return new List<CardDto>();
            return await this._alarmService.Get(filter, MainView.UserToken, mapMode);
        }

        public async Task<Dictionary<int, string>> GetSnapshot(List<int> idAlarms)
        {
            // Validación temprana: si no hay alarmas, retornar inmediatamente
            if (idAlarms == null || idAlarms.Count == 0)
                return new Dictionary<int, string>();

            // Verificar que MainView existe
            if (MainView == null || string.IsNullOrEmpty(MainView.UserToken))
                return new Dictionary<int, string>();
            try
            {
                Logger.Log($"GetSnapshots: Requesting {idAlarms.Count} snapshots", LogPriority.Information);

                bool alarmSnapshot = false;
                try
                {
                    alarmSnapshot = _config.AppSettings.Settings["AlarmSnapshot"] != null
                        && bool.Parse(_config.AppSettings.Settings["AlarmSnapshot"].Value);
                }
                catch
                {
                    // Si falla la lectura de config, usar false
                }

                if (alarmSnapshot)
                {
                    // Llamar al servicio solo si está habilitado
                    var result = await this._alarmService.GetSnapshot(MainView.UserToken, idAlarms);
                    return result ?? new Dictionary<int, string>();
                }
                else
                {
                    // Usar imagen por defecto - más eficiente con capacidad inicial
                    var result = new Dictionary<int, string>(idAlarms.Count);
                    foreach (int idAlarm in idAlarms)
                    {
                        result[idAlarm] = DEFAULT_ALARM_IMAGE;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"GetSnapshots Exception: {ex.Message}", LogPriority.Warning);
                return new Dictionary<int, string>();
            }
        }

        public Task<AlarmDTO> GetAlarm(int idAlarm)
        {
            return this._alarmService.Get(MainView.UserToken, idAlarm);
        }

        public void Discard(AlarmDTO alarm)
        {
            alarm.AttendedBy = (int)MainView.User.UserId;
            alarm.Level = AlarmLevels.CriticalDiscard;
            this._alarmService.Discard(MainView.UserToken, alarm);
        }

        public async Task<bool> DiscardAllAlarms(DiscardAllAlarms obj)
        {
            try
            {
                return await this._alarmService.DiscardAllAlarms(MainView.UserToken, obj);
            }
            catch (Exception ex)
            {
                Logger.Log(" DiscardAllAlarms Exception " + ex.Message, LogPriority.Fatal);
                //Logger.Log(ex);
                return false;
            }
        }

        private void AlarmOptionsControl_Resize(object sender, EventArgs e)
        {
            var main = this.Bounds;
            try
            {
                //var width = this.Width;
                //pnlHeader.Width = width;
                //pnlSilence.Width = width;
                //pnlHeader.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1M), 2)); 
                //pnlFooter.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.15M), 2));
                //Switch.Location = new Point(pnlSilence.Right - (Switch.Width + 10), Switch.Location.Y);
                //lblTextToggle.Location = new Point(Switch.Left - (lblTextToggle.Width + 10), lblTextToggle.Location.Y);
                //btnShowAll.Location = new Point(pnlSilence.Width - (btnShowAll.Width + 10), btnShowAll.Location.Y);
                //btnCancelAll.Location = new Point(btnShowAll.Left - (btnCancelAll.Width + 10), btnCancelAll.Location.Y);
                //CenterControlInParent(pnlHeaderTitle, Label);
            }
            catch (Exception) { }

        }

        public void SetTextButtons()
        {
            var appAuthorization = Locator.Current.GetService<IAppAuthorization>();

            _buttonShowAll.Text = Resources.ViewAll;
            _labelSilenceAlarm.Text = Resources.TextToggle;
            if (appAuthorization != null)
            {
                if (appAuthorization.Exist("auth.app.apps.alarm.showAlarm") || appAuthorization.Exist("auth.app.alarms.receiveAlarms"))
                {
                    _buttonShowAll.Visible = true;
                }
                else
                {
                    _buttonShowAll.Visible = false;
                    _panelSilence.Location = new Point(0, 4);
                    this.Height = 48;
                }
            }
            this._label.Text = Resources.TitleAlarm;
            LoadAlarm = true;
        }

        private void ButtonSilent_Click(object sender, EventArgs e)
        {
            this.ButtonClicked?.Invoke(sender, 2);
        }

        private void ButtonAlarms_Click(object sender, EventArgs e)
        {
            this.ButtonClicked?.Invoke(sender, 1);
        }

        private void ButtonShowAll_Click(object sender, EventArgs e)
        {
            this.ButtonClicked?.Invoke(sender, 1);
        }
        private async void ButtonCancelAll_Click(object sender, EventArgs e)
        {
            bool aceptar = false;
            string comments = null;
            using (DiscardComments us = new DiscardComments())
            using (Form form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                form.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
                form.ClientSize = new System.Drawing.Size(us.Width, us.Height);
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                us.SetMessage(Resources.Motive);
                form.Controls.Add(us);
                form.TopMost = true;
                form.ShowDialog();
                aceptar = us.Aceptar == true;
                comments = us.Comments;
            }

            if (aceptar)
            {
                var alarm = ListAlarm.OrderByDescending(x => x.IdAlarm).FirstOrDefault();
                if (alarm != null)
                {
                    DiscardAllAlarms discard = new DiscardAllAlarms()
                    {
                        DicardFromAlarmId = alarm.IdAlarm,
                        Message = comments
                    };
                    Logger.Log(string.Format("Sending Discard all from {0} ", discard.DicardFromAlarmId), LogPriority.Information);
                    if (await DiscardAllAlarms(discard))
                    {
                        this.ButtonClicked?.Invoke(sender, discard);
                        SyncList();
                    }
                    else
                    {
                        _notification.Show("Error al eliminar todas las alarmas", null);
                    }
                }
                else
                {
                    Logger.Log("No alarm ready to discard");
                }
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            this._switch.Value = !this._switch.Value;
            this.ButtonClicked?.Invoke(sender, 2);
        }

        private void Switch_Click(object sender, EventArgs e)
        {
            this.ButtonClicked?.Invoke(sender, 2);
        }

        public void SetValue(bool value)
        {
            this._switch.Value = value;
        }

        public static void CenterControlInParent(Control Parent, Control Child)
        {
            int x = 0;
            x = (Parent.Width / 2) - (Child.Width / 2);
            Child.Location = new System.Drawing.Point(x, Child.Location.Y);
        }

        public static void CenterXY(Control Parent, Control Child)
        {
            int x = 0;
            int y = 0;
            x = (Parent.Width / 2) - (Child.Width / 2);
            y = (Parent.Height / 2) - (Child.Height / 2);
            Child.Location = new System.Drawing.Point(x, y);
        }
    }
}
