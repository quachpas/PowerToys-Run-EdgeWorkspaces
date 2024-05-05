// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {
    public class EdgeWorkspace {
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


        public EdgeInstance EdgeInstance { get; set; }
        public string ProfilePath { get; set; }

        public string ProfileName { get; set; }
        public string ProfileType { get; set; }

        public Color EdgeWorkspaceColorToColor() {
            switch (Color) {
                case EdgeWorkspaceColor.Blue:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Blue);
                case EdgeWorkspaceColor.Teal:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Teal);
                case EdgeWorkspaceColor.DarkGreen:
                    return System.Drawing.Color.FromKnownColor(KnownColor.DarkGreen);
                case EdgeWorkspaceColor.Green:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Green);
                case EdgeWorkspaceColor.Purple:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Purple);
                case EdgeWorkspaceColor.Violet:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Violet);
                case EdgeWorkspaceColor.Pink:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Pink);
                case EdgeWorkspaceColor.Red:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Red);
                case EdgeWorkspaceColor.Orange:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Orange);
                case EdgeWorkspaceColor.Brown:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Brown);
                case EdgeWorkspaceColor.Gray:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Gray);
                case EdgeWorkspaceColor.LightGray:
                    return System.Drawing.Color.FromKnownColor(KnownColor.LightGray);
                case EdgeWorkspaceColor.LightBlue:
                    return System.Drawing.Color.FromKnownColor(KnownColor.LightBlue);
                case EdgeWorkspaceColor.NoColour:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Transparent);
                default:
                    return System.Drawing.Color.FromKnownColor(KnownColor.Blue);
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
    // Colors picked:
    //  - Blue: 69a1fa
    //  - Teal: 58d3db
    //  - DarkGreen: 5ae0a0
    //  - Green: a4cc6c
    //  - Purple: ab85ff
    //  - Violet: cf87da
    //  - Pink: ee5fb7
    //  - Red: e9835e
    //  - Orange: df8e64
    //  - Brown: ffba66
    //  - Gray: 9e9b99
    //  - LightGray: dfdfdf
    //  - LightBlue: c7dced
    //  - NoColour: transparent
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
    NoColour = 13
}
