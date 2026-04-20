using ElipgoVideo.Commons.Data;
using ElipgoVideo.Commons.EventArgs;
using System;
using System.Drawing;
using System.IO;

namespace ElipgoVideo.Core.Controllers
{
    public interface IVideoController
    {

        /// <summary>
        /// Start decoding of frames of video secuence.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="initialOffset">Starting offset.</param>
        void Play(Stream elgFileStream, int initialOffset);

        /// <summary>
        /// Start reverse decoding of frames of video secuence.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="barPosition">Position of track bar corresponding with video position.</param>
        /// <param name="barSize">Bar total size.</param>
        void PlayReverse(Stream elgFileStream, int barPosition, int barSize);

        /// <summary>
        /// Start decoding frames but in this case returns jpeg images that can be stored on HDD. Used in ElipgoSeproban system.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="initialOffset">Starting offset.</param>
        void GenerateImages(Stream elgFileStream, int initialOffset);

        /// <summary>
        /// Stop all decoding operations.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause decoding operations
        /// </summary>
        void Pause();

        /// <summary>
        /// Relocates the video sequence at a desired point.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="keyFramePositionData">Desired point of video secuence</param>
        void Seek(Stream elgFileStream, SeekData keyFramePositionData);

        /// <summary>
        /// Event triggered when a frame is decoded.
        /// </summary>
        event EventHandler<DecodedFrameEventArg> SendDecodedFrame;

        /// <summary>
        /// Event launched to move the track bar
        /// </summary>
        event EventHandler<MovingSeekBarEventArg> MoveSeekBar;

        /// <summary>
        /// Event launched when playback ended.
        /// </summary>
        event EventHandler PlaybackEnded;

        /// <summary>
        /// Raised SendDecodedFrame event. Method used in ElipgoSeproban system.
        /// </summary>
        /// <param name="decodedFrame">Frame decoded, needed to fill the event's args</param>
        /// <param name="camId">Camera id, needed to fill the event's args</param>
        /// <param name="frameTimeStamp">Frame's time stamp, needed to fill the event's args</param>
        void OnSendDecodedFrame(Image decodedFrame, int camId, string frameTimeStamp);

        /// <summary>
        /// Raised SendDecodedFrame event.
        /// </summary>
        /// <param name="decodedFrame">Frame decoded, needed to fill the event's args</param>
        /// <param name="camId">Camera id, needed to fill the event's args</param>
        /// <param name="frameTimeStamp">Frame's time stamp, needed to fill the event's args</param>
        void OnSendDecodedFrame(DecodedFrame decodedFrame, int camId, string frameTimeStamp);

        /// <summary>
        /// Raised SendMoveSeekBar event.
        /// </summary>
        /// <param name="movingValue"></param>
        /// <param name="reset">Indicate if track bar must be restart</param>
        void OnMoveSeekBar(int movingValue, bool reset);

        /// <summary>
        /// Raised PlaybackEnded event.
        /// </summary>
        void OnPlaybackEnded();
    }
}
