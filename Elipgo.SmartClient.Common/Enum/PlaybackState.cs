namespace Elipgo.SmartClient.Common.Enum
{
    public enum PlaybackState
    {
        Stopped,
        Paused,
        Playing
    }

    public enum PlaybackConnectionState
    {
        Connecting,
        Reconnecting,
        Disconnected,
        Connected,
        NoRecording
    }

    public enum PlaybackRetry
    {
        StartRetry,
        ProcessRetry,
        EndRetry
    }
}
