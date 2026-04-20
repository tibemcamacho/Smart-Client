using System.Collections.Generic;
using System.Linq;

namespace Elipgo.SmartClient.Drivers
{
    public class StreamSwitcher
    {
        private readonly int _expectedFps;
        private readonly int _minThreshold;
        private readonly int _maxThreshold;
        private readonly int _windowMinutes;
        private readonly Queue<int> _frameHistory;

        public StreamSwitcher(int expectedFps, int minThreshold = 50, int maxThreshold = 90, int windowMinutes = 10)
        {
            _expectedFps = expectedFps;
            _minThreshold = minThreshold;
            _maxThreshold = maxThreshold;
            _windowMinutes = windowMinutes;
            _frameHistory = new Queue<int>();
        }

        public void AddFrameCount(int frames)
        {
            if (_frameHistory.Count >= _windowMinutes)
            {
                _frameHistory.Dequeue();
            }

            _frameHistory.Enqueue(frames);
        }

        public double GetAveragePercentage()
        {
            if (_frameHistory.Count == 0)
            {
                return 0;
            }

            double averageFrames = _frameHistory.Average();
            double expectedPerMinute = _expectedFps * 60;
            return (averageFrames / expectedPerMinute) * 100.0;
        }

        public bool ShouldSwitchToSubstream(bool isMainStream)
        {
            return isMainStream && GetAveragePercentage() <= _minThreshold;
        }

        public bool ShouldSwitchToMainstream(bool isSubstream)
        {
            return isSubstream && GetAveragePercentage() >= _maxThreshold;
        }
    }
}

