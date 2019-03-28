using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TotalCommander
{
    /// <summary>
    /// Interaction logic for VerticalArrangement.xaml
    /// </summary>
    public partial class VerticalArrangement : Window
    {
        public VerticalArrangement()
        {
            InitializeComponent();
            commandsForLeft = new CommandsForLeftSide();
            commandsForRight = new CommandsForRightSide();
            actions = new Actions();
            menuActions = new MenuActions();
            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem()
                {
                    // Set the header
                    Header = drive,
                    // And the full path
                    Tag = drive
                };

                item.Items.Add(null);
                item.Expanded += Folder_Expanded;
                SideLeftListTreeView.Items.Add(item);
            }

            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem()
                {
                    // Set the header
                    Header = drive,
                    // And the full path
                    Tag = drive
                };

                item.Items.Add(null);
                item.Expanded += Folder_Expanded;
                SideRightListTreeView.Items.Add(item);
            }
        }
        internal CommandsForLeftSide commandsForLeft;
        internal CommandsForRightSide commandsForRight;
        internal Actions actions;
        internal MenuActions menuActions;

        public void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            item.Items.Clear();

            var fullPath = (string)item.Tag;
            var directories = new List<string>();

            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            directories.ForEach(directoryPath =>
            {
                var subItem = new TreeViewItem()
                {
                        // Set header as folder name
                        Header = GetFileFolderName(directoryPath),
                        // And tag as full path
                        Tag = directoryPath
                };

                subItem.Items.Add(null);
                subItem.Expanded += Folder_Expanded;
                item.Items.Add(subItem);
            });

            var files = new List<string>();
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            files.ForEach(filePath =>
            {
                var subItem = new TreeViewItem()
                {
                        // Set header as file name
                        Header = GetFileFolderName(filePath),
                        // And tag as full path
                        Tag = filePath
                };
                item.Items.Add(subItem);
            });
        }

        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ComboBox.ItemsSource = DriveInfo.GetDrives().Where(dr => dr.IsReady == true).ToList();
            this.ComboBox2.ItemsSource = DriveInfo.GetDrives().Where(dr => dr.IsReady == true).ToList();

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                AddButton(sender, e);
            }
            else if (e.Key == Key.F7)
            {
                ViewTxtButton(sender, e);
            }
            else if (e.Key == Key.F4)
            {
                CopyButton(sender, e);
            }
            else if (e.Key == Key.F6)
            {
                MoveButton(sender, e);
            }
            else if (e.Key == Key.F5)
            {
                DeleteButton(sender, e);
            }
            else if (e.Key == Key.F8)
            {
                BackButton(sender, e);
            }
            else if (e.Key == Key.F9)
            {
                ExitClick(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                PackClick(sender, e);
            }

            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                UnpackClick(sender, e);
            }
            else if (e.Key == Key.Tab)
            {
                NewTabClick(sender, e);
            }
            else if (e.Key == Key.D2 && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                TreeClick(sender, e);
            }
            else if (e.Key == Key.D3 && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                VerticalClick(sender, e);
            }
            else if (e.Key == Key.D1 && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                FullClick(sender, e);
            }

            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                AboutClick(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                ExitClick(sender, e);
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => commandsForLeft.ComboBoxSelect(ref ComboBox, ref SideLeftList, commandsForRight, ref PathLeftSide);

        private void SideLeftList_DoubleClick(object sender, MouseEventArgs e) => commandsForLeft.SideLeftListDoubleClick(ref SideLeftList, commandsForRight, ref PathLeftSide, menuActions, sender);

        private void SideLeftList_RightClick(object sender, MouseEventArgs e) => commandsForLeft.SideLeftListRightClick(ref SideLeftList, menuActions, sender);

        private void ComboBox_SelectionChanged2(object sender, SelectionChangedEventArgs e) => commandsForRight.ComboBoxSelect(ref ComboBox2, ref SideRightList, commandsForLeft, ref PathRightSide);

        private void SideRightList_RightClick(object sender, MouseEventArgs e) => commandsForRight.SideRightListRightClick(ref SideRightList, menuActions, sender, commandsForLeft);

        private void SideRightList_DoubleClick(object sender, MouseEventArgs e) => commandsForRight.SideRightListDoubleClick(ref SideRightList, commandsForLeft, ref PathRightSide, menuActions, sender);

        private void AddButton(object sender, RoutedEventArgs e) => actions.Add(commandsForLeft, commandsForRight, ref TextBox, menuActions, ref SideLeftList, ref SideRightList);

        private void CopyButton(object sender, RoutedEventArgs e) => actions.Copy(commandsForLeft, commandsForRight, ref TextBox, menuActions, ref SideLeftList, ref SideRightList);

        private void DeleteButton(object sender, RoutedEventArgs e) => actions.Delete(commandsForLeft, commandsForRight, ref TextBox, menuActions, ref SideLeftList, ref SideRightList);

        private void ViewTxtButton(object sender, RoutedEventArgs e) => actions.ViewTxt(commandsForLeft, commandsForRight, ref TextBox, menuActions);

        private void MoveButton(object sender, RoutedEventArgs e) => actions.Move(commandsForLeft, commandsForRight, ref TextBox, menuActions, ref SideLeftList, ref SideRightList);

        private void BackButton(object sender, RoutedEventArgs e) => actions.Back(ref SideLeftList, ref SideRightList, ref PathLeftSide, ref PathRightSide, commandsForLeft, commandsForRight, ref TextBox);

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(menuActions.About(), "Student details", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                System.Diagnostics.Process.Start("https://elearning.unitbv.ro/login/index.php");
            }
        }

        private void NewTabClick(object sender, RoutedEventArgs e) => menuActions.NewTab();

        private void VerticalClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void TreeClick(object sender, RoutedEventArgs e) => menuActions.Tree(ref SideRightList, ref SideLeftList, ref SideLeftListTreeView, ref SideRightListTreeView);

        private void FullClick(object sender, RoutedEventArgs e) => menuActions.Full(ref SideRightList, ref SideLeftList, ref SideLeftListTreeView, ref SideRightListTreeView);

        private void ExitClick(object sender, RoutedEventArgs e) => menuActions.Exit(this);

        private void CompareClick(object sender, RoutedEventArgs e)
        {

        }

        private void PackClick(object sender, RoutedEventArgs e) => menuActions.PackClick(commandsForLeft, commandsForRight, ref TextBox, ref SideRightList, ref SideLeftList);

        private void UnpackClick(object sender, RoutedEventArgs e) => menuActions.UnpackClick(commandsForLeft, commandsForRight, ref TextBox, ref SideRightList, ref SideLeftList);
    }
}
