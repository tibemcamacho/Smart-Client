using System;
using System.Runtime.InteropServices;

namespace Elipgo.SmartClient.Drivers.Rtsp.RtspPlayerControl
{
    [ComVisible(true)]
    [Guid("B510E263-968F-421C-A533-FAAEAA033538")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IRtspPlayerControlEvents
    {
        [DispId(0x00000001)]
        void OnError(int errorCode, string errorInfo);
        [DispId(0x00000002)]
        void OnNewImage();
        [DispId(0x00000003)]
        void OnKeyDown(int keyCode, long flags, bool handled);
        [DispId(0x00000004)]
        void OnClick(int x, int y);
        [DispId(0x00000005)]
        void OnDoubleClick(int nButton, int nShiftState, int fX, int fY);
        [DispId(0x00000006)]
        void OnMouseDown(int nButton, int nShiftState, int fX, int fY);
        [DispId(0x00000007)]
        void OnMouseMove(int nButton, int nShiftState, int fX, int fY);
        [DispId(0x00000008)]
        void OnMouseUp(int nButton, int nShiftState, int fX, int fY);
        [DispId(0x00000009)]
        void OnMouseWheel(int nShiftState, int zDelta, int fX, int fY);
    }
}
