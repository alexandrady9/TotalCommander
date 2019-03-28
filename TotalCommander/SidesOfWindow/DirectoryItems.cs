using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TotalCommander
{
    class DirectoryItems
    {
        private string name;
        private string lastWriteTime;
        private string extension;
        private string length;
        private BitmapImage icon;

        public string Extension { get => extension; set => extension = value; }
        public string LastWriteTime { get => lastWriteTime; set => lastWriteTime = value; }
        public string Name { get => name; set => name = value; }
        public string Length { get => length; set => length = value; }
        public BitmapImage Icon { get => icon; set => icon = value; }

        public DirectoryItems(string name, string lastWriteTime, string extension, string length, BitmapImage icon)
        {
            Name = name;
            LastWriteTime = lastWriteTime;
            Extension = extension;
            Length = length;
            Icon = icon;
        }
    }
}
