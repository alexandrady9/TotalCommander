using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TotalCommander
{
    public class CommandsForRightSide
    {
        internal ObservableCollection<DirectoryItems> Directories { get; set; }
        public bool IsVisibleRight { get; set; }
        public string ItemRight { get; set; }
        public string Path { get; set; }

        public CommandsForRightSide()
        {
            IsVisibleRight = false;
            ItemRight = null;
            Path = "C:\\";
        }

        public void ChangeListOfDirectories(string path)
        {
            Directories = new ObservableCollection<DirectoryItems>();

            DirectoryInfo di = new DirectoryInfo(Path);

            DirectoryInfo[] dire = di.GetDirectories();
            FileInfo[] dirs = di.GetFiles();

            foreach (FileInfo diNext in dirs)
                Directories.Add(new DirectoryItems(diNext.Name, diNext.LastWriteTime.ToString(), diNext.Extension, String.Format("{0:N2} {1}", (double)diNext.Length / 1024, "Kb"), new BitmapImage(new Uri(@"Images/file.png", UriKind.Relative))));

            foreach (DirectoryInfo dir in dire)
                Directories.Add(new DirectoryItems(dir.Name, dir.LastWriteTime.ToString(), "", "<DIR>", new BitmapImage(new Uri(@"Images/folder.png", UriKind.Relative))));
        }

        public void ComboBoxSelect(ref ComboBox comboBox, ref ListView SideRightList, CommandsForLeftSide commands, ref TextBlock PathRightSide)
        {
            Path = comboBox.SelectedItem.ToString();
            ChangeListOfDirectories(Path);

            SideRightList.ItemsSource = Directories;
            IsVisibleRight = true;
            commands.IsVisibleLeft = false;

            PathRightSide.Text = Path;
        }

        public void SideRightListDoubleClick(ref ListView SideRightList, CommandsForLeftSide commands, ref TextBlock PathRightSide, MenuActions menu, object sender)
        {
            if (menu.IsFull)
            {
                var item = (DirectoryItems)SideRightList.SelectedItem;
                if (item != null)
                {
                    if (File.Exists(Path + item.Name))
                        Process.Start(Path + item.Name);
                    Path = Path + item.Name + "\\";
                    ChangeListOfDirectories(Path);
                    SideRightList.ItemsSource = Directories;
                    PathRightSide.Text = Path;
                }
            }
            else if (menu.IsTree)
            {
                var rightPanel = (TreeView)sender;
                var item = (string)(((TreeViewItem)rightPanel.SelectedItem).Tag);
                if (item != null)
                {
                    if (File.Exists(item))
                        Process.Start(item);
                }
            }

             IsVisibleRight = true;
            commands.IsVisibleLeft = false;
        }


        public void SideRightListRightClick(ref ListView SideRightList, MenuActions menu, object sender, CommandsForLeftSide commands)
        {

            if (menu.IsFull)
            {
                var item = (DirectoryItems)SideRightList.SelectedItem;
                if (item != null)
                    ItemRight = item.Name;
            }
            else if (menu.IsTree)
            {
                var rightPanel = (TreeView)sender;
                var item = (string)(((TreeViewItem)rightPanel.SelectedItem).Tag);
                if (item != null)
                   ItemRight = item;
            }

            commands.IsVisibleLeft = false;
            IsVisibleRight = true;
        }
    }
}
