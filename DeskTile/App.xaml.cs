using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using SingleInstanceApp4.Util;
using System.Diagnostics;


namespace DeskTile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainWindow MyWindow { get; private set; }

      public App()
          : base()
      { }

      protected override void OnStartup(StartupEventArgs e)
      {
          base.OnStartup(e);

          MyWindow = new MainWindow();
          ProcessArgs(e.Args, true);

          MyWindow.Show();
      }

      public void ProcessArgs(string[] args, bool firstInstance)
      {
          if (!firstInstance)
          { MyWindow.GetNewThings(args); }
      }
    }
}
