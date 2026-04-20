using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers.PlayerService;
using Elipgo.SmartClient.Drivers.Vrec.DVRPlay;
using Microsoft.Win32;
using StreamCoders;
using StreamCoders.Decoder;
using StreamCoders.Encoder;
using StreamCoders.Wave;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DVRPlay
{
    #region Enums

    /// <summary>
    /// Estado del cliente de reproducción de video.
    /// </summary>
    public enum ClientStatus
    {
        /// <summary>
        /// No hay video a reproducir.
        /// </summary>
        Close,
        /// <summary>
        /// Reproducción detenida.
        /// </summary>
        Stop,
        /// <summary>
        /// Reproducción en curso.
        /// </summary>
        Play,
        /// <summary>
        /// Reproducción pausada.
        /// </summary>
        Pause
    };

    #endregion

    #region Delegates

    /// <summary>
    /// Manejador de evento de adquisición de nuevo cuadro de video.
    /// </summary>
    public delegate void newFrameHandler(byte[] data, int length);

    /// <summary>
    /// Manejador de evento de error.
    /// </summary>
    public delegate void errorHandler(string errorMsg);

    /// <summary>
    /// Manejador de evento de stop
    /// </summary>
    /// <param name="errorMsg"></param>
    public delegate void stopHandler();

    /// <summary>
    /// Manejador de evento de reloadtimeline
    /// </summary>
    /// <param name="errorMsg"></param>
    public delegate void reloadTimelineHandler();

    #endregion

    /// <summary>
    /// Proceso cliente de reproducción de video. Se conecta a un servidor y adquiere
    /// cuadros del mismo, a partir de una fecha/hora solicitada.
    /// </summary>
    public class VideoClient
    {
        #region Enums

        enum FrameHeader { Index, Length, Rate, Timestamp, Label, Text, Paused, Motion, IsVideoFrame, RequestTimeLine };

        #endregion

        #region Events

        /// <summary>
        /// Evento disparado cuando se recibe un nuevo cuadro de video.
        /// </summary>
        public event newFrameHandler OnNewFrame = null;

        public event reloadTimelineHandler OnReloadTimeline = null;
        /// <summary>
        /// Evento disparado cuando se produce un error.
        /// </summary>
        public event errorHandler OnError = null;

        public event stopHandler OnStop = null;

        #endregion

        #region Attributes

        private PlayerService playerService;
        private Thread videoThread;
        private int clientId = -1;
        private bool stopped;
        private TcpClient client;
        private int tcpPortTransfer = -1;
        private int connectionTimeout = 30000;
        private int frameCount = 0;
        private int currPosition = 0;
        private float currFrameRate = 0;
        private DateTime currTimestamp;
        private String currLabel = "";
        private bool motionDetection = false;
        private ClientStatus status;
        private int skipRate;
        private float connectionFrameRate = 0;
        private String cameraName = "";
        private byte[] timeLineVideo = null;
        private string hostProxy = string.Empty;
        private int portProxy = 0;
        private AacAccessUnitTool aacAuTool = new AacAccessUnitTool();
        private AACDecoder audioDecoder;
        private SpeechAudioDecoder audioPCMDecoder;
        private WaveOutput waveOut;
        private bool smarthSearch = false;
        private int bitPerSample = 16;
        private int audioChannels = 1;
        private int sampleRate = 8000;

        #endregion

        #region Constructors

        /// <summary>
        /// Instancia un nuevo proceso cliente de reproducción de video.
        /// </summary>
        public VideoClient()
        {
            status = ClientStatus.Close;
        }

        #endregion

        #region Private Methods

        private PlayerService GetPlayerServiceInstance()
        {
            if (this.playerService == null)
            {
                this.playerService = new PlayerService();

                // el servicio guarda estado de sesión, por lo cual debe crearse un contenedor de cookies para el proxy
                this.playerService.CookieContainer = new CookieContainer();
            }
            return this.playerService;
        }

        private String ReadLine()
        {
            // lee bytes del socket cliente hasta completar una linea, hasta que la 
            // cantidad de bytes leidos sea 'maxLength' (en cuyo caso levanta error)
            // o hasta que no haya mas datos para leer (en cuyo caso retorna null)
            // (linea = secuencia de bytes terminados en "\n" ó "\r\n" )
            const int maxLength = 256;
            byte[] line = new byte[maxLength];
            byte[] line_ptr = line;
            int length = 0;
            //fixed( byte* line_ptr = line )
            {
                while (length < maxLength)
                {
                    int b = client.GetStream().ReadByte();
                    if (b < 0)
                    {
                        return null;
                    }

                    if (b == '\n')
                    {
                        return Encoding.ASCII.GetString(line, 0, length);
                    }
                    else if (b == '\r')
                    {
                        b = client.GetStream().ReadByte();
                        if (b < 0)
                        {
                            return null;
                        }
                        else if (b == '\n')
                        {
                            return Encoding.ASCII.GetString(line, 0, length);
                        }
                        else
                        {
                            line_ptr[length++] = (byte)'\r';
                            if (length < maxLength)
                            {
                                line_ptr[length++] = (byte)b;
                            }
                        }
                    }
                    else
                    {
                        line_ptr[length++] = (byte)b;
                    }
                }
                throw new Exception("max line length exceeded");
            }
        }

        /// <summary>
        /// Rutina para detectar el proxy
        /// </summary>
        /// <param name="proxyAddress"></param>
        /// <param name="proxyPort"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        private bool ProxyEnabled(ref string proxyAddress, ref int proxyPort, string host)
        {
            try
            {
                //check for status registry en la clase:
                RegistryKey key = null;
                proxyAddress = null;
                proxyPort = 0;

                // HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\
                key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");

                if (key != null)
                {
                    //si esta habilitado el proxy entonces devolver el host y port.
                    //"ProxyEnable"=dword:00000001
                    int proxyEnable = (int)key.GetValue("ProxyEnable");

                    if (proxyEnable == 1)
                    {
                        string proxyServer = (string)key.GetValue("ProxyServer");
                        string proxyOverride = (string)key.GetValue("ProxyOverride");

                        //si esta habilitado el proxy entonces devolver el host y port.				
                        //"ProxyServer"="192.168.200.43:8051"				
                        string[] proxyServerParse = proxyServer.Split(':');
                        if (proxyServerParse != null && proxyServerParse.Length == 2)
                        {
                            proxyAddress = proxyServerParse[0];
                            proxyPort = int.Parse(proxyServerParse[1]);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void Run()
        {
            try
            {
                String[] headerPatterns = {
                                          "Frame-Index:\\s*(\\d+)",
                                          "Frame-Length:\\s*(\\d+)",
                                          "Frame-Rate:\\s*([\\d,]+)",
                                          "Frame-Timestamp:\\s*(.+)",
                                          "Frame-Label:\\s*(.+)",
                                          "Frame-Text:\\s*(.+)",
                                          "Paused:\\s*(.+)",
                                          "Motion-Detection:\\s*(.+)",
                                          "Frame-IsVideoFrame:\\s*(.+)",
                                          "RequestTimeLine:\\s*(.+)"
                                      };

                String[] headerValues = new String[20];

                DateTimeFormatInfo timestampFormat = new DateTimeFormatInfo();
                timestampFormat.FullDateTimePattern = "yyyyMMddHHmmssfff";

                // variables para cálculo de cuadros por segundo adquiridos
                connectionFrameRate = 0;
                int frameCount = 0;
                DateTime startDateTime = DateTime.Now;

                Boolean first = false;

                while (true)
                {
                    if (!stopped)
                    {
                        String line = ReadLine();
                        if (line == null)
                        {
                            continue;
                        }
                        else if (line == "--frameboundary")
                        {
                            // leer headers subsiguientes
                            for (int i = 0; i < headerValues.Length; i++)
                            {
                                headerValues[i] = "";
                            }

                            line = ReadLine();
                            while (line != null && line != "")
                            {
                                for (int i = 0; i < headerPatterns.Length; i++)
                                {
                                    Match m = Regex.Match(line, headerPatterns[i]);
                                    if (m.Success)
                                    {
                                        headerValues[i] = m.Groups[1].Value;
                                        break;
                                    }
                                }
                                line = ReadLine();
                            }
                            if (line == null)
                            {
                                throw new Exception("no data (header)");
                            }
                        }

                        if (String.Compare(headerValues[(int)FrameHeader.Paused], "true", true) == 0)
                        {
                            status = ClientStatus.Pause;

                            if (waveOut != null)
                            {
                                waveOut.Pause();
                            }
                            //LoggerClient.Logger.write( _ClassName.VideoClient, LoggerEntryType.Information, LoggerCategory.Normal, "Pause Received!");
                        }
                        else
                        {
                            status = ClientStatus.Play;

                            if (waveOut != null)
                            {
                                waveOut.Resume();
                            }
                        }

                        motionDetection = String.Compare(headerValues[(int)FrameHeader.Motion], "true", true) == 0;

                        currPosition = Int32.Parse(headerValues[(int)FrameHeader.Index]);

                        try
                        {
                            currFrameRate = float.Parse(headerValues[(int)FrameHeader.Rate].Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            currFrameRate = 0;
                        }

                        currLabel = headerValues[(int)FrameHeader.Label];

                        try
                        {
                            currTimestamp = DateTime.ParseExact(headerValues[(int)FrameHeader.Timestamp], "F", timestampFormat);

                            if (!first)
                            {
                                first = true;
                                //previousPresentationTime = currTimestamp.TimeOfDay;

                            }

                        }
                        catch
                        {
                        }

                        bool isVideo = true;

                        try
                        {
                            if (!string.IsNullOrEmpty(headerValues[(int)FrameHeader.IsVideoFrame]) && bool.TryParse(headerValues[(int)FrameHeader.IsVideoFrame], out isVideo))
                            {
                            }
                        }
                        catch
                        {
                            isVideo = true;
                        }

                        if (String.Compare(headerValues[(int)FrameHeader.RequestTimeLine], "true", true) == 0)
                        {
                            OnReloadTimeline?.Invoke();
                        }

                        DateTime now = DateTime.Now;
                        if (frameCount == 0)
                        {
                            startDateTime = now;
                        }
                        else
                        {
                            float elapsed = (float)(now.Ticks - startDateTime.Ticks) / TimeSpan.TicksPerSecond;
                            connectionFrameRate = frameCount / elapsed;

                            if (Double.IsInfinity(connectionFrameRate) || Double.IsNaN(connectionFrameRate))
                            {
                                connectionFrameRate = 0;
                            }

                            // resetear contador de cuadros por segundo cada 10 segundos
                            if (elapsed > 10)
                            {
                                startDateTime = DateTime.Now;
                                frameCount = 0;
                            }
                        }
                        frameCount++;

                        int length = Int32.Parse(headerValues[(int)FrameHeader.Length]);


                        if (length > 0)
                        {
                            byte[] data = new byte[length];

                            int bytesRead = client.GetStream().Read(data, 0, length);
                            if (bytesRead == 0)
                            {
                                throw new Exception(Resources.VRecRemoteConnectionError);
                            }

                            while (bytesRead < length)
                            {
                                int c = client.GetStream().Read(data, bytesRead, length - bytesRead);

                                if (c == 0)
                                {
                                    throw new Exception(Resources.VRecRemoteConnectionError);
                                }

                                bytesRead += c;
                            }

                            if (isVideo)
                            {
                                OnNewFrame?.Invoke(data, length);
                            }
                            else
                            {
                                DoAudio(data);
                            }

                        }
                    }
                    else
                    {
                        Thread.Sleep(200);

                        status = ClientStatus.Stop;

                        OnStop?.Invoke();
                    }
                }
            }
            catch (ThreadAbortException te)
            {
                Logger.Log($"Message 4 {te.Message} StackTrace {te.StackTrace}");
                // entra aqui por cierre externo (se dispara desde el close)
                try
                {
                    client.GetStream().Close();
                    client.Close();
                }
                catch(Exception exe)
                {
                    Logger.Log($"Message 5 {exe.Message} StackTrace {exe.StackTrace}");
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Message 1 {e.Message} StackTrace {e.StackTrace}");
                // en caso de error cerrar la conexión 
                // lock para evitar conflicto con close
                lock (this)
                {
                    try
                    {
                        client.GetStream().Close();
                        client.Close();
                    }
                    catch (Exception ex1)
                    {
                        Logger.Log($"Message 2 {ex1.Message} StackTrace {ex1.StackTrace}");
                    }

                    try
                    {
                        videoThread = null;
                        clientId = -1;
                        status = ClientStatus.Close;

                        OnError?.Invoke(e.Message);
                    }
                    catch (Exception ex2)
                    {
                        Logger.Log($"Message 3 {ex2.Message} StackTrace {ex2.StackTrace}");
                    }
                }
            }
        }

        private void DoAudio(byte[] data)
        {
            lock (this)
            {
                if (skipRate != 1 || smarthSearch)
                {
                    if (waveOut != null)
                    {
                        waveOut.ResetDevice();
                        waveOut.CloseDevice();
                        waveOut = null;
                    }

                    return;
                }

                if (audioDecoder == null)
                {
                    sampleRate = 16000;
                    audioDecoder = new AACDecoder();
                    audioDecoder.Channels = audioChannels;
                    audioDecoder.BitsPerSample = bitPerSample;
                    audioDecoder.SampleRate = sampleRate;
                    audioDecoder.AudioSource = StreamSources.NETWORK_RTP;
                    if (!audioDecoder.Init("1408"))
                    {
                        audioDecoder = null;
                        return;
                    }
                }

                MediaBuffer<byte> ndata = aacAuTool.CreateAU(new MediaBuffer<byte>(data));

                byte[] decodedData = audioDecoder.DecodeAU(ndata.Buffer.Array);

                if (decodedData == null)
                {
                    if (audioPCMDecoder == null)
                    {
                        sampleRate = 8000;
                        audioPCMDecoder = new SpeechAudioDecoder(SpeechCodecs.G711U, 64000);
                        audioPCMDecoder.Channels = audioChannels;
                        audioPCMDecoder.BitsPerSample = bitPerSample;
                        audioPCMDecoder.SampleRate = sampleRate;
                        //audioPCMDecoder.Bitrate = 0;
                        if (!audioPCMDecoder.Init())
                        {
                            audioPCMDecoder = null;
                            return;
                        }
                    }
                    decodedData = audioPCMDecoder.Decode(data);
                }

                if (decodedData == null)
                {
                    return;
                }

                if (waveOut != null)
                {
                    waveOut.Enqueue(decodedData);
                    waveOut.UnprepareBuffers();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retorna la URL del servidor de video.
        /// </summary>
        /// <returns>URL del servidor de video.</returns>
        public String GetServerUrl()
        {
            return this.GetPlayerServiceInstance().Url;
        }

        /// <summary>
        /// Cambia la URL del servidor de video.
        /// </summary>
        /// <param name="url">URL del servidor de video.</param>
        public void SetServerUrl(String url)
        {
            this.GetPlayerServiceInstance().Url = url;
        }

        /// <summary>
        /// Cambia el puerto de transmision.
        /// </summary>
        /// <param name="port">Puerto servidor de video.</param>
        public void SetPortTransfer(int port)
        {
            tcpPortTransfer = port;
        }

        /// <summary>
        /// Cambia el timeout de la conexión.
        /// </summary>
        /// <param name="timeout">Período de espera en milisegundos.</param>
        public void SetConnectionTimeout(int timeout)
        {
            connectionTimeout = timeout;
        }

        /// <summary>
        /// Abre la conexión al servidor de reproducción de video, solicitando la 
        /// reproducción de una cámara y fecha/hora en particular.
        /// </summary>
        /// <param name="cameraId">Identificador de cámara</param>
        /// <param name="videoDateTime">Fecha/hora del video requerido, en formato 'YYYYMMDDHHMMSS'</param>
        /// <returns>True si la conexión  se abrió exitosamente, false en caso contrario.</returns>
        public bool Open(String cameraId, String videoDateTime)
        {
            if (status != ClientStatus.Close)
            {
                return false;
            }

            if (videoThread != null)
            {
                return false;
            }

            int portTransfer = 0;
            int frameCountVideo = 0;
            string nameCam = "";

            int videoTransferPort = 0;

            UriBuilder ubUrl = new UriBuilder(this.GetPlayerServiceInstance().Url);

            try
            {
                this.GetPlayerServiceInstance().Proxy = null;

                cameraName = "";
                DateTime dateTime = DateTime.ParseExact(videoDateTime, "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo);

                this.GetPlayerServiceInstance().RequestVideoPreParameter(cameraId, videoDateTime, out clientId, out portTransfer);

                if (clientId < 0)
                {
                    switch (clientId)
                    {
                        case -1:
                            throw new Exception(Resources.VRecMaximumClientNumbers);
                        case -2:
                            throw new Exception(Resources.VRecVideoNotExists);
                        case -3:
                            throw new Exception(Resources.VRecVideoEmpty);
                        default:
                            throw new Exception(Resources.VRecError);
                    }
                }

                if (tcpPortTransfer > 0)
                {
                    videoTransferPort = tcpPortTransfer;
                }
                else
                {
                    videoTransferPort = portTransfer;
                }

                //Step 02
                Uri uri = new Uri(this.GetPlayerServiceInstance().Url);

                client = new TcpClient();
                client.ReceiveTimeout = connectionTimeout;
                client.SendTimeout = connectionTimeout;

                //Step 03
                IPAddress hostAddress = DnsHelper.Resolve(uri.Host);
                client.Connect(hostAddress, videoTransferPort);


                BinaryWriter w = new BinaryWriter(client.GetStream());

                //GET /video?token=123412 HTTP/1.0\r\n
                //Host: Host:Port\r\n
                //Client-Id: 123412\r\n
                //\r\n

                //GET http://Host:Port/video?token=123412 HTTP/1.0\r\n
                //Host: Host:Port\r\n
                //Client-Id: 123412\r\n
                //\r\n

                Random r = new Random();
                string rndSeed = r.Next(1000, 100000).ToString();
                string requestMessage = string.Empty;

                requestMessage += "GET /video?token=" + rndSeed + " HTTP/1.0\r\n";

                requestMessage += "Host: " + uri.Host + ":" + videoTransferPort.ToString() + "\r\n";
                requestMessage += "Client-Id: " + clientId + "\r\n";
                requestMessage += "\r\n";
                w.Write(Encoding.ASCII.GetBytes(requestMessage));
                w.Flush();

                String response = ReadLine();

                //HTTP/1.0 200 OK\r\n
                //Content-Type: multipart/x-mixed-replace; boundary=--myboundary\r\n
                //\r\n

                if (!Regex.IsMatch(response, "HTTP/1\\.(0|1)\\s*200\\s*OK", RegexOptions.IgnoreCase))
                {
                    throw new Exception("connection failed");
                }

                String header = ReadLine();
                while (header != "")
                {
                    // aqui lee el header Content-Type...
                    header = ReadLine();
                }

                //	Factorizacion de consultas
                if (!this.GetPlayerServiceInstance().RequestVideoPostParameter(clientId, cameraId, out frameCountVideo, out this.timeLineVideo, out nameCam))
                {
                    return false;
                }

                frameCount = frameCountVideo;

                status = ClientStatus.Stop;
                skipRate = 1;

                cameraName = nameCam;

                return true;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);

                return false;
            }
        }

        /// <summary>
        /// Inicia el proceso de reproducción.
        /// </summary>
        public void Start()
        {
            if (status != ClientStatus.Stop && status != ClientStatus.Pause)
            {
                return;
            }

            if (clientId < 0)
            {
                return;
            }

            try
            {
                this.GetPlayerServiceInstance().StartVideo(clientId);
                stopped = false;

                if (videoThread == null)
                {
                    videoThread = new Thread(new ThreadStart(Run));
                    videoThread.Start();
                }

                motionDetection = false;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Detiene el proceso de reproducción y retorna a la posición inicial.
        /// </summary>
        public void Stop()
        {
            if (status != ClientStatus.Play && status != ClientStatus.Pause)
            {
                return;
            }

            if (clientId < 0)
            {
                return;
            }

            lock (this)
            {
                if (waveOut != null)
                {
                    waveOut.CloseDevice();
                    waveOut.Dispose();
                    waveOut = null;
                }

                if (audioDecoder != null)
                {
                    audioDecoder.Dispose();
                    audioDecoder = null;
                }

                if (audioPCMDecoder != null)
                {
                    audioPCMDecoder.Dispose();
                    audioPCMDecoder = null;
                }
            }

            try
            {
                this.GetPlayerServiceInstance().StopVideo(clientId);
                stopped = true;
                //currPosition = 0;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Congela la reproducción en la posición actual.
        /// </summary>
        public void Pause()
        {
            if (status != ClientStatus.Play)
            {
                return;
            }

            if (clientId < 0)
            {
                return;
            }

            try
            {
                this.GetPlayerServiceInstance().PauseVideo(clientId);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Finaliza la conexión con el servidor de reproducción de video.
        /// </summary>
        public void Close()
        {
            if (status == ClientStatus.Close)
            {
                return;
            }

            if (clientId < 0)
            {
                return;
            }

            try
            {
                // TODO : por algún motivo desconocido la llamada a thread.Abort() aquí
                // no cancela la ejecución del thread ->
                // cierra la conexión para forzar excepción y fin del thread
                // lock para evitar conflicto con fin de thread
                lock (this)
                {
                    if (videoThread != null)
                    {
                        videoThread.Abort();
                        videoThread = null;
                    }

                    this.GetPlayerServiceInstance().CloseVideo(clientId);

                    clientId = -1;
                    status = ClientStatus.Close;
                    cameraName = "";
                }
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Retorna la posición actual de cuadro en reproducción.
        /// </summary>
        /// <returns>Posición actual de cuadro en reproducción.</returns>
        public int GetPosition()
        {
            return currPosition;
        }

        /// <summary>
        /// Cambia la posición actual de cuadro en reproducción.
        /// </summary>
        /// <param name="position">Nueva posición de cuadro en reproducción.</param>
        public void SetPosition(int position)
        {
            if (clientId < 0)
            {
                return;
            }

            try
            {
                this.GetPlayerServiceInstance().SetVideoFramePosition(clientId, position);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Retorna la posición de reproducción actual (en tiempo).
        /// </summary>
        /// <returns>Posición de reproducción (en segundos respecto de hora de reproducción actual).</returns>
        public int GetTimeOffset()
        {
            DateTime ti = new DateTime(currTimestamp.Year, currTimestamp.Month, currTimestamp.Day, currTimestamp.Hour, 0, 0);
            TimeSpan offset = currTimestamp - ti;

            return (int)offset.TotalSeconds;
        }

        /// <summary>
        /// Cambiar posición de reproducción actual (en tiempo).
        /// </summary>
        /// <param name="seconds">Desplazamiento en segundos respecto de hora de reproducción actual.</param>
        public void SetTimeOffset(double seconds)
        {
            if (clientId < 0)
            {
                return;
            }

            try
            {
                DateTime ti = new DateTime(currTimestamp.Year, currTimestamp.Month, currTimestamp.Day, currTimestamp.Hour, 0, 0);
                this.GetPlayerServiceInstance().SetVideoTimePosition(clientId, ti.AddSeconds(seconds).ToString("yyyyMMddHHmmss"));
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Saltar a una posición absoluta de reproducción (en tiempo).
        /// </summary>
        /// <param name="seekDateTime">Fecha/hora de reproducción buscada.</param>
        public void Seek(DateTime seekDateTime)
        {
            if (clientId < 0)
            {
                return;
            }

            try
            {
                this.GetPlayerServiceInstance().SetVideoTimePosition(clientId, seekDateTime.ToString("yyyyMMddHHmmss"));

                if (waveOut != null)
                {
                    waveOut.ResetDevice();
                }
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Obtiene el factor de skip (salto de cuadros) de reproducción.
        /// </summary>
        /// <returns>Factor de skip.</returns>
        public int GetSkipRate()
        {
            return skipRate;
        }

        /// <summary>
        /// Cambia el factor de skip (salto de cuadros) de reproducción. 
        /// </summary>
        /// <param name="rate">Factor de skip. Es la cantidad de cuadros que deben
        /// saltearse durante la reproducción. Puede ser un número positivo (siendo 1 el
        /// factor de reproducción normal) o un número negativo (reproducción inversa).</param>
        public void SetSkipRate(int rate)
        {
            if (clientId < 0 || rate == 0)
            {
                return;
            }

            try
            {
                this.GetPlayerServiceInstance().SetVideoSkipRate(clientId, rate);
                skipRate = rate;
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
            }
        }

        /// <summary>
        /// Obtiene la cantidad de cuadros por segundo a la que fue grabado el
        /// cuadro actualmente en reproducción.
        /// </summary>
        /// <returns>FPS de cuadro actual.</returns>
        public float GetFrameRate()
        {
            return currFrameRate;
        }

        /// <summary>
        /// Obtiene la fecha/hora correspondiente al cuadro actualmente en 
        /// reproducción.
        /// </summary>
        /// <returns>Fecha/hora de cuadro actual.</returns>
        public DateTime GetTimestamp()
        {
            return currTimestamp;
        }

        /// <summary>
        /// Obtiene la etiqueta correspondiente al cuadro actualmente en 
        /// reproducción.
        /// </summary>
        /// <returns>Etiqueta del cuadro actual.</returns>
        public String GetLabel()
        {
            return currLabel;
        }

        /// <summary>
        /// Informa si el cuadro actualmente en reproducción contiene detección de movimiento.
        /// </summary>
        /// <returns>True si hubo detección de movimiento, false en caso contrario.</returns>
        public bool IsMotionDetection()
        {
            return motionDetection;
        }

        /// <summary>
        /// Retorna la cantidad de cuadros disponibles.
        /// </summary>
        /// <returns>Cantidad de cuadros disponibles.</returns>
        public int GetFrameCount()
        {
            return clientId < 0 ? 0 : frameCount;
        }

        /// <summary>
        /// Obtiene el estado del proceso de reproducción.
        /// </summary>
        /// <returns>Estado del proceso de reproducción.</returns>
        public ClientStatus GetStatus()
        {
            return status;
        }

        /// <summary>
        /// Obtiene la cantidad de cuadros por segundo a la que funciona la 
        /// conexión (valor menor o igual a los FPS de grabación).
        /// </summary>
        /// <returns>FPS de la conexión.</returns>
        public float GetConnectionFrameRate()
        {
            return connectionFrameRate;
        }

        /// <summary>
        /// Obtiene el nombre de la cámara en reproducción.
        /// </summary>
        /// <returns>Nombre de cámara.</returns>
        public String GetCameraName()
        {
            return cameraName;
        }

        /// <summary>
        /// Obtiene la linea de tiempo que indica las zonas de video/no-video/eventos.
        /// </summary>
        /// <returns>Linea de tiempo.</returns>
        public byte[] GetTimeline()
        {
            if (clientId < 0)
            {
                return null;
            }

            if (this.timeLineVideo != null)
            {
                return this.timeLineVideo;
            }
            else
            {
                return this.GetPlayerServiceInstance().GetVideoTimeline(clientId);
            }
        }

        /// <summary>
        /// Obtiene la linea de tiempo que indica las zonas de video/no-video/eventos.
        /// </summary>
        /// <returns>Linea de tiempo.</returns>
        public byte[] GetTimelineReload()
        {
            if (clientId < 0)
            {
                return null;
            }

            this.timeLineVideo = this.GetPlayerServiceInstance().GetVideoTimeline(clientId);

            return this.timeLineVideo;
        }

        /// <summary>
        /// Habilita/desahabilita el modo de "búsqueda inteligente" (detección de movimiento).
        /// La reproducción saltea cuadros hasta encontrar uno con detección de movimiento, 
        /// y queda en pausa.
        /// </summary>
        /// <param name="enable">Habilitar/deshabilitar modo de búsqueda inteligente.</param>
        public void SetSmartSearchMode(bool enable)
        {
            if (clientId < 0)
            {
                return;
            }

            this.smarthSearch = enable;
            this.GetPlayerServiceInstance().SetSmartSearchMode(clientId, enable);
        }

        /// <summary>
        /// Determina el area rectangular (ROI) de búsqueda inteligente dentro de la imagen.
        /// </summary>
        /// <param name="left">Origen del rectángulo desde el borde izquierdo de la imagen (porcentaje del ancho).</param>
        /// <param name="top">Origen del rectángulo desde el borde superior de la imagen (porcentaje del alto).</param>
        /// <param name="width">Ancho del rectángulo (porcentaje del ancho de la imagen).</param>
        /// <param name="height">Alto del rectángulo (porcentaje del alto de la imagen).</param>
        public void SetSmartSearchArea(int left, int top, int width, int height)
        {
            if (clientId < 0)
            {
                return;
            }

            this.GetPlayerServiceInstance().SetSmartSearchArea(clientId, left, top, width, height);
        }

        /// <summary>
        /// Determina los umbrales para búsqueda inteligente.
        /// </summary>
        /// <param name="delta">Umbral de diferencia de nivel por pixel.</param>
        /// <param name="vol">Umbral de volumen de movimiento global.</param>
        public void SetSmartSearchThresholds(int delta, int vol)
        {
            lock (this)
            {
                if (clientId < 0)
                {
                    return;
                }

                this.GetPlayerServiceInstance().SetSmartSearchThresholds(clientId, delta, vol);
            }
        }

        /// <summary>
        /// Determina si debe mostrarse el overlay de movimiento y el color del mismo.
        /// </summary>
        /// <param name="show">Mostrar overlay.</param>
        /// <param name="red">Nivel de rojo (0 a 255).</param>
        /// <param name="green">Nivel de verde (0 a 255).</param>
        /// <param name="blue">Nivel de azul (0 a 255).</param>
        public void SetSmartSearchOverlay(bool show, int red, int green, int blue)
        {
            lock (this)
            {
                if (clientId < 0)
                {
                    return;
                }

                this.GetPlayerServiceInstance().SetSmartSearchOverlay(clientId, show, red, green, blue);
            }
        }

        /// <summary>
        /// Realiza una búsqueda inteligente (detección de movimiento) offline sobre todos 
        /// los archivos de video dentro del rango horario dado.
        /// </summary>
        /// <param name="cameraId">Identificador de cámara.</param>
        /// <param name="start">Fecha/hora inicial.</param>
        /// <param name="end">Fecha/hora final.</param>
        /// <param name="deltaSeconds">Intervalo de búsqueda en segundos.</param>
        /// <param name="roiX">Origen del área de búsqueda desde el borde izquierdo de la imagen (porcentaje del ancho).</param>
        /// <param name="roiY">Origen del área de búsqueda desde el borde superior de la imagen (porcentaje del alto).</param>
        /// <param name="roiW">Ancho del área de búsqueda (porcentaje del ancho).</param>
        /// <param name="roiH">Alto del área de búsqueda (porcentaje del alto).</param>
        /// <param name="thDelta">Umbral de diferencia de nivel por pixel.</param>
        /// <param name="thVol">Umbral de volumen de movimiento global.</param>
        public void StartOfflineSmartSearch(String cameraId, DateTime start, DateTime end, int deltaSeconds, int roiX, int roiY, int roiW, int roiH, int thDelta, int thVol)
        {
            this.GetPlayerServiceInstance().StartOfflineSmartSearch(cameraId, start.ToString("yyyyMMddHHmmss"), end.ToString("yyyyMMddHHmmss"), deltaSeconds, roiX, roiY, roiW, roiH, thDelta, thVol);
        }

        /// <summary>
        /// Finalizar la búsqueda offline.
        /// </summary>
        public void AbortOfflineSmartSearch()
        {
            this.GetPlayerServiceInstance().AbortOfflineSmartSearch();
        }

        /// <summary>
        /// Obtiene los resultados parciales de la búsqueda offline.
        /// </summary>
        /// <returns>Lista de fecha/hora indicando los instantes donde se detectó movimiento.</returns>
        public DateTime[] GetOfflineSmartSearchResults()
        {
            string[] dateList = this.GetPlayerServiceInstance().GetOfflineSmartSearchResults();
            DateTime[] dateTimeList = new DateTime[dateList.Length];

            for (int i = 0; i < dateList.Length; i++)
            {
                dateTimeList[i] = DateTime.ParseExact(dateList[i], "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo);
            }

            return dateTimeList;
        }

        /// <summary>
        /// Indica si la búsqueda offline se completó.
        /// </summary>
        /// <returns>True si la búsqueda está completa, false en caso contrario.</returns>
        public bool IsOfflineSmartSearchComplete()
        {
            return this.GetPlayerServiceInstance().IsOfflineSmartSearchComplete();
        }

        /// <summary>
        /// Obtiene el porcentaje de progreso de la búsqueda offline.
        /// </summary>
        /// <returns>Porcentaje de progreso de la búsqueda (valor de 0 a 100).</returns>
        public int GetOfflineSmartSearchProgress()
        {
            return this.GetPlayerServiceInstance().GetOfflineSmartSearchProgress();
        }

        /// <summary>
        /// setAutoDetectProxy
        /// </summary>
        /// <param name="autoDetectProxy"></param>
        public void SetAutoDetectProxy(bool autoDetectProxy)
        {
            string autoDetectedProxyHost = string.Empty;
            int autoDetectedProxyPort = 0;
            bool isProxyEnabled = false;

            if (autoDetectProxy)
            {
                isProxyEnabled = ProxyEnabled(ref autoDetectedProxyHost, ref autoDetectedProxyPort, "");

                if (isProxyEnabled && autoDetectedProxyHost != string.Empty && autoDetectedProxyPort != 0)
                {
                    this.hostProxy = autoDetectedProxyHost;
                    this.portProxy = autoDetectedProxyPort;
                }
            }
            else
            {
                this.hostProxy = string.Empty;
                this.portProxy = 0;
            }
        }

        public bool OpenSound()
        {
            try
            {
                if (waveOut == null)
                {
                    waveOut = new WaveOutput();
                    waveOut.Volume = 100;
                    waveOut.BitsPerSample = bitPerSample;
                    waveOut.Channels = audioChannels;
                    waveOut.SampleRate = sampleRate;
                    if (waveOut.Init() == false)
                    {
                        throw new Exception(Resources.VRecAudioDeviceError);
                    }
                    if (waveOut.OpenDevice() == false)
                    {
                        throw new Exception(Resources.VRecAudioDeviceError);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                CloseSound();
                return false;
            }
        }

        public bool CloseSound()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.ResetDevice();
                    waveOut.CloseDevice();
                    waveOut = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
