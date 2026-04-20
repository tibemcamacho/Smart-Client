using StreamCoders;
using StreamCoders.Decoder;
using System;

namespace Elipgo.SmartClient.Drivers.Vrec.DVRPlay
{
    /// <summary>
    /// This simply encapsulates the SpeechDecoder class into a an AudioDecoder so we can use it interchangeably.
    /// </summary>
    internal class SpeechAudioDecoder : AudioDecoderBase
    {
        public SpeechAudioDecoder(SpeechCodecs codec, int bitrate)
        {
            dec = new SpeechDecoder();
            dec.SetCodec(codec);
            if (codec == SpeechCodecs.G711A || codec == SpeechCodecs.G711U)
                dec.Bitrate = bitrate;
            dec.StreamType = SpeechStreamType.STORAGE_FORMAT;
        }


        ~SpeechAudioDecoder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dec.Dispose();

                }
                disposed = true;
            }
        }


        private SpeechDecoder dec;
        //private int bps;
        //private int chans;
        //private int sr;
        private bool disposed = false;

        #region AudioDecoderBase Members

        public int BitsPerSample
        {
            get; set;
        }

        public int Bitrate
        {
            get { return dec.Bitrate; }

            set { dec.Bitrate = value; }
        }

        // Doesn't apply
        public int Channels
        {
            get; set;
        }

        public byte[] Decode(byte[] b) { return dec.Decode(b); }

        public MediaBuffer<byte> Decode(MediaBuffer<byte> mediaPack)
        {
            return new MediaBuffer<byte>(dec.Decode(mediaPack.Buffer.Array), mediaPack.StartTime, mediaPack.EndTime);
        }

        // Doesn't apply.
        public uint GetUnDecodedBytes()
        {

            return 0;
        }

        public bool Init()
        {
            return dec.Init();
        }

        // Doesn't apply.
        public int SampleRate
        {
            get; set;
        }

        #endregion
    }
}
