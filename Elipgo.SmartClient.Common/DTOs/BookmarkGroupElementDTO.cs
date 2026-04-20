namespace Elipgo.SmartClient.Common.DTOs
{
    public class BookmarkGroupElementDTO
    {
        public int Id { get; set; }
        public int BookmarkGroupId { get; set; }
        public int DeviceFeatureId { get; set; }
        public int? NvrId { get; set; }
        public int? SsrId { get; set; }
        public string TypeRecorder { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string ProcessId { get; set; }
        public bool IsDeleted { get; set; }

        // Extra 
        public float Progress { get; set; } = 0;
        public bool Success { get; set; }
    }
}