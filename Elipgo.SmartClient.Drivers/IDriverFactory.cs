using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Reflections;
using System;

namespace Elipgo.SmartClient.Drivers
{
    public interface IDriverFactory
    {
        IDriverLive GetDriverLive(CameraDTO camera, Profile profile, bool initAudio, string nameTab);
        IDriverLive GetDriverLive(CameraDTO camera);
        IDriverInstantPlayback GetDriverInstantPlayback(CameraDTO camera, Profile profile, RecorderDTOSmall recorder, DateTime selectedDateTime, string nameTab, bool hideControls = false, bool isDiagnostic = false, DateTime? selectedEndDateTime = null);
        IManufactureUri GetDriverApiCgi(CameraDTO camera, int recorderId = 0);
        IDriverDownload GetDriverDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName, bool isEdge = false, bool isSyncServer = false);
        IDriverDownloadVisualSearch GetDriverVisualSerachDownload(CameraDTO camera);
    }
}
