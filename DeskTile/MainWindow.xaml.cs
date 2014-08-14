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
using DeskIcon;
using System.Windows.Media.Effects;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace DeskTile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TileManager tileSet;
        public SerializerTile Sertile;
        
        System.Windows.Forms.ContextMenu ctx = new System.Windows.Forms.ContextMenu();
        System.Windows.Forms.NotifyIcon mainIOCN = new System.Windows.Forms.NotifyIcon();
       
        public MainWindow()
        {
            InitializeComponent();
     
            mainIOCN.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe");
            mainIOCN.BalloonTipText = " is minimized here.. for more option\n Please Right Click the Icon and Choose Option";
            mainIOCN.BalloonTipTitle = "Pay Attention!!";
            mainIOCN.Visible = true;
            System.Windows.Forms.MenuItem Setting_menu = new System.Windows.Forms.MenuItem();
            Setting_menu.Text = "Settings";
            Setting_menu.Click += Setting_menu_Click;
            System.Windows.Forms.MenuItem Exit_Menu = new System.Windows.Forms.MenuItem();
            Exit_Menu.Text = "Exit";
            Exit_Menu.Click += Exit_Menu_Click;
            System.Windows.Forms.MenuItem Develper_Menu = new System.Windows.Forms.MenuItem();
            Develper_Menu.Click += Develper_Menu_Click;
            Develper_Menu.Text = "Developer";
            ctx.MenuItems.Add(Setting_menu);
            ctx.MenuItems.Add(Develper_Menu);
            ctx.MenuItems.Add(Exit_Menu);
            mainIOCN.ContextMenu = ctx;
            
        }


        
        void Develper_Menu_Click(object sender, EventArgs e)
        {
          //  setup = new Setting(ref tileSet, 2);
//            setup.Show();


        }

        void Exit_Menu_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        void Setting_menu_Click(object sender, EventArgs e)
        {
          //  setup = new Setting(ref tileSet, 1);
           // setup.Show();
        }
        public void GetNewThings(string[] args)
        {
            try {
                if (args[0] == "-n")
                {
                    string id = tileSet.Add("New Tile"); ;
                    DeskTileList.TileList newTile = (DeskTileList.TileList)((Kent.Boogaart.Windows.Controls.Resizer)this.FindName(id)).Content;
                    for (int i = 1; i < args.Length; i++)
                        newTile.Add(args[i]);
                }
            }
            catch(Exception ex){
                
                MessageBox.Show(ex.Message);
                }
        }


        
        string pathtotiles = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\";
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

           // DeskTileList.IconTile TILE = new DeskTileList.IconTile();
            //TILE.Show();
           // System.Threading.Thread.Sleep(500);
          //  TILE.sendtoBack();
           tileSet = new TileManager(this);
            Sertile = new SerializerTile();
          //  Dictionary<string, Kent.Boogaart.Windows.Controls.Resizer> r = Sertile.GetSavedTiles(pathtotiles);
          //  MessageBox.Show(r.Count.ToString());
           // foreach (string s in r.Keys)
           // {

               // tileSet.AddDeserialised(r[s], s);
           //}
          
           

        }
       
        private void RenderCanvas_Loaded(object sender, RoutedEventArgs e)
        {

           // tileSet.Add("jh");
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            DesktopCall.Dekstop.StickThisWindowToDesktop(windowHandle);
            // DesktopCall.Dekstop.WndProc(e);
            System.Windows.Interop.HwndSource SOURCE = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            SOURCE.AddHook(DesktopCall.Dekstop.WndProc);




        }
        bool now = true;

        private void RenderCanvas_Deactivated(object sender, EventArgs e)
        {
          
        }
        Setting setup;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (new Setting(ref tileSet,1)).Show();
        }
    }

}
