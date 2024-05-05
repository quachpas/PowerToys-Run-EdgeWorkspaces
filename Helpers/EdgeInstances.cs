// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {
    public class EdgeInstances {
        private static readonly string _userAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string _programFilesx86Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static List<EdgeInstance> Instances { get; set; } = new List<EdgeInstance>();

        public static void LoadEdgeInstances() {
            Instances.Clear();

            // For: (Edge, Edge Dev, Edge Beta)
            // Search for Edge executables in Program Files (x86)/Microsoft/Edge */Application/msedge.exe
            // Edge Canary installs to %LOCALAPPDATA%\Microsoft\Edge SxS\Application\msedge.exe
            var MicrosoftEdgePath = Path.Combine(_programFilesx86Path, "Microsoft");
            var paths = Directory.GetDirectories(MicrosoftEdgePath, "Edge*", SearchOption.TopDirectoryOnly).Where(
                path => path.EndsWith("Dev") || path.EndsWith("Beta") || path.EndsWith("Edge")
            ).ToList();

            var canaryPath = Path.Combine(_userAppDataPath, "Microsoft", "Edge SxS");
            if (Directory.Exists(canaryPath)) {
                paths.Add(canaryPath);
            }

            foreach (var path in paths) {
                var directoryName = new DirectoryInfo(path).Name;
                var executablePath = Path.Combine(path, "Application", "msedge.exe");
                var userDataPath = Path.Combine(_userAppDataPath, "Microsoft", directoryName);
                if (File.Exists(executablePath) && Directory.Exists(userDataPath)) {
                    var edgeVersion = new EdgeVersion();
                    // C:\Program Files (x86)\Microsoft\Edge Beta\Application\125.0.2535.13\VisualElements
                    var iconPath = Path.Combine(
                        path, 
                        "Application", 
                        new DirectoryInfo(Path.Combine(path, "Application")).GetDirectories().First().Name, 
                        "VisualElements"
                    );
                    if (directoryName.Contains("Dev")) {
                        edgeVersion = EdgeVersion.Dev;
                        iconPath = Path.Combine(iconPath, "SmallLogoDev.png");
                    }
                    else if (directoryName.Contains("Beta")) {
                        edgeVersion = EdgeVersion.Beta;
                        iconPath = Path.Combine(iconPath, "SmallLogoBeta.png");
                    }
                    else if (directoryName.Contains("SxS")) {
                        edgeVersion = EdgeVersion.Canary;
                        iconPath = Path.Combine(iconPath, "SmallLogoCanary.png");
                    }
                    else {
                        edgeVersion = EdgeVersion.Stable;
                        iconPath = Path.Combine(iconPath, "SmallLogo.png");
                    }


                    EdgeInstance instance = new EdgeInstance {
                        ExecutablePath = executablePath,
                        AppData = userDataPath,
                        IconPath = iconPath,
                        Version = edgeVersion
                    };
                    Instances.Add(instance);
                }
            }
        }
    }
}
