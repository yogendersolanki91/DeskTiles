using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grip = Kent.Boogaart.Windows.Controls;

namespace DeskTile
{
    
    struct Movers
    {
       public  bool isMoving;
       public System.Windows.Point MoveStartPoint;
    }
    public class TileManager
    {
       public MainWindow DesktopWindow;
       Dictionary<string, Movers> MoveDict=new Dictionary<string,Movers>();
       public Dictionary<string, Grip.Resizer> ALLTILES = new Dictionary<string, Grip.Resizer>();
    
        public TileManager(MainWindow mainWin)
        {
            DesktopWindow = mainWin;
          
          
          
            
          
        }
       
        public string Add(string Name){
            Grip.Resizer temp = new Grip.Resizer();
            temp.SizeChanged += temp_SizeChanged;
            temp.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            temp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            temp.MouseDown += temp_MouseDown;
            temp.MouseUp += temp_MouseUp;
            temp.MouseMove += temp_MouseMove;
            temp.Margin = new System.Windows.Thickness(100, 100, 0, 0);
            temp.MouseRightButtonDown+=temp_MouseRightButtonDown;
            string tempName = "Tile" + (new Random()).Next().ToString() + (new Random().Next()).ToString();
                temp.Name = tempName;
   
             DeskTileList.TileList Content= new DeskTileList.TileList();
             Content.FooterValue=Name;
             temp.Width = 200;
             temp.Height = 200;
            temp.Content = Content;
            MoveDict[tempName] = new Movers();
          temp.MouseLeave+=temp_MouseLeave;
            DesktopWindow.RegisterName(tempName,temp); 

            DesktopWindow.TopContainer.Children.Add(temp);
            ALLTILES[tempName] = temp;
            return tempName;
        }
        string pathtotiles = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\DeskTile";
        void temp_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SerializerTile s = new SerializerTile();
            foreach (Grip.Resizer r in ALLTILES.Values)
            {
                s.savetile(r, pathtotiles);
               // System.Windows.MessageBox.Show(pathtotiles);
            }
        }
        public void AddDeserialised(Grip.Resizer temp,string tempName)
        {
            temp.SizeChanged += temp_SizeChanged;
            temp.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            temp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            temp.MouseDown += temp_MouseDown;
            temp.MouseUp += temp_MouseUp;
            temp.MouseMove += temp_MouseMove;
            MoveDict[tempName] = new Movers();
            ALLTILES[tempName] = temp;
            temp.Name = tempName;
            temp.MouseRightButtonDown += temp_MouseRightButtonDown;
            temp.MouseLeave+=temp_MouseLeave;
            DesktopWindow.RegisterName(tempName, temp);
            //System.Windows.MessageBox.Show(tempName);
            DesktopWindow.TopContainer.Children.Add(temp);
        }
        
        void temp_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
                   
        }
        public void Remove(string UniqueName)
        {
            Grip.Resizer rs = (Grip.Resizer)DesktopWindow.TopContainer.FindName(UniqueName);

            if (rs != null)
            {
                DesktopWindow.TopContainer.Children.Remove(rs);
                ALLTILES.Remove(UniqueName);
                System.IO.File.Delete(pathtotiles + @"\" + UniqueName);
            }

        }
        void temp_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DeskTileList.TileList mainTile = (DeskTileList.TileList)((Grip.Resizer)(sender)).Content;
            System.Windows.Point currentOntile = e.GetPosition(mainTile);
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && MoveDict[((Grip.Resizer)(sender)).Name].isMoving)
            {
              ((Grip.Resizer)(sender)).Margin = new System.Windows.Thickness(((Grip.Resizer)(sender)).Margin.Left + e.GetPosition(null).X - MoveDict[((Grip.Resizer)(sender)).Name].MoveStartPoint.X, ((Grip.Resizer)(sender)).Margin.Top + e.GetPosition(null).Y - MoveDict[((Grip.Resizer)(sender)).Name].MoveStartPoint.Y, 0, 0);
                Movers update= new Movers();
                update.MoveStartPoint=e.GetPosition(null);
                update.isMoving = true;
                MoveDict[((Grip.Resizer)(sender)).Name] = update;
               // mainTile.ChangeTextOfFooter(currentOntile.ToString());
            }

            
        }

        void temp_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DeskTileList.TileList mainTile = (DeskTileList.TileList)((Grip.Resizer)(sender)).Content;
            mainTile.ReleaseMouseCapture();
           // mainTile.ChangeTextOfFooter("Not Moving");
            Movers destroy= new Movers();
            destroy.isMoving=false;
            destroy.MoveStartPoint=new System.Windows.Point();
            MoveDict[((Grip.Resizer)(sender)).Name] = destroy;
            

        }
      
        void temp_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DeskTileList.TileList mainTile = (DeskTileList.TileList)((Grip.Resizer)(sender)).Content;
            if ((mainTile.ActualHeight - 30) < e.GetPosition(mainTile).Y)
            {
               //mainTile.ChangeTextOfFooter("Moving");
                Movers update= new Movers();
                update.isMoving = true; 
                update.MoveStartPoint=e.GetPosition(null);
                MoveDict[((Grip.Resizer)(sender)).Name] = update;

            }
            mainTile.CaptureMouse();
            
        }

        void temp_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            DeskTileList.TileList mainTile = (DeskTileList.TileList)((Grip.Resizer)(sender)).Content;
            
            mainTile.ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }

    }
}
