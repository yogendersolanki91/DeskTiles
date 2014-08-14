using System;
using System.Collections.Generic;
using System.Linq;
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
using DeskIcon;
using ShellTestApp;
using System.IO;


namespace DeskTileList
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TileList : UserControl
    {
        public TileList()
        {
            InitializeComponent();
           
        }
        public void Add(Object item)
        {
            DeskIcon.DeskIconClass icon = new DeskIcon.DeskIconClass(item.ToString());
            Lister.Items.Add(icon);
          //  MainPanal.MaxHeight=Title.Height + Tile1.Height;
        
        }
        
        public void ChangeTextOfFooter(string text)
        {
            Title.Text = text;
        }
        public void ChangeSize(double width, double height)
        {
            MainPanal.Height = height;
            MainPanal.Width = width;
        }
        public string FooterValue
        {
            get { return Title.Text.ToString(); }
            set { Title.Text = value.ToString(); }
        }
        public Brush BackColor
        {
            get { return MainPanal.Background; }
            set { 
                MainPanal.Background = (Brush)value; }
        }
        public ListBox ListerofTile
        {
            get { return Lister; }
        }
        public void updateColor(string color)
        {
            var bc = new BrushConverter();
            MainPanal.Background = (Brush)bc.ConvertFrom(color);
        }
        bool isDragging = false;
        Point clicekd = new Point();

        private void Lister_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private object GetObjectDataFromPoint(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                //get the object from the element
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    // try to get the object value for the corresponding element
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    //get the parent and we will iterate again
                    if (data == DependencyProperty.UnsetValue)
                        element = VisualTreeHelper.GetParent(element) as UIElement;

                    //if we reach the actual listbox then we must break to avoid an infinite loop
                    if (element == source)
                        return null;
                }

                //return the data that we fetched only if it is not Unset value, 
                //which would mean that we did not find the data
                if (data != DependencyProperty.UnsetValue)
                    return data;
            }

            return null;
        }
        private void ListBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeskIcon.DeskIconClass item = (DeskIcon.DeskIconClass)GetObjectDataFromPoint(Lister, e.GetPosition(Lister));
            if (item != null)
            {
               
                
                ShellContextMenu scm = new ShellContextMenu();
                FileInfo[] files = new FileInfo[1];
                  files[0] = new FileInfo(item.completePath);
                  scm.ShowContextMenu(new System.Drawing.Point((int)e.GetPosition(null).X, (int)e.GetPosition(null).Y)); ;

            }
       
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                DeskIcon.DeskIconClass item = (DeskIcon.DeskIconClass)GetObjectDataFromPoint(Lister, e.GetPosition(Lister));
                if (item != null)
                    if (item.completePath.EndsWith(".lnk") && File.Exists(item.completePath))
                    {

                        ShellShortcut shrtct = new ShellShortcut(item.completePath);
                        //  MessageBox.Show(shrtct.Path);
                        System.Diagnostics.Process.Start(shrtct.Path);
                    }
                    else if (File.Exists(item.completePath) || Directory.Exists(item.completePath))
                        System.Diagnostics.Process.Start(item.completePath);
                    else
                        System.Windows.Forms.MessageBox.Show("File/Folder Not found", "Error 01", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Lister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                while (Lister.SelectedItems.Count > 0)
                {
                    
                    Lister.Items.Remove(Lister.SelectedItems[0]);
                }
             
            }
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            DeskIcon.DeskIconClass item = (DeskIcon.DeskIconClass)GetObjectDataFromPoint(Lister, e.GetPosition(Lister));
            if (Lister.SelectedItem!= null)
            {

                ShellContextMenu ctxMnu = new ShellContextMenu();
                FileInfo[] arrFI = new FileInfo[1];
                arrFI[0] = new FileInfo(((DeskIcon.DeskIconClass)Lister.SelectedItem).completePath);     
                ctxMnu.ShowContextMenu(arrFI,(new System.Drawing.Point((int)e.GetPosition(null).X, (int)e.GetPosition(null).Y))); ;
               // MessageBox.Show("dssd");

            }
            
        }
            
     
       
    }
}
