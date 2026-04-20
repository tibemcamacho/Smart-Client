
namespace Elipgo.SmartClient.Common.DTOs
{
    public class LprSnapshotDTO
    {
        public byte[] Data { get; set; }
    }

    public class LprInstanPlayback
    {
        public int DeviceFeatureId { get; set; }
        public string TimeAction { get; set; }
    }
}
