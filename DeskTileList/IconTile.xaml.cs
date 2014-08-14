using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeskTileList
{
    /// <summary>
    /// Interaction logic for IconTile.xaml
    /// </summary>
    public partial class IconTile : Window
    {
        public IconTile()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }
        protected override void OnSourceInitialized(EventArgs e)
        { IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            base.OnSourceInitialized(e);
            DesktopCall.Dekstop.StickThisWindowToDesktop(windowHandle);
            // DesktopCall.Dekstop.WndProc(e);
            System.Windows.Interop.HwndSource SOURCE = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            SOURCE.AddHook(DesktopCall.Dekstop.WndProc);
           


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
           
            DesktopCall.Dekstop.StickThisWindowToDesktop(windowHandle);
            // DesktopCall.Dekstop.WndProc(e);
            System.Windows.Interop.HwndSource SOURCE = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            SOURCE.AddHook(DesktopCall.Dekstop.WndProc);
        }
        public void sendtoBack()
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;

            DesktopCall.Dekstop.StickThisWindowToDesktop(windowHandle);
        }

        private void TileList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tileObj.ChangeSize(this.Width, this.Height);
            
        }
        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
