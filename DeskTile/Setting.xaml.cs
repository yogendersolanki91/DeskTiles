using System.Windows;
using System.Windows.Input;
using DeskTileList;
using Kent.Boogaart.Windows.Controls;
using System;
using System.Runtime.InteropServices;


namespace DeskTile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    
   
     
    public class comboitem : System.Windows.Controls.TextBlock
    {
        public string valueName;
        public comboitem(string text, string value)
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            this.Width = 100;
            this.Text = text;
            valueName = value;
        }
    }
    internal static class WindowExtensions
    {
        // from winuser.h
        private const int GWL_STYLE = -16,
                          WS_MAXIMIZEBOX = 0x10000,
                          WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        internal static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }
    }

    public partial class Setting : Window
    {

        private Xceed.Wpf.Toolkit.ColorPicker getPicker() { 
        Xceed.Wpf.Toolkit.ColorPicker colorpik=new Xceed.Wpf.Toolkit.ColorPicker();
        colorpik.Margin=new Thickness(100,10,0,0);
        
        colorpik.Width = 250;
        colorpik.Height = 25;
        colorpik.SelectedColorChanged += colorpik_SelectedColorChanged;
            return colorpik;
        }

        void colorpik_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            if (ListAll.SelectedItem!=null)
            ((DeskTileList.TileList)setUpDuty.ALLTILES[((comboitem)ListAll.SelectedItem).valueName.ToString()].Content).updateColor(this.colorpik.SelectedColor.ToString());
        }
        TileManager setUpDuty;
        string pathtotiles = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\DeskTile";
  
        string currentName="";
        Xceed.Wpf.Toolkit.ColorPicker colorpik ;
        public Setting(ref TileManager source,int tileID)
        {
            
         
            InitializeComponent();
            colorpik = getPicker();
            controlBox.Children.Add(colorpik);
            switch (tileID)
            {
                case 1:
                    this.tabCT.SelectedIndex=0;
                    break;
                case 2:
                    this.tabCT.SelectedIndex = 1;
                    break;
                default:
                    break;
            }
            
            setUpDuty = source;
            foreach(Resizer r in source.ALLTILES.Values)
            {
                
                Resizer newControl = new Resizer { DataContext = ((Resizer)r.FindName(r.Name)).DataContext };
                ListAll.Items.Add(new comboitem(((DeskTileList.TileList)r.Content).FooterValue, r.Name));
            }
            if (ListAll.Items.Count > 0)
            {
                ListAll.SelectedIndex = 0;
                DeleteTile.IsEnabled = true;
                  
            }
            else
                DeleteTile.IsEnabled = false;
         
           
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            SerializerTile s = new SerializerTile();
            foreach (Resizer r in setUpDuty.ALLTILES.Values)
            {
                s.savetile(r, pathtotiles);

            }
            /*
            if (GetTileListByName() != null)
                GetTileListByName().updateColor(this.colorpik.SelectedColor.ToString());
            else
                MessageBox.Show("Tile not found,Please Make sure you have selected a tile that exist.", "Error02",MessageBoxButton.OK,MessageBoxImage.Exclamation);
             */
        }
        private DeskTileList.TileList GetTileListByName()
        {
            if(ListAll.SelectedItem!=null){
            if ((setUpDuty.ALLTILES[((comboitem)ListAll.SelectedItem).valueName]) != null )
                return ((DeskTileList.TileList)setUpDuty.ALLTILES[((comboitem)ListAll.SelectedItem).valueName.ToString()].Content);
            else
                return null;
            }
            return
                null;

        } 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
             
        }

        private void _Color_MouseMove(object sender, MouseEventArgs e)
        {

        }
        System.Windows.Media.ColorConverter cc = new System.Windows.Media.ColorConverter();
        private void ListAll_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListAll.SelectedIndex >= 0)
            {
                System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(GetTileListByName().BackColor.ToString());
                TileFooter.Text = ((comboitem)ListAll.SelectedItem).Text;
                colorpik.SelectedColor = color;
                DeleteTile.IsEnabled = true;
            }
            else
            {
                DeleteTile.IsEnabled = false;
            }
        }

        private void TileFooter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (GetTileListByName()!=null)
            {

                GetTileListByName().FooterValue = TileFooter.Text;
            }
        }

        private void AddNewTile_Click(object sender, RoutedEventArgs e)
        {
            
           
            
           
            //GetTileListByName().updateColor(colorpik.SelectedColor.ToString());

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerializerTile s = new SerializerTile();
            foreach (Resizer r in setUpDuty.ALLTILES.Values)
            {
                s.savetile(r, pathtotiles);

            }
        }

        private void AddTile_Click(object sender, RoutedEventArgs e)
        {
            NewTile nt=new NewTile();
            nt.ShowDialog();
            if (nt.result)
            {
                string newName = setUpDuty.Add(nt.footer);
                ((DeskTileList.TileList)setUpDuty.ALLTILES[newName].Content).updateColor(nt.color);
                int i = ListAll.Items.Add(new comboitem(nt.footer, newName));
                ListAll.SelectedIndex = i;
            }
        }

        private void DeleteTile_Click(object sender, RoutedEventArgs e)
        {
            if (ListAll.SelectedItem != null)
            {
                setUpDuty.Remove(((comboitem)(ListAll.SelectedItem)).valueName);
                ListAll.Items.Remove(ListAll.SelectedItem);
            }
        }

        private void FinalAdd_Click(object sender, RoutedEventArgs e)
        {
           
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowExtensions.HideMinimizeAndMaximizeButtons(this);
            
        }
      
    }
}
