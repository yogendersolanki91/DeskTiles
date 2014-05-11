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
            DeskIcon.DeskIcon icon = new DeskIcon.DeskIcon(item.ToString());
            Lister.Items.Add(icon);
          //  MainPanal.MaxHeight=Title.Height + Tile1.Height;
        
        }

        private void MovingCanvas_Drop(object sender, DragEventArgs e)
        {
        }

        private void Movers_MouseMove(object sender, MouseEventArgs e)
        {
           

        }

        private void Movers_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Lister_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
