using ElipgoVideo.Commons.Data;
using ElipgoVideo.Commons.Enums;
using System;
using System.Collections.Generic;

namespace ElipgoVideo.Commons.Utils
{
    public class VideoPlayUtil
    {
        /// <summary>
        /// Length of the video secuence in milliseconds.
        /// </summary>
        private static double length;

        /// <summary>
        /// Time that must elapse before the progress bar step forward
        /// </summary>
        private static double trackTime;

        /// <summary>
        /// Step that should advance the progress bar
        /// </summary>
        private static int trackBarStep;

        /// <summary>
        /// Time that must elapse before the progress bar step back
        /// </summary>
        private static double trackTimeReversePlayback;

        /// <summary>
        /// Step that should advance the progress bar in reverse.
        /// </summary>
        private static int trackBarStepReversePlayback;

        /// <summary>
        /// Video play speed.
        /// </summary>
        private static int speed = 1;

        private static List<SeekData> keyFramesPosition = new List<SeekData>();

        private static SortedDictionary<int, CameraData> cameraList = new SortedDictionary<int, CameraData>();

        private static Dictionary<int, Queue<DecodedFrame>> decodedFrames = new Dictionary<int, Queue<DecodedFrame>>();

        public static readonly object locker = new object();

        /// <summary>
        /// Initialize the data for the reproduction of video.
        /// </summary>
        /// <param name="startingTime">Date of the initial frame.</param>
        /// <param name="finishingTime">Date of the last frame.</param>
        /// <param name="barSize">Size of progress bar.</param>
        public static void Initialize(DateTime startingTime, DateTime finishingTime,
            int barSize = 0)
        {
            length = finishingTime.TimeOfDay.TotalMilliseconds - startingTime.TimeOfDay.TotalMilliseconds;
            trackTime = 1 / ((barSize) / length);
        }

        /// <summary>
        /// Initialize the data for the reproduction of video in reverse.
        /// </summary>
        /// <param name="totalFrames">Total frame within a video secuence .</param>
        /// <param name="barRemainigSize">Size from actual bar position to initial position.</param>
        /// <param name="barSize">Size of progress bar.</param>
        public static void InitializeReverse(long totalFrames, int barRemainigSize, int barSize = 0)
        {
            trackTimeReversePlayback = totalFrames > barRemainigSize ? Math.Round((double)totalFrames / barRemainigSize, 0, MidpointRounding.AwayFromZero) : 1;
            //int barReductionFactor = (int) Math.Round((double)barSize / barRemainigSize, 0, MidpointRounding.AwayFromZero);
            trackBarStepReversePlayback = (int)(totalFrames < barRemainigSize ? Math.Round((double)barRemainigSize / totalFrames, 0, MidpointRounding.AwayFromZero) : 1);
        }

        /// <summary>
        /// Initialize the data for the reproduction of video.
        /// </summary>
        /// <param name="totalFrames">Total frame within a video secuence .</param>
        /// <param name="barSize">Size of progress bar.</param>
        public static void Initialize(long totalFrames, int barSize = 1)
        {
            double estimateTime = totalFrames > barSize ? totalFrames / barSize : 1;
            trackTime = Math.Round(estimateTime, 0, MidpointRounding.AwayFromZero);
            double estimateBarStep = totalFrames < barSize ? barSize / totalFrames : 1;
            trackBarStep = (int)Math.Round(estimateBarStep, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Store the key-frame psition.
        /// </summary>
        /// <param name="offset">Key-frame's offset.</param>
        public static void StoreKeyFrameOffset(int offset, int cameraId, DateTime frameDateTime)
        {
            keyFramesPosition.Add(new SeekData(offset, cameraId, frameDateTime));
        }

        /// <summary>
        /// Calculate the position of the nearest frame to the position selected by the user
        /// </summary>
        /// <param name="barPosition">User's selected position</param>
        /// <param name="barSize">Bar total length</param>
        /// <returns>Info of te nearest key frame</returns>
        public static SeekData GetCloserKeyFrame(int barPosition, int barSize)
        {
            // Is the position into keyFramesPosition where can find the key frame offset
            int relativeBarKeyFramesPosition = (barPosition * keyFramesPosition.Count) / barSize;
            int position = relativeBarKeyFramesPosition < keyFramesPosition.Count ? relativeBarKeyFramesPosition : (keyFramesPosition.Count - 1);
            SeekData closerKeyFrame = keyFramesPosition[position];
            closerKeyFrame.Position = position;
            return closerKeyFrame;
        }

        /// <summary>
        /// Calculate the position of the nearest frame to the date and time selected by the user
        /// </summary>
        /// <param name="dateTime">DateTime to find a closer frame.</param>
        /// <returns>Info of te nearest key frame</returns>
        public static SeekData GetCloserKeyFrame(DateTime dateTime, int barSize, out int barNewPosition)
        {
            long dateTimeDiference = 0;
            int i = 0;
            SeekData closerKeyFrame;
            while (i < keyFramesPosition.Count && dateTimeDiference >= 0)
            {
                dateTimeDiference = dateTime.Ticks - keyFramesPosition[i].FrameDateTime.Ticks;
                i++;
            }
            if (i > 2 && (dateTime.Ticks - keyFramesPosition[i - 2].FrameDateTime.Ticks < -(dateTimeDiference) || i == keyFramesPosition.Count))
            {
                closerKeyFrame = keyFramesPosition[i - 2];
                closerKeyFrame.Position = i - 2;
                barNewPosition = ((i - 2) * barSize) / keyFramesPosition.Count;
            }
            else
            {
                closerKeyFrame = keyFramesPosition[i - 1];
                closerKeyFrame.Position = i - 1;
                barNewPosition = ((i - 1) * barSize) / keyFramesPosition.Count;
            }
            return closerKeyFrame;
        }

        public static int Cameras { get { return cameraList.Count; } }

        public static double Length { get { return length; } }

        public static double TrackTime { get { return trackTime; } }

        public static int TrackBarStep { get { return trackBarStep; } }

        public static double TrackTimeReversePlayback { get { return trackTimeReversePlayback; } }

        public static int TrackBarStepReversePlayback { get { return trackBarStepReversePlayback; } }

        public static int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public static int CurrentOffSet { get; set; }

        public static int KeyFramesCount
        {
            get { return keyFramesPosition.Count; }
        }

        public static List<SeekData> KeyFramesPositions
        {
            get { return keyFramesPosition; }
        }

        public static SortedDictionary<int, CameraData> CameraList
        {
            get { return cameraList; }
        }

        public static void RegisterCamera(int camera, int cameraFinalOffset)
        {
            if (!cameraList.ContainsKey(camera))
            {
                cameraList.Add(camera, new CameraData(cameraFinalOffset));
                if (!decodedFrames.ContainsKey(camera))
                {
                    decodedFrames.Add(camera, new Queue<DecodedFrame>());
                }
            }
            else
            {
                cameraList[camera].FinalOffset = cameraFinalOffset;
                cameraList[camera].CameraState = CameraState.Active;
            }
        }

        public static void CleanCameraList()
        {
            cameraList.Clear();
        }

        public static void CleanKeyFramesPositions()
        {
            keyFramesPosition.Clear();
        }

        public static Dictionary<int, Queue<DecodedFrame>> DecodedFrames
        {
            set { decodedFrames = value; }
            get { return decodedFrames; }
        }
    }

}
