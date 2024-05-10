// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {
    public class EdgeWorkspace {
        // VSCodeHelper
        private static BitmapImage Bitmap2BitmapImage(Bitmap bitmap) {
            using (var memory = new MemoryStream()) {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool IsOwner { get; set; }

        public bool IsShared { get; set; }
        public int CollaboratorsCount { get; set; }

        public DateTime LastActiveTime { get; set; }

        public int Version { get; set; }

        public EdgeWorkspaceColor Color { get; set; }

        public ImageSource Icon() => WorkspaceIconBitmap;

        public BitmapImage WorkspaceIconBitmap {
            get {
                // Get the icon from the EdgeInstance
                var img = new Bitmap(EdgeInstance.IconPath);
                // Add a rounded color outline to the icon
                var color = EdgeWorkspaceColorToColor();
                var bitmap = new Bitmap(img.Width, img.Height);
                using (var g = Graphics.FromImage(bitmap)) {
                    g.Clear(System.Drawing.Color.Transparent);
                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                    using var pen = new System.Drawing.Pen(color, 20);
                    g.DrawEllipse(pen, 0, 0, img.Width, img.Height);
                }
                return Bitmap2BitmapImage(bitmap);
            }
        }

        public EdgeInstance EdgeInstance { get; set; }
        public string ProfilePath { get; set; }

        public string ProfileName { get; set; }
        public string ProfileType { get; set; }

        public System.Drawing.Color EdgeWorkspaceColorToColor() {
            switch (Color) {
                case EdgeWorkspaceColor.Blue:
                    return System.Drawing.Color.FromArgb(0x69, 0xa1, 0xfa);
                case EdgeWorkspaceColor.Teal:
                    return System.Drawing.Color.FromArgb(0x58, 0xd3, 0xdb);
                case EdgeWorkspaceColor.DarkGreen:
                    return System.Drawing.Color.FromArgb(0x5a, 0xe0, 0xa0);
                case EdgeWorkspaceColor.Green:      
                    return System.Drawing.Color.FromArgb(0xa4, 0xcc, 0x6c);
                case EdgeWorkspaceColor.Purple:
                    return System.Drawing.Color.FromArgb(0xab, 0x85, 0xff);
                case EdgeWorkspaceColor.Violet:
                    return System.Drawing.Color.FromArgb(0xcf, 0x87, 0xda);
                case EdgeWorkspaceColor.Pink:
                    return System.Drawing.Color.FromArgb(0xee, 0x5f, 0xb7);
                case EdgeWorkspaceColor.Red:
                    return System.Drawing.Color.FromArgb(0xe9, 0x83, 0x5e); 
                case EdgeWorkspaceColor.Orange:
                    return System.Drawing.Color.FromArgb(0xdf, 0x8e, 0x64);
                case EdgeWorkspaceColor.Brown: 
                    return System.Drawing.Color.FromArgb(0xff, 0xba, 0x66);
                case EdgeWorkspaceColor.Gray:
                    return System.Drawing.Color.FromArgb(0x9e, 0x9b, 0x99); 
                case EdgeWorkspaceColor.LightGray:
                    return System.Drawing.Color.FromArgb(0xdf, 0xdf, 0xdf); 
                case EdgeWorkspaceColor.LightBlue:  
                    return System.Drawing.Color.FromArgb(0xc7, 0xdc, 0xed);
                case EdgeWorkspaceColor.NoColor:
                    return System.Drawing.Color.Transparent;
                default :
                    return System.Drawing.Color.Transparent;
            }
        }
        public Process launchWorkspace() {
            var profileName = new DirectoryInfo(ProfilePath).Name;
            var process = new ProcessStartInfo {
                FileName = EdgeInstance.ExecutablePath,
                Arguments = $"--profile-directory=\"{profileName}\" --launch-workspace=\"{ID}\"",
                UseShellExecute = true
            };
            return Process.Start(process);
        }
        public Process openProfile() {
            var profileName = new DirectoryInfo(ProfilePath).Name;
            var process = new ProcessStartInfo {
                FileName = EdgeInstance.ExecutablePath,
                Arguments = $"--profile-directory=\"{profileName}\"",
                UseShellExecute = true
            };
            return Process.Start(process);
        }
    }


}

public enum EdgeWorkspaceColor {
    // Follows the order of the colours in the Edge UI
    Blue = 0,
    Teal = 1,
    DarkGreen = 2,
    Green = 3,
    Purple = 4,
    Violet = 5,
    Pink = 6,
    Red = 7,
    Orange = 8,
    Brown = 9,
    Gray = 10,
    LightGray = 11,
    LightBlue = 12,
    NoColor = 13
}
