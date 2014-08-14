
using System;
using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;

namespace SingleInstanceExample
{
    public sealed class SingleInstanceManager : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        { (new SingleInstanceManager()).Run(args); }

        public SingleInstanceManager()
        { IsSingleInstance = true; }

        public DeskTile.App App { get; private set; }

        protected override bool OnStartup(StartupEventArgs e)
        {
            App = new DeskTile.App();
            App.Run();
            return false;
        }

        protected override void OnStartupNextInstance(
          StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            App.MyWindow.Activate();
            App.ProcessArgs(eventArgs.CommandLine.ToArray(), false);
        }
    }
}
