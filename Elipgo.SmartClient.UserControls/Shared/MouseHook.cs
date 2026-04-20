using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public class MouseHook : IDisposable
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;

        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;

        public MouseHook()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc,
                        GetModuleHandle(curModule.ModuleName), 0);
                }
            }
            return IntPtr.Zero;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                MouseButton button = MouseButton.None;

                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    button = MouseButton.Left;
                }
                else if (wParam == (IntPtr)WM_RBUTTONDOWN)
                {
                    button = MouseButton.Right;
                }
                else if (wParam == (IntPtr)WM_MBUTTONDOWN)
                {
                    button = MouseButton.Middle;
                }

                if (button != MouseButton.None)
                {
                    OnMouseButtonPressed(new MouseButtonEventArgs(button, hookStruct.pt.x, hookStruct.pt.y));
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected virtual void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~MouseHook()
        {
            Dispose();
        }

        #region Windows API Imports

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        #endregion
    }

    public enum MouseButton
    {
        None,
        Left,
        Right,
        Middle
    }

    public class MouseButtonEventArgs : EventArgs
    {
        public MouseButton Button { get; }
        public int X { get; }
        public int Y { get; }

        public MouseButtonEventArgs(MouseButton button, int x, int y)
        {
            Button = button;
            X = x;
            Y = y;
        }
    }


}