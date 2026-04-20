using Elipgo.SmartClient.Drivers.Vrec;
using System;
using System.IO;

namespace H264Decoder
{
    public class Decoder_H264 : IDisposable
    {
        private StreamCoders.Decoder.H264Decoder vdec;

        private bool isInitialized;

        private bool _disposed;


        public Decoder_H264()
        {
            // Create new decoder
            vdec = new StreamCoders.Decoder.H264Decoder();
            isInitialized = false;
            _disposed = false;

        }

        public Frame process(Frame frame)
        {
            // Verifica que el codec del frame sea el adecuado
            if (frame.codec.CompareTo(VideoCodecType.H264.ToString()) != 0)
                return null;

            if (!(frame.dataStream.Length > 0))
                return null;

            byte[] encBuffer = new byte[frame.dataStream.Length];
            frame.dataStream.Position = 0;


            int readLength = frame.dataStream.Read(encBuffer, 0, (int)frame.dataStream.Length);

            if (!isInitialized)
            {
                if (!frame.keyFrame)
                    return null;

                if (frame.Configuration != null)
                {
                    vdec.Init(frame.Configuration);
                }
                else
                {
                    byte[] configheader = new byte[32];
                    Buffer.BlockCopy(encBuffer, 0, configheader, 0, configheader.Length);
                    vdec.Init(configheader);
                    //vdec.Init(encBuffer);
                }
                isInitialized = true;
            }

            byte[] decBuffer = null;

            decBuffer = vdec.Decode(encBuffer);

            //return null;

            StreamCoders.Decoder.DecodedFrameInfo dfi = vdec.GetLastFrameInfo();

            if (dfi.IsError)
                return null;

            if (dfi.FrameType == StreamCoders.FrameTypes.NO_FRAME)
                return null;

            if (decBuffer == null)
                return null;

            MemoryStream rawStream = new MemoryStream();
            rawStream.Write(decBuffer, 0, decBuffer.Length);
            rawStream.Position = 0;

            Frame retFrame = new Frame(rawStream,
                VideoCodecType.RawBGR.ToString(),
                true);

            // Copiar propiedades 
            retFrame.fps = frame.fps;
            retFrame.label = frame.label;
            retFrame.text = frame.text;
            retFrame.UTCDateTime = frame.UTCDateTime;
            retFrame.width = vdec.InputWidth;
            retFrame.height = vdec.InputHeight;

            encBuffer = null;
            decBuffer = null;
            dfi = null;

            return retFrame;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (this.vdec != null)
                        this.vdec.Dispose();

                    this.vdec = null;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
