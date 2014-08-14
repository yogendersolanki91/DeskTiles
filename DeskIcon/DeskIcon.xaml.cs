using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DeskIcon
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DeskIconClass : UserControl
    {
        public string completePath;
        public DeskIconClass(string pathToShortcut)
        {
            InitializeComponent();
            this.IconShow.Source = GetThumbnail(pathToShortcut.ToString());
            completePath = pathToShortcut;
            this.FileName.Text = GetFileString();
           
        }

        public string FilePathValue
        {
            get { return this.FileName.Text.ToString(); }
            set
            {
                this.IconShow.Source = GetThumbnail(value.ToString());
                this.FileName.Text = value ; }
        }
        private string GetFileString()
        {
           // MessageBox.Show(completePath);
            string filename=completePath.Substring(completePath.LastIndexOf(@"\")+1);
            if (filename.Length > 24)
            {
                return filename.Substring(0, 18) + "...";
            }
            else
            {
                return filename;
            }
        }

        private BitmapImage GetThumbnail(string filePath)
        {
          /*  if (System.IO.Directory.Exists(filePath))
            {*/

                ShellObject shellFile = ShellFolder.FromParsingName(filePath);
                BitmapSource shellThumb = shellFile.Thumbnail.ExtraLargeBitmapSource;
                BitmapImage bImg = new BitmapImage();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                var memoryStream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(shellThumb));
                encoder.Save(memoryStream);
                bImg.BeginInit();
                bImg.StreamSource = memoryStream;
                bImg.EndInit();
                return bImg;
                
          /*  }
            else 
            {
                ShellFile shellFile = ShellFile.FromFilePath(filePath);
                BitmapSource shellThumb = shellFile.Thumbnail.LargeBitmapSource;
                BitmapImage bImg = new BitmapImage();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                var memoryStream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(shellThumb));
                encoder.Save(memoryStream);
                bImg.BeginInit();
                bImg.StreamSource = memoryStream;
                bImg.EndInit();
                return bImg;
                
           // }*/
            
            
            

            
        }
    }
}
