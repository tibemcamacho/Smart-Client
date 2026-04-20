using System;
using System.IO;

namespace Elipgo.SmartClient.Drivers.Vrec
{

    /// <summary>
    /// Almacena un cuadro de video (imagen), opcionalmente con audio y sus datos asociados 
    /// (fecha/hora de adquisición, cuadros por segundo, etiqueta de evento y texto adicional).
    /// </summary>
    public class Frame : IDisposable
    {
        private MemoryStream _dataStream;
        private string _codec;
        private bool _keyFrame;
        private int _UTCTimeShift = 0;

        private int _width;
        private int _height;

        private DateTime _UTCDateTime;
        private double _fps;
        private string _label;
        private string _text;

        /// <summary>
        /// Indica si se llamo Dispose.
        /// </summary>
        private bool disposed = false;
        private byte[] _latestConfigurationFrame;

        public bool IsVideoFrame
        {
            get { return _codec != "aac"; }
        }

        public byte[] Configuration
        {
            get { return _latestConfigurationFrame; }
        }

        /// <summary>
        /// Crea una nueva instancia de la clase con los datos de video especificados,
        /// sin datos de audio.
        /// </summary>
        /// <param name="videoDataStream">Datos del cuadro de video.</param>
        /// <param name="videoCodec">Identificador de codec de video.</param>
        /// <param name="keyFrame">True si el cuadro es un "key frame", false en caso contrario.</param>
        public Frame(MemoryStream videoDataStream, string videoCodec, bool keyFrame)
        {
            _dataStream = videoDataStream;
            _codec = videoCodec;
            _keyFrame = keyFrame;

            _UTCDateTime = DateTime.UtcNow;
            _fps = 0;
            _label = "";
            _text = "";
        }

        public Frame(MemoryStream videoDataStream, string videoCodec, bool keyFrame, DateTime dTime)
        {
            _dataStream = videoDataStream;
            _codec = videoCodec;
            _keyFrame = keyFrame;

            _UTCDateTime = dTime;
            _fps = 0;
            _label = "";
            _text = "";
        }

        public Frame(MemoryStream audioDataStream, string audioCodec)
        {
            _dataStream = audioDataStream;
            _codec = audioCodec;
            _keyFrame = true;

            _UTCDateTime = DateTime.UtcNow;
            _fps = 0;
            _label = "";
            _text = "";
        }

        public Frame(MemoryStream audioDataStream, string audioCodec, DateTime dTime)
        {
            _dataStream = audioDataStream;
            _codec = audioCodec;
            _keyFrame = true;

            _UTCDateTime = dTime;
            _fps = 0;
            _label = "";
            _text = "";
        }

        public Frame(MemoryStream videoDataStream, string videoCodec, bool keyFrame, byte[] latestConfigurationFrame)
        {
            _latestConfigurationFrame = latestConfigurationFrame;
            _dataStream = videoDataStream;
            _codec = videoCodec;
            _keyFrame = keyFrame;

            _UTCDateTime = DateTime.UtcNow;
            _fps = 0;
            _label = "";
            _text = "";
        }



        /// <summary>
        /// Destructor
        /// </summary>
        ~Frame()
        {
            Dispose(true);
        }

        /// <summary>
        /// Libera los recursos utilizados 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implementación del dispose
        /// </summary>
        /// <param name="disposing">Bool de Estado</param>
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (this.dataStream != null)
                        {
                            this.dataStream.Dispose();
                            //this.videoDataStream.Close();
                        }
                        this._dataStream = null;
                    }
                    catch (Exception ex)
                    {
                        string pepo = ex.Message;
                    }
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Stream de datos de video.
        /// </summary>
        public MemoryStream dataStream
        {
            get
            {
                return _dataStream;
            }
            set
            {
                this._dataStream = value;
            }
        }

        /// <summary>
        /// Identificador de codec de video que utiliza el cuadro.
        /// </summary>
        public string codec
        {
            get
            {
                return _codec;
            }
        }

        /// <summary>
        /// Indica si el cuadro de video corresponde a un "key frame".
        /// </summary>
        public bool keyFrame
        {
            get
            {
                return _keyFrame;
            }
        }

        /// <summary>
        /// Ancho en pixels del cuadro de video
        /// </summary>
        public int width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Alto en pixels del cuadro de video
        /// </summary>
        public int height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Retorna la longitud en bytes del stream de datos de video.
        /// </summary>
        /// <returns>Longitud en bytes del stream de datos de video.</returns>
        public long getDataSize()
        {
            return dataStream != null ? dataStream.Length : 0;
        }

        /// <summary>
        /// Fecha/hora UTC del cuadro.
        /// </summary>
        public DateTime UTCDateTime
        {
            get
            {
                return _UTCDateTime;
            }
            set
            {
                _UTCDateTime = value;
            }
        }

        /// <summary>
        /// Diferencia entre hora local y hora UTC
        /// </summary>
        public int UTCTimeShift
        {
            get
            {
                return this._UTCTimeShift;
            }

            set
            {
                this._UTCTimeShift = value;
            }
        }

        /// <summary>
        /// Cantidad de cuadros por segundo de adquisición.
        /// </summary>
        public double fps
        {
            get
            {
                return _fps;
            }
            set
            {
                _fps = value;
            }
        }

        /// <summary>
        /// Etiqueta asociada al cuadro.
        /// </summary>
        public string label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        /// <summary>
        /// Texto asociado al cuadro.
        /// </summary>
        public string text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
    }
}
