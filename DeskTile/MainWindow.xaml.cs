using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DesktopCall;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace DeskTile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RenderCanvas_Loaded(object sender, RoutedEventArgs e)
        {

            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            DesktopCall.Dekstop.StickThisWindowToDesktop(windowHandle);
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(DesktopCall.Dekstop.WndProc);

        }
        bool now = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            now = !now;
            DesktopCall.Dekstop.HideTaskbar(now);

        }
       

    }
}
