using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ZipFile = Ionic.Zip.ZipFile;

namespace TotalCommander
{
    public class MenuActions
    {
        public bool IsTree { get; set; }
        public bool IsFull { get; set; }

        public MenuActions()
        {
            IsTree = false;
            IsFull = true;
        }

        public string About() => "Nume: Nica\n Prenume: Diana - Alexandra\n Grupa: 10LF373\n Email: diana.nica@student.unitbv.ro";

        public void NewTab()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        public void Tree(ref ListView SideRightList, ref ListView SideLeftList, ref TreeView SideLeftListTreeView, ref TreeView SideRightListTreeView)
        {
            IsTree = true;
            IsFull = false;
            SideLeftList.Visibility = Visibility.Hidden;
            SideRightList.Visibility = Visibility.Hidden;
            SideLeftListTreeView.Visibility = Visibility.Visible;
            SideRightListTreeView.Visibility = Visibility.Visible;
        }

        public void Full(ref ListView SideRightList, ref ListView SideLeftList, ref TreeView SideLeftListTreeView, ref TreeView SideRightListTreeView)
        {
            SideLeftListTreeView.Visibility = Visibility.Hidden;
            SideLeftList.Visibility = Visibility.Visible;
            SideRightListTreeView.Visibility = Visibility.Hidden;
            SideRightList.Visibility = Visibility.Visible;
            IsTree = false;
            IsFull = false;
        }

        public void Exit(Window window) => window.Close();

        public void Compare(CommandsForLeftSide commandsForLeft, CommandsForRightSide commandsForRight)
        {
            if (!File.Exists(commandsForLeft.ItemLeft) || !File.Exists(commandsForRight.ItemRight))
                return;
            var ContentCompareResult = new List<CompareByContentLine>();
            string[] linesFile1 = File.ReadAllLines(commandsForLeft.Path + commandsForLeft.ItemLeft);
            string[] linesFile2 = File.ReadAllLines(commandsForRight.Path + commandsForRight.ItemRight);

            string[] linesFile1Sorted = new string[linesFile1.Length];
            linesFile1.CopyTo(linesFile1Sorted, 0);

            string[] linesFile2Sorted = new string[linesFile2.Length];
            linesFile2.CopyTo(linesFile2Sorted, 0);

            Array.Sort(linesFile1Sorted, StringComparer.InvariantCulture);
            Array.Sort(linesFile2Sorted, StringComparer.InvariantCulture);

            var minimumLength = linesFile1.Length < linesFile2.Length ? linesFile1.Length : linesFile2.Length;

            var CommonLines = new HashSet<string>();
            uint lineCounterFile1 = 0, lineCounterFile2 = 0;
            for (; lineCounterFile1 < minimumLength && lineCounterFile2 < minimumLength;)
            {
                if (linesFile1Sorted[lineCounterFile1] == linesFile2Sorted[lineCounterFile2])
                    CommonLines.Add(linesFile1Sorted[lineCounterFile1]);
                if (linesFile1Sorted[lineCounterFile1].CompareTo(linesFile2Sorted[lineCounterFile2]) < 0)
                    ++lineCounterFile1;
                else
                    ++lineCounterFile2;
            }
            if (linesFile1.Length > linesFile2.Length)
            {
                foreach (var line in linesFile1)
                {
                    if (CommonLines.Contains(line))
                        ContentCompareResult.Add(new CompareByContentLine { Text = line, Color = "Black" });
                    else
                        ContentCompareResult.Add(new CompareByContentLine { Text = line, Color = "Red" });
                }
            }
            else
            {
                foreach (var line in linesFile2)
                {
                    if (CommonLines.Contains(line))
                        ContentCompareResult.Add(new CompareByContentLine { Text = line, Color = "Black" });
                    else
                        ContentCompareResult.Add(new CompareByContentLine { Text = line, Color = "Red" });
                }
            }
            //new CompareByContentWindow(ref ContentCompareResult).Show();
        }

        public void PackClick(CommandsForLeftSide commandsForLeftSide, CommandsForRightSide commandsForRightSide, ref TextBox textBox, ref ListView SideRightList, ref ListView SideLeftList)
        {
            using (ZipFile zip = new ZipFile())
            {
                if (commandsForLeftSide.IsVisibleLeft)
                {
                    string item;
                    if (commandsForLeftSide.ItemLeft != null && commandsForRightSide.Path != null)
                    {
                        if (IsFull)
                        {
                            item = commandsForLeftSide.Path + commandsForLeftSide.ItemLeft;
                            if (File.Exists(item))
                                zip.AddFile(item);
                            else
                                zip.AddDirectory(item, commandsForLeftSide.ItemLeft);
                            zip.Save(commandsForRightSide.Path + commandsForLeftSide.ItemLeft +".zip");
                            commandsForRightSide.ChangeListOfDirectories(commandsForRightSide.Path);
                            SideRightList.ItemsSource = commandsForRightSide.Directories;
                        }
                        else
                        {
                            item = commandsForLeftSide.ItemLeft;
                            int pos = commandsForLeftSide.ItemLeft.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                            commandsForLeftSide.ItemLeft = commandsForLeftSide.ItemLeft.Substring(pos + 1);
                            if (File.Exists(item))
                                zip.AddFile(item);
                            else
                                zip.AddDirectory(item, commandsForLeftSide.ItemLeft);
                            zip.Save(commandsForRightSide.ItemRight + commandsForLeftSide.ItemLeft + ".zip");
                        }
                    }
                    else MessageBox.Show("You don`t select an item \nOr Your path to arhive is not found!", "Info");
                }
                else if (commandsForRightSide.IsVisibleRight)
                {
                    string item;
                    if (commandsForRightSide.ItemRight != null && commandsForLeftSide.Path != null)
                    {
                        if (IsFull)
                        {
                            item = commandsForRightSide.Path + commandsForRightSide.ItemRight;
                            if (File.Exists(item))
                                zip.AddFile(item);
                            else
                                zip.AddDirectory(item, commandsForRightSide.ItemRight);
                            zip.Save(commandsForLeftSide.Path + commandsForRightSide.ItemRight + ".zip");
                            commandsForLeftSide.ChangeListOfDirectories(commandsForLeftSide.Path);
                            SideLeftList.ItemsSource = commandsForLeftSide.Directories;
                        }
                        else
                        {
                            item = commandsForRightSide.ItemRight;
                            int pos = commandsForRightSide.ItemRight.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                            commandsForRightSide.ItemRight = commandsForRightSide.ItemRight.Substring(pos + 1);
                            if (File.Exists(item))
                                zip.AddFile(item);
                            else
                                zip.AddDirectory(item, commandsForRightSide.ItemRight);
                            zip.Save(commandsForLeftSide.ItemLeft + commandsForRightSide.ItemRight + ".zip");
                        }
                    }
                    else MessageBox.Show("You don`t select an item \nOr Your path to arhive is not found!", "Info");
                }

            }
        }

        public void UnpackClick(CommandsForLeftSide commandsForLeftSide, CommandsForRightSide commandsForRightSide, ref TextBox textBox, ref ListView SideRightList, ref ListView SideLeftList)
        {
            if (commandsForLeftSide.IsVisibleLeft)
            {
                if (IsFull)
                {
                    var path = commandsForLeftSide.Path + commandsForLeftSide.ItemLeft;
                    ZipFile zip = new ZipFile(path);
                    zip.ExtractAll(commandsForRightSide.Path);
                    commandsForRightSide.ChangeListOfDirectories(commandsForRightSide.Path);
                    SideRightList.ItemsSource = commandsForRightSide.Directories;
                }
                else
                {
                    var item = commandsForLeftSide.ItemLeft;
                    int pos = commandsForLeftSide.ItemLeft.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                    commandsForLeftSide.ItemLeft = commandsForLeftSide.ItemLeft.Substring(pos + 1);
                    ZipFile zip = new ZipFile(item);
                    zip.ExtractAll(commandsForRightSide.ItemRight);
                }
            }
            else
            {
                if (IsFull)
                {
                    var path = commandsForRightSide.Path + commandsForRightSide.ItemRight;
                    ZipFile zip = new ZipFile(path);
                    zip.ExtractAll(commandsForLeftSide.Path);
                    commandsForLeftSide.ChangeListOfDirectories(commandsForLeftSide.Path);
                    SideLeftList.ItemsSource = commandsForLeftSide.Directories;
                }
                else
                {
                    var item = commandsForRightSide.ItemRight;
                    int pos = commandsForRightSide.ItemRight.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
                    commandsForRightSide.ItemRight = commandsForRightSide.ItemRight.Substring(pos + 1);
                    ZipFile zip = new ZipFile(item);
                    zip.ExtractAll(commandsForLeftSide.ItemLeft);
                }
            }
        }
    }
}
