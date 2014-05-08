using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DesktopCall
{
    public class Dekstop
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);
        [DllImport("user32.dll")]

        static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }
        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_WINDOWPOSCHANGING = 0x46;
            const uint SWP_NOZORDER = 0x4;

            if (msg == WM_WINDOWPOSCHANGING)
            {
                WINDOWPOS wp = (WINDOWPOS)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                wp.flags = wp.flags | SWP_NOZORDER;
                System.Runtime.InteropServices.Marshal.StructureToPtr(wp, lParam, true);
            }
            return IntPtr.Zero;
        }
       
        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        public static bool HideDesktopIcon(bool toDo)
        {

            return true;
        }
        public static void StickThisWindowToDesktop(IntPtr windowHandle)
        {
           // IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
           // SetParent(windowHandle, hWndProgMan);
            HideFromAltTab(windowHandle);
            SetWindowPos(windowHandle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        public static void HideTaskbar(bool toDo)
        {

        
        }


    }
}
