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
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Shell;

namespace DeskTileList
{
  public class DragAndDrop
    {
        static public ListBox DragSource;
        static public Type DragType;

        #region Helper
        private static object GetObjectDataFromPoint(ListBox source, Point point)
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
        #endregion
    
        #region DragEnabled
        public static readonly DependencyProperty DragEnabledProperty =
            DependencyProperty.RegisterAttached("DragEnabled",
                typeof(Boolean),
                typeof(DragAndDrop),
                new FrameworkPropertyMetadata(OnDragEnabledChanged));

        public static void SetDragEnabled(DependencyObject element, Boolean value)
        {
            element.SetValue(DragEnabledProperty, value);
        }

        public static Boolean GetDragEnabled(DependencyObject element)
        {
            return (Boolean)element.GetValue(DragEnabledProperty);
        }

        public static void OnDragEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue == true)
            {
                ListBox listbox = (ListBox)obj;
                listbox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(listbox_PreviewMouseLeftButtonDown);
                listbox.MouseMove += listbox_MouseMove;
               // listbox.PreviewMouseLeftButtonUp += listbox_PreviewMouseLeftButtonUp;
            }
        }

      

      
        #endregion

        #region DropEnabled
        public static readonly DependencyProperty DropEnabledProperty =
            DependencyProperty.RegisterAttached("DropEnabled",
                typeof(Boolean),
                typeof(DragAndDrop),
                new FrameworkPropertyMetadata(OnDropEnabledChanged));

        public static void SetDropEnabled(DependencyObject element, Boolean value)
        {
            element.SetValue(DropEnabledProperty, value);
        }

        public static Boolean GetDropEnabled(DependencyObject element)
        {
            return (Boolean)element.GetValue(DropEnabledProperty);
        }

        public static void OnDropEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue == true)
            {
                ListBox listbox = (ListBox)obj;
                listbox.AllowDrop = true;
                listbox.Drop += new DragEventHandler(listbox_Drop);
            }
        }
        #endregion
        
        static void listbox_Drop(object sender, DragEventArgs e)
        {
            if (DragType != null && isDragging)
            {
                object data = e.Data.GetData(DragType);
                //Change: Check if type is visible, remove only visible items from DragSource...
                if (!DragSource.Equals((ListBox)sender))
                {
                    DragSource.Items.Remove(data);
                    ((ListBox)sender).Items.Add(data);
                }
                
            }
           // MessageBox.Show(e.Data.GetFormats()[0].ToString());


           /* string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            if (droppedFilePaths != null)
            {
                foreach (var path in droppedFilePaths)
                {
                    DeskIcon.DeskIconClass icon= new DeskIcon.DeskIconClass(path);
                    ((ListBox)sender).Items.Add(icon);
                    MessageBox.Show("NOmralaad");
                }
            }*/
            string[] formats = e.Data.GetFormats();
            foreach (string format in formats)
            {
                // Shell items are passed using the "Shell IDList Array" format. 
                if (format == "Shell IDList Array")
                {
                    // Retrieve the ShellObjects from the data object
                  //  MessageBox.Show(ShellObjectCollection.FromDataObject((System.Runtime.InteropServices.ComTypes.IDataObject)e.Data)[0].ParsingName);
                    foreach(ShellObject s in (ShellObjectCollection.FromDataObject((System.Runtime.InteropServices.ComTypes.IDataObject)e.Data)))
                    ((ListBox)sender).Items.Add( new DeskIcon.DeskIconClass(s.ParsingName));
                    e.Handled = true;

                  //  MessageBox.Show("SSSS");
                }
            }
           
        }
        static Point startPoint = new Point();
        static Point CurrentPoint = new Point();
        static bool isDragging = false;
        static bool newDrag = false;
        static object data;
        static void listbox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            DragSource = (ListBox)sender;
            startPoint = e.GetPosition(null);
            data = (object)GetObjectDataFromPoint(DragSource, e.GetPosition(DragSource));
            newDrag = true;
            
        }
        static void listbox_MouseMove(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.GetPosition(null);
            if ((CurrentPoint - startPoint).X >= SystemParameters.MinimumHorizontalDragDistance || (CurrentPoint - startPoint).Y >= SystemParameters.MinimumVerticalDragDistance)
            {
                isDragging = true;
            }
            if (isDragging && e.LeftButton == MouseButtonState.Released)
            {
                isDragging = false;
            }
            if (isDragging && newDrag)
            {
                // Only get type if it is a valid data object
               if (data!=null){
                    DragType = data.GetType();
                    DragDrop.DoDragDrop(DragSource, data, DragDropEffects.Copy);
                    newDrag = false;

               }
                
            }
            
            
        }
       
    }
}
