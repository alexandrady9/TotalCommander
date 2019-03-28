using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TotalCommander
{
    public class Actions
    {
        public void Add(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox, MenuActions menuActions, ref ListView SideLeftList, ref ListView SideRightList)
        {
            string newPath = null;
            if (commandsForLeft.IsVisibleLeft && commandsForLeft.Path != null)
            {
                if (commandsForLeft.ItemLeft != null)
                {
                    if (menuActions.IsFull)
                        newPath = Path.Combine(commandsForLeft.Path + commandsForLeft.ItemLeft, "New Folder");
                    else newPath = Path.Combine(commandsForLeft.ItemLeft, "New Folder");

                    TextBox.Text = "In " + commandsForLeft.Path + commandsForLeft.ItemLeft + " was created a new folder";
                    Directory.CreateDirectory(newPath);
                    commandsForLeft.ChangeListOfDirectories(commandsForLeft.Path);
                    SideLeftList.ItemsSource = commandsForLeft.Directories;
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }
            else if (commandsForRight.IsVisibleRight)
            {
                if (commandsForRight.ItemRight != null && commandsForRight.Path != null)
                {
                    if (menuActions.IsFull)
                        newPath = Path.Combine(commandsForRight.Path + commandsForRight.ItemRight, "New Folder");
                    else newPath = System.IO.Path.Combine(commandsForRight.ItemRight, "New Folder");

                    TextBox.Text = "In " + commandsForRight.Path + commandsForRight.ItemRight + " was created a new folder";
                    Directory.CreateDirectory(newPath);
                    commandsForRight.ChangeListOfDirectories(commandsForRight.Path);
                    SideRightList.ItemsSource = commandsForRight.Directories;
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }
        }

        public void Copy(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox, MenuActions menuActions, ref ListView SideLeftList, ref ListView SideRightList)
        {
            if (commandsForLeft.IsVisibleLeft)
            {
                if (commandsForLeft.ItemLeft != null && commandsForRight.Path != null)
                {
                    if (menuActions.IsFull)
                    {
                        string pathNew = commandsForLeft.Path + commandsForLeft.ItemLeft;
                        if (File.Exists(pathNew))
                        {
                            File.Copy(pathNew, commandsForRight.Path + commandsForLeft.ItemLeft, true);
                            TextBox.Text = "File " + commandsForLeft.ItemLeft + " from: " + commandsForLeft.Path + ", was copied in: " + commandsForRight.Path;
                        }
                        else
                        {
                            DirectoryCopy(pathNew, commandsForRight.Path + commandsForLeft.ItemLeft);
                            TextBox.Text = "Directory " + commandsForLeft.ItemLeft + " from: " + commandsForLeft.Path + ", was copied in: " + commandsForRight.Path;
                        }
                        commandsForRight.ChangeListOfDirectories(commandsForRight.Path);
                        SideRightList.ItemsSource = commandsForRight.Directories;
                    }

                    else
                    {
                        string item = commandsForLeft.ItemLeft;
                        int pos = commandsForLeft.ItemLeft.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                        commandsForLeft.ItemLeft = commandsForLeft.ItemLeft.Substring(pos + 1);
                        if (File.Exists(item))
                            File.Copy(item, commandsForRight.ItemRight + "\\" + commandsForLeft.ItemLeft, true);
                        else
                            DirectoryCopy(item, commandsForRight.ItemRight + "\\" + commandsForLeft.ItemLeft);
                        commandsForRight.ChangeListOfDirectories(commandsForRight.ItemRight);
                        SideRightList.ItemsSource = commandsForRight.Directories;
                    }
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }

            else if (commandsForRight.IsVisibleRight)
            {
                if (commandsForRight.ItemRight != null && commandsForLeft.Path != null)
                {
                    if (menuActions.IsFull)
                    {
                        string pathNew = commandsForRight.Path + commandsForRight.ItemRight;
                        if (File.Exists(pathNew))
                        {
                            File.Copy(pathNew, commandsForLeft.Path + commandsForRight.ItemRight, true);
                            TextBox.Text = "File " + commandsForRight.ItemRight + " from: " + commandsForRight.Path + ", was copied in: " + commandsForLeft.Path;
                        }
                        else
                        {
                            DirectoryCopy(pathNew, commandsForLeft.Path + commandsForRight.ItemRight);
                            TextBox.Text = "File " + commandsForRight.ItemRight + " from: " + commandsForRight.Path + ", was copied in: " + commandsForLeft.Path;
                        }
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.Path);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }

                    else
                    {
                        string item = commandsForRight.ItemRight;
                        int pos = commandsForRight.ItemRight.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                        commandsForRight.ItemRight = commandsForRight.ItemRight.Substring(pos + 1);
                        if (File.Exists(item))
                            File.Copy(item, commandsForLeft.ItemLeft + "\\" + commandsForRight.ItemRight, true);
                        else
                            DirectoryCopy(item, commandsForLeft.ItemLeft + "\\" + commandsForRight.ItemRight);
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.ItemLeft);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        public void Delete(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox, MenuActions menuActions, ref ListView SideLeftList, ref ListView SideRightList)
        {
            if (commandsForLeft.IsVisibleLeft)
            {
                if (commandsForLeft.ItemLeft != null)
                {
                    if (menuActions.IsFull)
                    {
                        string pathNew = commandsForLeft.Path + commandsForLeft.ItemLeft;
                        MessageBoxResult result = MessageBox.Show("Do you really want to move the selected item to the recycle bin?", "Total Commander", MessageBoxButton.YesNoCancel);

                        if (File.Exists(pathNew))
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(pathNew);
                                TextBox.Text = "File " + commandsForLeft.ItemLeft + " deleted";
                            }
                        }
                        else
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                Directory.Delete(pathNew, true);
                                TextBox.Text = "File " + commandsForLeft.ItemLeft + " deleted";
                            }
                        }
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.Path);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Do you really want to move the selected item to the recycle bin?", "Total Commander", MessageBoxButton.YesNoCancel);

                        if (File.Exists(commandsForLeft.ItemLeft))
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(commandsForLeft.ItemLeft);
                                TextBox.Text = "File " + commandsForLeft.ItemLeft + " deleted";
                            }
                        }
                        else
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                Directory.Delete(commandsForLeft.ItemLeft, true);
                                TextBox.Text = "File " + commandsForLeft.ItemLeft + " deleted";
                            }
                        }
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.ItemLeft);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }
                }
                else MessageBox.Show("You don`t select an item", "Info");
            }

            else if (commandsForRight.IsVisibleRight)
            {
                if (commandsForRight.ItemRight != null)
                {
                    if (menuActions.IsFull)
                    {
                        string pathNew = commandsForRight.Path + commandsForRight.ItemRight;
                        MessageBoxResult result = MessageBox.Show("Do you really want to move the selected item to the recycle bin?", "Total Commander", MessageBoxButton.YesNoCancel);

                        if (File.Exists(pathNew))
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(pathNew);
                                TextBox.Text = "File " + commandsForRight.ItemRight + " deleted";
                            }
                        }
                        else
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                Directory.Delete(pathNew, true);
                                TextBox.Text = "File " + commandsForRight.ItemRight + " deleted";
                            }
                        }
                        commandsForRight.ChangeListOfDirectories(commandsForRight.Path);
                        SideRightList.ItemsSource = commandsForRight.Directories;
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Do you really want to move the selected item to the recycle bin?", "Total Commander", MessageBoxButton.YesNoCancel);

                        if (File.Exists(commandsForRight.ItemRight))
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(commandsForRight.ItemRight);
                                TextBox.Text = "File " + commandsForRight.ItemRight + " deleted";
                            }
                        }
                        else
                        {
                            if (result == MessageBoxResult.Yes)
                            {
                                Directory.Delete(commandsForRight.ItemRight, true);
                                TextBox.Text = "File " + commandsForRight.ItemRight + " deleted";
                            }
                        }
                        commandsForRight.ChangeListOfDirectories(commandsForRight.ItemRight);
                        SideRightList.ItemsSource = commandsForRight.Directories;

                    }
                }
                else MessageBox.Show("You don`t select an item", "Info");
            }
        }

        public void ViewTxt(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox, MenuActions menuActions)
        {
            if (commandsForLeft.IsVisibleLeft)
            {
                if (commandsForLeft.ItemLeft != null)
                {
                    string newPath = commandsForLeft.Path + commandsForLeft.ItemLeft;

                    if (File.Exists(newPath))
                        Process.Start("notepad.exe", newPath);
                    else TextBox.Text = "Selected item is not exists.";
                }
                else MessageBox.Show("You don`t select an item", "Info");
            }

            else if (commandsForRight.IsVisibleRight)
            {
                if (commandsForRight.ItemRight != null)
                {
                    string newPath = commandsForRight.Path + commandsForRight.ItemRight;
                    if (File.Exists(newPath))
                        Process.Start("notepad.exe", newPath);
                    else TextBox.Text = "Selected item is not exists.";
                }
                else MessageBox.Show("You don`t select an item", "Info");

            }
        }

        public void Move(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox, MenuActions menuActions, ref ListView SideLeftList, ref ListView SideRightList)
        {
            if (commandsForLeft.IsVisibleLeft)
            {
                if (commandsForLeft.ItemLeft != null && commandsForRight.Path != null) 
                {
                    if (menuActions.IsFull)
                    {
                        string newPath = commandsForLeft.Path + commandsForLeft.ItemLeft;
                        if (File.Exists(newPath))
                        {
                            File.Move(newPath, commandsForRight.Path + commandsForLeft.ItemLeft);
                            TextBox.Text = "File " + commandsForLeft.ItemLeft + " from: " + commandsForLeft.Path + commandsForLeft.ItemLeft + ", was moved in: " + commandsForRight.Path;
                        }
                        else
                        {
                            Directory.Move(newPath, commandsForRight.Path + commandsForLeft.ItemLeft);
                            TextBox.Text = "Directory " + commandsForLeft.ItemLeft + " from: " + commandsForLeft.Path + commandsForLeft.ItemLeft + ", was moved in: " + commandsForRight.Path;
                        }
                        commandsForRight.ChangeListOfDirectories(commandsForRight.Path);
                        SideRightList.ItemsSource = commandsForRight.Directories;
                    }
                    else
                    {
                        string item = commandsForLeft.ItemLeft;
                        int pos = commandsForLeft.ItemLeft.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                        commandsForLeft.ItemLeft = commandsForLeft.ItemLeft.Substring(pos + 1);
                        if (File.Exists(item))
                            File.Move(item, commandsForRight.ItemRight + "\\" + commandsForLeft.ItemLeft);
                        else
                            Directory.Move(item, commandsForRight.ItemRight + "\\" + commandsForLeft.ItemLeft);
                        commandsForRight.ChangeListOfDirectories(commandsForRight.ItemRight);

                        SideRightList.ItemsSource = commandsForRight.Directories;
                    }
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }

            else if (commandsForRight.IsVisibleRight)
            {
                if (commandsForRight.ItemRight != null && commandsForLeft.Path != null)
                {
                    if (menuActions.IsFull)
                    {
                        string newPath = commandsForRight.Path + commandsForRight.ItemRight;
                        if (File.Exists(commandsForRight.Path + commandsForRight.ItemRight))
                        {
                            File.Move(newPath, commandsForLeft.Path + commandsForRight.ItemRight);
                            TextBox.Text = "File " + commandsForRight.ItemRight + " from: " + commandsForRight.Path + commandsForRight.ItemRight + ", was moved in: " + commandsForLeft.Path;
                        }
                        else
                        {
                            Directory.Move(newPath, commandsForLeft.Path + commandsForRight.ItemRight);
                            TextBox.Text = "Directory " + commandsForRight.ItemRight + " from: " + commandsForRight.Path + commandsForRight.ItemRight + ", was moved in: " + commandsForLeft.Path;
                        }
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.Path);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }

                    else
                    {
                        string item = commandsForRight.ItemRight;
                        int pos = commandsForRight.ItemRight.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                        commandsForRight.ItemRight = commandsForRight.ItemRight.Substring(pos + 1);
                        if (File.Exists(item))
                            File.Move(item, commandsForLeft.ItemLeft + "\\" + commandsForRight.ItemRight);
                        else
                            Directory.Move(item, commandsForLeft.ItemLeft + "\\" + commandsForRight.ItemRight);
                        commandsForLeft.ChangeListOfDirectories(commandsForLeft.ItemLeft);
                        SideLeftList.ItemsSource = commandsForLeft.Directories;
                    }
                }
                else MessageBox.Show("You don`t select an item\nOr your path is not found!", "Info");
            }
        }

        public void Back(ref ListView SideLeftList,ref ListView SideRightList, ref TextBlock PathLeftSide, ref TextBlock PathRightSide, CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight, ref TextBox TextBox)
        {
            if (commandsForLeft.IsVisibleLeft)
            {
                string newPath = commandsForLeft.Path.Substring(0, commandsForLeft.Path.Length - 1);
                int pos = newPath.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                if (pos != -1)
                {
                    commandsForLeft.Path = newPath.Substring(0, pos + 1);
                    commandsForLeft.ChangeListOfDirectories(commandsForLeft.Path);
                    SideLeftList.ItemsSource = commandsForLeft.Directories;
                }
                PathLeftSide.Text = commandsForLeft.Path;

                commandsForLeft.IsVisibleLeft = true;
                commandsForRight.IsVisibleRight = false;
            }

            else if (commandsForRight.IsVisibleRight)
            {
                string newPath = commandsForRight.Path.Substring(0, commandsForRight.Path.Length - 1);
                int pos = newPath.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                if (pos != -1)
                {
                    commandsForRight.Path = newPath.Substring(0, pos + 1);
                    commandsForRight.ChangeListOfDirectories(commandsForRight.Path);
                    SideRightList.ItemsSource = commandsForRight.Directories;
                }
                PathRightSide.Text = commandsForRight.Path;

                commandsForLeft.IsVisibleLeft = false;
                commandsForRight.IsVisibleRight = true;
            }
        }
    }
}
