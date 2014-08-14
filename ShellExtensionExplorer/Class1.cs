using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.SharpContextMenu;
using System.Runtime.InteropServices;
using SharpShell.Attributes;

namespace ShellExtensionExplorer
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Drive)]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.UnknownFiles)]
    public class AdderExtension:SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override System.Windows.Forms.ContextMenuStrip CreateMenu()
        {
            System.Windows.Forms.ContextMenuStrip contextMenuofExplorer = new System.Windows.Forms.ContextMenuStrip();
            
            var menu=new System.Windows.Forms.ToolStripMenuItem()
            {
                Text="Add to New tile"
            };
            menu.Click+=(sender, args) => NewAdd_Click();
            contextMenuofExplorer.Items.Add(menu);
            


            
            return contextMenuofExplorer;
            
        }

        void NewAdd_Click()
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append(" -n");
                foreach (string s in SelectedItemPaths)
                {
                    stb.Append(" " + s);
                }
                //System.Windows.Forms.MessageBox.Show(@"Desktile" + stb.ToString());
                System.Diagnostics.Process.Start(@"C:\Desktile\desktile.exe", stb.ToString());
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
            }
           
        }
    }
}
