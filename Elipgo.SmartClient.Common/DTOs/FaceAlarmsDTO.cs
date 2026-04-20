using System;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class FaceAlarmsDTO
    {
        public int Id { get; set; }
        public long DeviceFeatureId { get; set; }
        public string CameraId { get; set; }
        public string FacePosition { get; set; }
        public int FaceSize { get; set; }
        public byte[] FrameImage { get; set; }
        public ulong FrameIndex { get; set; }
        public int Quality { get; set; }
        public int Score { get; set; }
        public string SubjectCode { get; set; }
        public byte[] SubjectFaceImage { get; set; }
        public string SubjectGroup { get; set; }
        public int SubjectId { get; set; }
        public string SubjectLastName { get; set; }
        public byte[] SubjectModelImage { get; set; }
        public string SubjectName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
