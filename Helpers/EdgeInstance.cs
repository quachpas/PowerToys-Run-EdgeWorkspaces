// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {

    public enum EdgeVersion {
        Stable = 1,
        Dev = 2,
        Beta = 4,
        Canary = 3
    }
    public class EdgeInstance {
        public string ExecutablePath { get; set; } = string.Empty;
        public string AppData { get; set; } = string.Empty;
        public string IconPath { get; set; }
        public EdgeVersion Version { get; set; }

        public List<string> profiles {
            get {
                var profiles = new List<string>();
                var profilePath = Path.Combine(AppData, "User Data");

                if (Directory.Exists(profilePath)) {
                    var dirs = Directory.EnumerateDirectories(profilePath).
                        Where(
                        p => Regex.IsMatch(new DirectoryInfo(p).Name, @"(Default|Profile \d+)")
                        && File.Exists(getWorkspacesCache(p)));
                    profiles.AddRange(dirs);
                }

                return profiles;
            }
        }

        public static string getWorkspacesCache(string profilePath) {
            return Path.Combine(profilePath, "Workspaces", "WorkspacesCache");
        }

        public static string getProfilePreferences(string profilePath) {
            return Path.Combine(profilePath, "Preferences");
        }
    }

    public class EdgeProfilePreferencesProfile {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public enum EdgeAccountType {
        Local = 0,
        Personal = 1,
        Work = 2,
    }
    public class EdgeProfilePreferencesSync {
        [JsonPropertyName("edge_account_type")]
        public EdgeAccountType EdgeAccountType { get; set; }
    }
    public class EdgeProfilePreferences {

        [JsonPropertyName("profile")]
        public EdgeProfilePreferencesProfile Profile { get; set; }

        [JsonPropertyName("sync")]
        public EdgeProfilePreferencesSync Sync { get; set; }
    }
}
