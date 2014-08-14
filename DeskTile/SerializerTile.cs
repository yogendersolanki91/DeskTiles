using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kent.Boogaart.Windows.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DeskTile
{
    public class SerializerTile
    {
        [Serializable]
        struct MyMargin
        {
            public double Left;
            public double Top;
            public double Down;
            public double Right;
        }
         [Serializable]
        struct storableInfo{
            public string tempName;
            public string Name;
            public MyMargin margins;
            public string[] filepaths;
            public string color;
            public System.Windows.Size tilesize;
        }
        Dictionary<string, Resizer> finalcontent;
        public SerializerTile()
        {
            finalcontent = new Dictionary<string, Resizer>();
        }
        public  void savetile(Resizer r, string path)
        {
            DeskTileList.TileList list = ((DeskTileList.TileList)(r.Content));
            storableInfo info = new storableInfo();
            info.color = list.BackColor.ToString();
            info.filepaths = new string[list.ListerofTile.Items.Count];
            int i=0;

            foreach (DeskIcon.DeskIconClass d in list.ListerofTile.Items)
            {
                if (d.FilePathValue != "")
                    info.filepaths[i] = d.completePath;
                else
                    //System.Windows.MessageBox.Show(d.FilePathValue[i].ToString());

                i++;   
            }
            MyMargin mrgn = new MyMargin();
            mrgn.Top = r.Margin.Top;
            mrgn.Left = r.Margin.Left;
            info.margins =  mrgn;
            info.tilesize =new System.Windows.Size( r.ActualWidth,r.ActualHeight);
            info.Name = list.FooterValue;
            info.tempName = r.Name;
          //  System.Windows.MessageBox.Show(@path + @"\" + r.Name.ToString());
            System.IO.Stream stream = System.IO.File.Open(path+@"\"+r.Name.ToString(),System.IO.FileMode.Create);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bformatter.Serialize(stream, info);
            stream.Close();
        }
        public Dictionary<string, Resizer> GetSavedTiles(string path)
        {
           
                Stream stream; BinaryFormatter bformatter;
                foreach (string tileFile in Directory.GetFiles(path))
                {
                    storableInfo info;
                    stream = File.Open(tileFile, FileMode.Open);
                    bformatter = new BinaryFormatter();
                    info = (storableInfo)bformatter.Deserialize(stream);
                    stream.Close();
                    Resizer temp = new Resizer();
                    DeskTileList.TileList Content = new DeskTileList.TileList();
                    // System.Windows.MessageBox.Show(info.filepaths.Length.ToString());
                    foreach (string files in info.filepaths)
                    {
                        if (File.Exists(files) || Directory.Exists(files))
                            Content.Add(files);
                        else
                            System.Windows.MessageBox.Show("errrr"+files);
                    }
                    temp.Margin = new System.Windows.Thickness(info.margins.Left, info.margins.Top, 0, 0);
                    Content.FooterValue = info.Name;
                    Content.BackColor = (System.Windows.Media.Brush)(new System.Windows.Media.BrushConverter()).ConvertFromString(info.color);

                    temp.Height = info.tilesize.Height;
                    temp.Width = info.tilesize.Width;
                    Content.ChangeSize(info.tilesize.Width, info.tilesize.Height);
                    temp.Content = Content;
                    finalcontent.Add(info.tempName, temp);

                }

                return finalcontent;
            
           
        }
    }
}
