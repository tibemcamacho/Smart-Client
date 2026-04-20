using ElipgoVideo.Commons.Data;
using ElipgoVideo.Commons.Enums;
using ElipgoVideo.Commons.EventArgs;
using ElipgoVideo.Commons.Utils;
using ElipgoVideo.Core.Decoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ElipgoVideo.Core.Controllers.Impl
{
    class VideoControllerImpl : IVideoController
    {
        private readonly List<VideoDecoder> decoderList;

        private volatile bool playing = false;

        private Thread videoDecoderThread;

        internal VideoControllerImpl()
        {
            this.decoderList = new List<VideoDecoder>();
        }

        #region Video operations

        /// <summary>
        /// Start decoding of frames of video secuence.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="initialOffset">Starting offset.</param>
        public void Play(Stream elgFileStream, int initialOffset)
        {
            this.playing = true;
            this.Initialize(elgFileStream, initialOffset);
        }

        /// <summary>
        /// Start decoding frames but in this case returns jpeg images that can be stored on HDD. Used in ElipgoSeproban system.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="initialOffset">Starting offset.</param>
        public void GenerateImages(Stream elgFileStream, int initialOffset)
        {
            this.playing = true;
            this.InitializeDecodeToImage(elgFileStream, initialOffset);
        }

        /// <summary>
        /// Start reverse decoding of frames of video secuence.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="barPosition">Position of track bar corresponding with video position.</param>
        /// <param name="barSize">Bar total size.</param>
        public void PlayReverse(Stream elgFileStream, int barPosition, int barSize)
        {
            this.playing = true;
            SeekData keyFramePositionData = VideoPlayUtil.GetCloserKeyFrame(barPosition, barSize);
            int framesCount = 0;
            for (int i = keyFramePositionData.Position; i >= 0; i--)
            {
                if (VideoPlayUtil.KeyFramesPositions[i].CameraId == keyFramePositionData.CameraId)
                {
                    framesCount++;
                }
            }
            VideoPlayUtil.InitializeReverse(framesCount, barPosition, barSize);
            this.InitializeReverse(elgFileStream, keyFramePositionData.Position);
        }

        /// <summary>
        /// Stop all decoding operations.
        /// </summary>
        public void Stop()
        {
            this.End();
            if (MoveSeekBar != null)
            {
                OnMoveSeekBar(0, true);
            }
        }

        /// <summary>
        /// Pause decoding operations
        /// </summary>
        public void Pause()
        {
            this.End();
        }

        /// <summary>
        /// Relocates the video sequence at a desired point.
        /// </summary>
        /// <param name="elgFileStream">Secuence file stream.</param>
        /// <param name="keyFramePositionData">Desired point of video secuence</param>
        public void Seek(Stream elgFileStream, SeekData keyFramePositionData)
        {
            if (!this.playing)
            {
                this.ManualDecodeOperation(elgFileStream, keyFramePositionData);
            }
        }

        private void ManualDecodeOperation(Stream elgFileStream, SeekData keyFramePositionData)
        {
            elgFileStream.Position = keyFramePositionData.KeyFrameOffset;
            int lon = (int)elgFileStream.Length;
            //File's offset.
            int offset = keyFramePositionData.KeyFrameOffset;
            int decodedCameraFrame = 0;
            while (!OneFrameByCamera() && offset < lon - 1)
            {
                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFileStream.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);

                //Read freme's bytes.
                byte[] frame;
                if (offset + bHeader.frameLength < lon)
                {
                    frame = new byte[bHeader.frameLength];
                    //Track the key-frames of the camId = 0. Monitoring for pause the video.
                    if (bHeader.frameType == 1)
                    {
                        VideoPlayUtil.CurrentOffSet = (int)elgFileStream.Position - ElipgoHeader.HEADER_SIZE;
                    }

                    //Find camera's decoder.
                    if (bHeader.frameType == 1)
                    {
                        elgFileStream.Read(frame, 0, bHeader.frameLength);
                        offset += bHeader.frameLength;
                        VideoDecoder dec = FindDecoder(bHeader.camId, bHeader.codec);
                        //Decoding frame.
                        DecodedFrame decodedFrame = dec.Decode(frame, bHeader.time);
                        if (decodedFrame != null && VideoPlayUtil.DecodedFrames[bHeader.camId].Count == 0)
                        {
                            decodedFrame.Fps = bHeader.fpsRate;
                            decodedFrame.Time = bHeader.time;
                            lock (VideoPlayUtil.locker)
                            {
                                VideoPlayUtil.DecodedFrames[bHeader.camId].Enqueue(decodedFrame);
                                Monitor.Pulse(VideoPlayUtil.locker);
                            }
                        }
                    }
                    else
                    {
                        offset += bHeader.frameLength;
                        elgFileStream.Position = offset;
                    }
                }
                else
                {
                    frame = new byte[lon - (offset + bHeader.frameLength)];
                    elgFileStream.Read(frame, 0, (lon - (offset + bHeader.frameLength)));
                    offset = lon;
                }
                decodedCameraFrame++;
            }
        }

        private bool OneFrameByCamera()
        {
            int framesCount = 0;
            foreach (var decodedFrame in VideoPlayUtil.DecodedFrames)
            {
                if (decodedFrame.Value.Count >= 1)
                {
                    framesCount++;
                }
            }
            return framesCount >= VideoPlayUtil.CameraList.Count;
        }

        #endregion

        private void Initialize(Stream elgFileStream, int initialOffset)
        {
            if (this.videoDecoderThread == null || !this.videoDecoderThread.IsAlive)
            {
                this.videoDecoderThread = new Thread(new ThreadStart(() => Decode(elgFileStream, initialOffset)));
                this.videoDecoderThread.IsBackground = true;
                this.videoDecoderThread.Start();
            }
        }

        private void InitializeReverse(Stream elgFileStream, int position)
        {
            if (this.videoDecoderThread == null || !this.videoDecoderThread.IsAlive)
            {
                this.videoDecoderThread = new Thread(new ThreadStart(() => DecodeReverse(elgFileStream, position)));
                this.videoDecoderThread.IsBackground = true;
                this.videoDecoderThread.Start();
            }
        }

        private void InitializeDecodeToImage(Stream elgFileStream, int position)
        {
            if (this.videoDecoderThread == null || !this.videoDecoderThread.IsAlive)
            {
                this.videoDecoderThread = new Thread(new ThreadStart(() => DecodeToImage(elgFileStream, position)));
                this.videoDecoderThread.IsBackground = true;
                this.videoDecoderThread.Start();
            }
        }

        private void End()
        {
            this.playing = false;
            if (this.videoDecoderThread != null)
            {
                this.videoDecoderThread.Join(5);
            }
        }

        /// <summary>
        /// Decoding frame to show video secuence.
        /// </summary>
        /// <param name="elgFileStream">Stream to elg file</param>
        /// <param name="intinitialOffset">Initial offset of video file.</param>
        private void Decode(Stream elgFileStream, int intinitialOffset)
        {
            elgFileStream.Position = intinitialOffset;
            int lon = (int)elgFileStream.Length;

            //File's offset.
            int offset = intinitialOffset;
            int totalFrame = 0;
            int camaraId = 0;

            int width = 0;
            int height = 0;

            Dictionary<int, CameraData> elgCameras = new Dictionary<int, CameraData>(VideoPlayUtil.CameraList);

            while (offset < lon - 1 && this.playing)
            {
                int activeCameras = 0;
                foreach (var camId in elgCameras.Keys)
                {
                    if (elgCameras[camId].FinalOffset > offset && elgCameras[camId].CameraState == CameraState.Active)
                    {
                        activeCameras++;
                    }
                }

                int trackTime = (int)VideoPlayUtil.TrackTime;

                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFileStream.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);

                camaraId = (camaraId == 0 || VideoPlayUtil.CameraList[camaraId].CameraState == CameraState.Inactive) ? bHeader.camId : camaraId;

                //Waiting for consumer
                while (this.DecodedFramesLimit(5) && this.playing) { }

                if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.Active ||
                    (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy && bHeader.frameType == 1))
                {
                    if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy)
                    {
                        VideoPlayUtil.CameraList[bHeader.camId].CameraState = CameraState.Active;
                        activeCameras++;
                    }

                    if (elgCameras[bHeader.camId].FinalOffset > 0)
                    {
                        elgCameras[bHeader.camId].FinalOffset--;
                    }

                    //Read freme's bytes.
                    byte[] frame;
                    if (offset + bHeader.frameLength <= lon)
                    {
                        frame = new byte[bHeader.frameLength];
                        //Track the key-frames of the camId = 0. Monitoring for pause the video.
                        if (bHeader.frameType == 1 && bHeader.camId == 0)
                        {
                            VideoPlayUtil.CurrentOffSet = (int)elgFileStream.Position - ElipgoHeader.HEADER_SIZE;
                        }
                        elgFileStream.Read(frame, 0, bHeader.frameLength);
                        offset += bHeader.frameLength;

                        if (bHeader.frameType == 1)
                        {
                            int newWidth = 0;
                            int newHeight = 0;
                            H264SPSParser.ParseSpsNal(frame, out newWidth, out newHeight);
                            if (newWidth != width || newHeight != height)
                            {
                                width = newWidth;
                                height = newHeight;
                                CloseDecoder(bHeader.camId);
                            }
                        }

                        //Find camera's decoder.
                        VideoDecoder dec = FindDecoder(bHeader.camId, bHeader.codec);
                        //Decoding frame.
                        DecodedFrame decodedFrame = dec.Decode(frame, bHeader.time);
                        if (decodedFrame != null)
                        {
                            decodedFrame.Fps = bHeader.fpsRate;
                            decodedFrame.Time = bHeader.time;
                            lock (VideoPlayUtil.locker)
                            {
                                VideoPlayUtil.DecodedFrames[bHeader.camId].Enqueue(decodedFrame);
                                Monitor.Pulse(VideoPlayUtil.locker);
                            }
                            totalFrame = bHeader.camId == camaraId ? totalFrame + 1 : totalFrame;
                        }
                    }
                    else
                    {
                        int arrayAllocation = lon - (offset + bHeader.frameLength) < 0 ?
                            lon - (offset + (bHeader.frameLength + lon - (offset + bHeader.frameLength))) : lon - (offset + bHeader.frameLength);
                        frame = new byte[arrayAllocation];
                        elgFileStream.Read(frame, 0, arrayAllocation);
                        offset = lon;
                    }
                    if (totalFrame == trackTime)
                    {
                        OnMoveSeekBar(VideoPlayUtil.TrackBarStep, false);
                        totalFrame = 0;
                    }
                    if (VideoPlayUtil.Speed == 0)
                    {
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    offset += bHeader.frameLength;
                    elgFileStream.Position += bHeader.frameLength;
                }
            }

            if (offset == lon || !this.playing)
            {
                // This could disappear when the seek functionality be ready
                this.CloseDecoders();
                if (this.playing)
                {
                    this.End();
                }
                if (offset == lon)
                {
                    OnMoveSeekBar(0, true);
                    OnPlaybackEnded();
                    MoveSeekBar = null;
                    SendDecodedFrame = null;
                    PlaybackEnded = null;
                }
            }
        }

        private void DecodeReverse(Stream elgFileStream, int position)
        {
            //File's offset.
            int totalFrame = 0;
            int camaraId = 0;

            Dictionary<int, CameraData> elgCameras = new Dictionary<int, CameraData>(VideoPlayUtil.CameraList);

            int currentPosition = position;
            bool firstFrame = false;

            while (currentPosition >= 0 && this.playing)
            {
                int offset = VideoPlayUtil.KeyFramesPositions[currentPosition].KeyFrameOffset;
                elgFileStream.Position = VideoPlayUtil.KeyFramesPositions[currentPosition].KeyFrameOffset;
                VideoPlayUtil.CurrentOffSet = offset;
                firstFrame = offset == 0;
                int activeCameras = 0;
                foreach (var camId in elgCameras.Keys)
                {
                    if (elgCameras[camId].FinalOffset > offset && elgCameras[camId].CameraState == CameraState.Active)
                    {
                        activeCameras++;
                    }
                }

                int trackTime = (int)VideoPlayUtil.TrackTimeReversePlayback;

                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFileStream.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);

                camaraId = (camaraId == 0 || VideoPlayUtil.CameraList[camaraId].CameraState == CameraState.Inactive) ? bHeader.camId : camaraId;

                //Waiting for consumer
                while (this.DecodedFramesLimit(5) && this.playing) { }

                if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.Active ||
                    (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy && bHeader.frameType == 1))
                {
                    if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy)
                    {
                        VideoPlayUtil.CameraList[bHeader.camId].CameraState = CameraState.Active;
                        activeCameras++;
                    }
                    byte[] frame = new byte[bHeader.frameLength];
                    //Track the key-frames of the camId = 0. Monitoring for pause the video.
                    if (bHeader.frameType == 1 && bHeader.camId == 0)
                    {
                        VideoPlayUtil.CurrentOffSet = (int)elgFileStream.Position - ElipgoHeader.HEADER_SIZE;
                    }
                    elgFileStream.Read(frame, 0, bHeader.frameLength);
                    offset += bHeader.frameLength;

                    //Find camera's decoder.
                    VideoDecoder dec = FindDecoder(bHeader.camId, bHeader.codec);
                    //Decoding frame.
                    DecodedFrame decodedFrame = dec.Decode(frame, bHeader.time);
                    if (decodedFrame != null)
                    {
                        decodedFrame.Fps = bHeader.fpsRate;
                        decodedFrame.Time = bHeader.time;
                        lock (VideoPlayUtil.locker)
                        {
                            VideoPlayUtil.DecodedFrames[bHeader.camId].Enqueue(decodedFrame);
                            Monitor.Pulse(VideoPlayUtil.locker);
                        }
                        totalFrame = bHeader.camId == camaraId ? totalFrame + 1 : totalFrame;
                    }
                    if (totalFrame == trackTime)
                    {
                        OnMoveSeekBar(-VideoPlayUtil.TrackBarStepReversePlayback, false);
                        totalFrame = 0;
                    }
                }
                currentPosition--;
            }

            if (firstFrame || !this.playing)
            {
                this.CloseDecoders();
                if (this.playing)
                {
                    this.End();
                }
                if (firstFrame)
                {
                    OnMoveSeekBar(0, true);
                    OnPlaybackEnded();
                    MoveSeekBar = null;
                    SendDecodedFrame = null;
                    PlaybackEnded = null;
                }
            }
        }

        private bool DecodedFramesLimit(int frameLimit)
        {
            int framesCount = 0;
            foreach (var decodedFrame in VideoPlayUtil.DecodedFrames)
            {
                framesCount += decodedFrame.Value.Count;
            }
            return framesCount >= (frameLimit * VideoPlayUtil.CameraList.Count);
        }

        private void Decode1(Stream elgFileStream, int intinitialOffset)
        {
            elgFileStream.Position = intinitialOffset;
            int lon = (int)elgFileStream.Length;

            //File's offset.
            int offset = intinitialOffset;
            int totalFrame = 0;
            int camaraId = 0;
            int timeElapsed = 0;

            Dictionary<int, CameraData> elgCameras = new Dictionary<int, CameraData>(VideoPlayUtil.CameraList);

            while (offset < lon - 1 && this.playing)
            {
                int activeCameras = 0;
                foreach (var camId in elgCameras.Keys)
                {
                    if (elgCameras[camId].FinalOffset > offset && elgCameras[camId].CameraState == CameraState.Active)
                    {
                        activeCameras++;
                    }
                }
                var watch = Stopwatch.StartNew();

                int trackTime = (int)VideoPlayUtil.TrackTime;

                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFileStream.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);

                camaraId = (camaraId == 0 || VideoPlayUtil.CameraList[camaraId].CameraState == CameraState.Inactive) ? bHeader.camId : camaraId;

                if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.Active ||
                    (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy && bHeader.frameType == 1))
                {
                    if (VideoPlayUtil.CameraList[bHeader.camId].CameraState == CameraState.StandingBy)
                    {
                        VideoPlayUtil.CameraList[bHeader.camId].CameraState = CameraState.Active;
                        activeCameras++;
                    }

                    int rateFactor = 0;
                    if (VideoPlayUtil.Speed != 0)
                    {
                        rateFactor = (int)(VideoPlayUtil.Speed > 0
                                                ? activeCameras * (Math.Ceiling((decimal)bHeader.fpsRate / 100)) * VideoPlayUtil.Speed
                                                : -1 * ((activeCameras * (bHeader.fpsRate / 100)) / VideoPlayUtil.Speed));
                    }
                    //Avoiding division by zero.
                    rateFactor = rateFactor == 0 ? 1 : rateFactor;

                    if (elgCameras[bHeader.camId].FinalOffset > 0)
                    {
                        elgCameras[bHeader.camId].FinalOffset--;
                    }

                    //Read freme's bytes.
                    byte[] frame;
                    if (offset + bHeader.frameLength <= lon)
                    {
                        frame = new byte[bHeader.frameLength];
                        //Track the key-frames of the camId = 0. Monitoring for pause the video.
                        if (bHeader.frameType == 1 && bHeader.camId == 0)
                        {
                            VideoPlayUtil.CurrentOffSet = (int)elgFileStream.Position - ElipgoHeader.HEADER_SIZE;
                        }
                        elgFileStream.Read(frame, 0, bHeader.frameLength);
                        offset += bHeader.frameLength;

                        //Find camera's decoder.
                        VideoDecoder dec = FindDecoder(bHeader.camId, bHeader.codec);
                        //Decoding frame.
                        DecodedFrame decodedFrame = dec.Decode(frame, bHeader.time);
                        if (SendDecodedFrame != null)
                        {
                            OnSendDecodedFrame(decodedFrame, bHeader.camId, new DateTime(bHeader.time).ToString("MM/dd/yyyy:HH:mm:ss"));
                            totalFrame = bHeader.camId == camaraId ? totalFrame + 1 : totalFrame;
                        }
                    }
                    else
                    {
                        frame = new byte[lon - (offset + bHeader.frameLength)];
                        elgFileStream.Read(frame, 0, (lon - (offset + bHeader.frameLength)));
                        offset = lon;
                    }
                    watch.Stop();
                    timeElapsed += (VideoPlayUtil.Speed < 0 ? -VideoPlayUtil.Speed : VideoPlayUtil.Speed) * ((int)(1000 - watch.ElapsedMilliseconds) / rateFactor);
                    if (totalFrame == trackTime)
                    {
                        OnMoveSeekBar(VideoPlayUtil.TrackBarStep, false);
                        totalFrame = 0;
                        timeElapsed = 0;// timeElapsed - trackTime;
                    }
                    if (VideoPlayUtil.Speed != 0)
                    {
                        if (watch.ElapsedMilliseconds < 1000)
                        {
                            Thread.Sleep((int)(1000 - watch.ElapsedMilliseconds) / rateFactor);
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    offset += bHeader.frameLength;
                    elgFileStream.Position += bHeader.frameLength;
                }
            }

            if (offset == lon || !this.playing)
            {
                // This could disappear when the seek functionality be ready
                this.CloseDecoders();
                if (this.playing)
                {
                    this.End();
                }
                if (offset == lon)
                {
                    OnMoveSeekBar(0, true);
                    OnPlaybackEnded();
                    MoveSeekBar = null;
                    SendDecodedFrame = null;
                    PlaybackEnded = null;
                }
            }
        }

        private void DecodeToImage(Stream elgFileStream, int intinitialOffset)
        {
            elgFileStream.Position = intinitialOffset;
            int lon = (int)elgFileStream.Length;

            //File's offset.
            int offset = intinitialOffset;
            int totalFrame = 0;
            int camaraId = 0;
            int timeElapsed = 0;

            Dictionary<int, CameraData> elgCameras = new Dictionary<int, CameraData>(VideoPlayUtil.CameraList);

            while (offset < lon - 1 && this.playing)
            {
                int activeCameras = 0;
                foreach (var camId in elgCameras.Keys)
                {
                    if (elgCameras[camId].FinalOffset > offset)
                    {
                        activeCameras++;
                    }
                }
                var watch = Stopwatch.StartNew();

                int trackTime = (int)VideoPlayUtil.TrackTime;

                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFileStream.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);

                camaraId = camaraId == 0 ? bHeader.camId : camaraId;

                int rateFactor = 0;
                if (VideoPlayUtil.Speed != 0)
                {
                    rateFactor = (int)(VideoPlayUtil.Speed > 0
                                            ? activeCameras * (Math.Ceiling((decimal)bHeader.fpsRate / 100)) * VideoPlayUtil.Speed
                                            : -1 * ((activeCameras * (bHeader.fpsRate / 100)) / VideoPlayUtil.Speed));
                }
                //Avoiding division by zero.
                rateFactor = rateFactor == 0 ? 1 : rateFactor;

                if (elgCameras[bHeader.camId].FinalOffset > 0)
                {
                    elgCameras[bHeader.camId].FinalOffset--;
                }

                //Read freme's bytes.
                byte[] frame;
                if (offset + bHeader.frameLength <= lon)
                {
                    frame = new byte[bHeader.frameLength];
                    //Track the key-frames of the camId = 0. Monitoring for pause the video.
                    if (bHeader.frameType == 1 && bHeader.camId == 0)
                    {
                        VideoPlayUtil.CurrentOffSet = (int)elgFileStream.Position - ElipgoHeader.HEADER_SIZE;
                    }
                    elgFileStream.Read(frame, 0, bHeader.frameLength);
                    offset += bHeader.frameLength;

                    //Find camera's decoder.
                    VideoDecoder dec = FindDecoderToImage(bHeader.camId, bHeader.codec);
                    //Decoding frame.
                    Image decodedFrame = dec.DecodeToImage(frame, bHeader.time);
                    if (SendDecodedFrame != null)
                    {
                        OnSendDecodedFrame(decodedFrame, bHeader.camId, new DateTime(bHeader.time).ToString("MM/dd/yyyy:HH:mm:ss"));
                        totalFrame = bHeader.camId == camaraId ? totalFrame + 1 : totalFrame;
                    }
                }
                else
                {
                    frame = new byte[lon - (offset + bHeader.frameLength)];
                    elgFileStream.Read(frame, 0, (lon - (offset + bHeader.frameLength)));
                    offset = lon;
                }
                watch.Stop();
                timeElapsed += (VideoPlayUtil.Speed < 0 ? -VideoPlayUtil.Speed : VideoPlayUtil.Speed) * ((int)(1000 - watch.ElapsedMilliseconds) / rateFactor);
                if (totalFrame == trackTime)
                {
                    OnMoveSeekBar(VideoPlayUtil.TrackBarStep, false);
                    totalFrame = 0;
                    timeElapsed = 0;// timeElapsed - trackTime;
                }
                if (VideoPlayUtil.Speed != 0)
                {
                    if (watch.ElapsedMilliseconds < 1000)
                    {
                        Thread.Sleep((int)(1000 - watch.ElapsedMilliseconds) / rateFactor);
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }

            if (offset == lon || !this.playing)
            {
                // This could disappear when the seek functionality be ready
                this.CloseDecoders();
                if (this.playing)
                {
                    this.End();
                }
                if (offset == lon)
                {
                    OnMoveSeekBar(0, true);
                    OnPlaybackEnded();
                    MoveSeekBar = null;
                    SendDecodedFrame = null;
                    PlaybackEnded = null;
                }
            }
        }

        #region Utils

        private VideoDecoder FindDecoder(int camId, string codec)
        {
            bool found = false;
            int off = 0;
            if (decoderList.Count > 0)
            {
                while (off < decoderList.Count && !found)
                {
                    if (decoderList[off] != null && ((VideoDecoder)decoderList[off]).camId == camId)
                    {
                        found = true;
                    }
                    else
                    {
                        off++;
                    }
                }
                if (found)
                    return (VideoDecoder)decoderList[off];
            }

            var dec = new VideoDecoder(camId, codec);
            dec.Init(codec);
            decoderList.Add(dec);
            return dec;
        }

        private void CloseDecoder(int camId)
        {
            bool found = false;
            int off = 0;
            if (decoderList.Count > 0)
            {
                while (off < decoderList.Count && !found)
                {
                    if (((VideoDecoder)decoderList[off]).camId == camId)
                    {
                        found = true;
                    }
                    else
                    {
                        off++;
                    }
                }
                if (found)

                {
                    VideoDecoder videoDecoder = decoderList[off];
                    decoderList.Remove(decoderList[off]);
                    videoDecoder.Destroy();
                }
            }
        }

        private VideoDecoder FindDecoderToImage(int camId, string codec)
        {
            bool found = false;
            int off = 0;
            if (decoderList.Count > 0)
            {
                while (off < decoderList.Count && !found)
                {
                    if (((VideoDecoder)decoderList[off]).camId == camId)
                    {
                        found = true;
                    }
                    else
                    {
                        off++;
                    }
                }
                if (found)
                    return (VideoDecoder)decoderList[off];
            }

            var dec = new VideoDecoder(camId, codec);
            dec.InitDecoderToImage(codec);
            decoderList.Add(dec);
            return dec;
        }

        private void CloseDecoders()
        {
            foreach (VideoDecoder dec in decoderList)
            {
                dec.Destroy();
            }
            decoderList.Clear();
        }

        #endregion

        #region Seek bar event

        public event EventHandler<MovingSeekBarEventArg> MoveSeekBar;

        /// <summary>
        /// Raised SendMoveSeekBar event.
        /// </summary>
        /// <param name="movingValue"></param>
        /// <param name="reset">Indicate if track bar must be restart</param>
        public void OnMoveSeekBar(int movingValue, bool reset)
        {
            var handler = MoveSeekBar;
            if (handler != null)
            {
                handler(this, new MovingSeekBarEventArg(movingValue, reset));
            }
        }

        #endregion

        #region Decoding frames event

        /// <summary>
        /// Event triggered when a frame is decoded.
        /// </summary>
        public event EventHandler<DecodedFrameEventArg> SendDecodedFrame;

        /// <summary>
        /// Raised SendDecodedFrame event. Method used in ElipgoSeproban system.
        /// </summary>
        /// <param name="decodedFrame">Frame decoded, needed to fill the event's args</param>
        /// <param name="camId">Camera id, needed to fill the event's args</param>
        /// <param name="frameTimeStamp">Frame's time stamp, needed to fill the event's args</param>
        public void OnSendDecodedFrame(Image decodedFrame, int camId, string frameTimeStamp)
        {
            var handler = SendDecodedFrame;
            if (handler != null)
            {
                handler(this, new DecodedFrameEventArg(decodedFrame, camId, frameTimeStamp));
            }
        }

        /// <summary>
        /// Raised SendDecodedFrame event.
        /// </summary>
        /// <param name="decodedFrame">Frame decoded, needed to fill the event's args</param>
        /// <param name="camId">Camera id, needed to fill the event's args</param>
        /// <param name="frameTimeStamp">Frame's time stamp, needed to fill the event's args</param>
        public void OnSendDecodedFrame(DecodedFrame decodedFrame, int camId, string frameTimeStamp)
        {
            var handler = SendDecodedFrame;
            if (handler != null)
            {
                handler(this, new DecodedFrameEventArg(decodedFrame, camId, frameTimeStamp));
            }
        }

        #endregion

        #region End playback event.

        public event EventHandler PlaybackEnded;

        public void OnPlaybackEnded()
        {
            var handler = PlaybackEnded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}