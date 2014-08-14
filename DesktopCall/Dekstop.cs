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
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xf020;
        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_WINDOWPOSCHANGING = 0x46;
            const int WM_SIZE = 5;
            const uint SWP_NOZORDER = 0x4;
           

            if (msg == WM_WINDOWPOSCHANGING)
            {
                WINDOWPOS wp = (WINDOWPOS)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                wp.flags = wp.flags | SWP_NOZORDER;
                System.Runtime.InteropServices.Marshal.StructureToPtr(wp, lParam, true);
            }
            if (msg == WM_SYSCOMMAND)
            {
                if (wParam.ToInt32()== SC_MINIMIZE)
                {
                    handled = true;
                    return IntPtr.Zero;
                }
            }

            return IntPtr.Zero;
        }
       
        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        
        public static void StickThisWindowToDesktop(IntPtr windowHandle)
        {
          //IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
           //SetParent(windowHandle, hWndProgMan);
            HideFromAltTab(windowHandle);

            SetWindowPos(windowHandle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd,IntPtr childAfterHwnd,IntPtr className,string windowText);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        void ShowDesktopIcons(bool show)
        {
            IntPtr hWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            if (show)
                ShowWindow(hWnd, 5);
            else 
                ShowWindow(hWnd, 0);
        }
 
        public static void HideTaskbar(bool toDo)
        {
            int hwnd = FindWindow("Shell_TrayWnd", "").ToInt32();
            IntPtr hwndOrb = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null);
            if (!toDo)
            {
                ShowWindow(hwndOrb, SW_HIDE);
                ShowWindow(hwnd, SW_HIDE);
            }

            else
            {
                ShowWindow(hwndOrb, SW_SHOW);
                ShowWindow(hwnd, SW_SHOW);
            }
        }



       
    }
}
