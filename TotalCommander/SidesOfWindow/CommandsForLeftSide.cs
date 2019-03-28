using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TotalCommander
{
    public class CommandsForLeftSide
    {
        internal ObservableCollection<DirectoryItems> Directories { get; set; }
        public bool IsVisibleLeft { get; set; }
        public string ItemLeft { get; set; }
        public string Path { get; set; }

        public CommandsForLeftSide()
        {
            IsVisibleLeft = false;
            ItemLeft = null;
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

        public void ComboBoxSelect(ref ComboBox comboBox, ref ListView SideLeftList, CommandsForRightSide commandsForRight, ref TextBlock PathLeftSide)
        {
            Path = comboBox.SelectedItem.ToString();
            ChangeListOfDirectories(Path);

            SideLeftList.ItemsSource = Directories;
            IsVisibleLeft = true;
            commandsForRight.IsVisibleRight = false;

            PathLeftSide.Text = Path;
        }

        public void SideLeftListDoubleClick(ref ListView SideLeftList,CommandsForRightSide commandsForRight,ref TextBlock PathLeftSide, MenuActions menu, object sender)
        {
            if (menu.IsFull)
            {
                var item = (DirectoryItems)SideLeftList.SelectedItem;
                if (item != null)
                {
                    if (File.Exists(Path + item.Name))
                        Process.Start(Path + item.Name);
                    Path = Path + item.Name + "\\";
                    ChangeListOfDirectories(Path);
                    SideLeftList.ItemsSource = Directories;
                    PathLeftSide.Text = Path;
                }
            }
            else
            {
                var leftPanel = (TreeView)sender;
                var item = (string)(((TreeViewItem)leftPanel.SelectedItem).Tag);
                if (item != null)
                    if (File.Exists(item))
                        Process.Start(item);
            }

            IsVisibleLeft = true;
            commandsForRight.IsVisibleRight = false;
        }

        public void SideLeftListRightClick(ref ListView SideLeftList, MenuActions menu, object sender)
        {
            if (menu.IsFull)
            {
                var item = (DirectoryItems)SideLeftList.SelectedItem;
                if (item != null)
                    ItemLeft = item.Name;
            }
            else if (menu.IsTree)
            {
                var leftPanel = (TreeView)sender;
                var item = (string)(((TreeViewItem)leftPanel.SelectedItem).Tag);
                if (item != null)
                    ItemLeft = item;
            }

            IsVisibleLeft = true;
        }

    }
}
