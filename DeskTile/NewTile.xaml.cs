using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeskTile
{
    /// <summary>
    /// Interaction logic for NewTile.xaml
    /// </summary>
    internal static class WindowExtensionsX
    {
        // from winuser.h
        private const int GWL_STYLE = -16,
                          WS_MAXIMIZEBOX = 0x10000,
                          WS_MINIMIZEBOX = 0x20000,
                          WS_Close = 0x30000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        internal static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX & WS_Close));
        }
    }
    public partial class NewTile : Window
    {
         
   

        public string color, footer;
        public bool result = false;
        Xceed.Wpf.Toolkit.ColorPicker pick;
        public NewTile()
        {
            InitializeComponent();
            pick = new Xceed.Wpf.Toolkit.ColorPicker();
            pick.SelectedColorChanged += pick_SelectedColorChanged;
            pick.Margin=new Thickness(90,75,0,0);
            pick.Width = 275;
            Container.Children.Add(pick);
            
        }

        void pick_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            color=((Xceed.Wpf.Toolkit.ColorPicker)(sender)).SelectedColor.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            footer = FOOT.Text;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (color==null || footer==null)
            {
                if (color == null && footer != null)
                {
                    color = pick.SelectedColor.ToString();
                    result = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Either put some value or close the diloge . ", "Error04", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    result = false;
                }
            }
            else
            {
                result=true;
                this.Close();
            }
        }

        private void cANCEL_Click(object sender, RoutedEventArgs e)
        {
              result=false;
              this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                result = false;
                this.Close();

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // WindowExtensionsX.HideMinimizeAndMaximizeButtons(this);
        }

       
    }
}
