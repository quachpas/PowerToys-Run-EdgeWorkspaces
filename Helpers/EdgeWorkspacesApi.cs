// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text.Json;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {
    public class EdgeWorkspacesApi {

        public List<EdgeWorkspace> Workspaces { 
            get {
                var results = new List<EdgeWorkspace>();

                foreach (var edgeInstance in EdgeInstances.Instances) {                     
                    var edgeProfiles = edgeInstance.profiles;
                    if (edgeProfiles == null) {
                        continue;
                    }
                    foreach (var profile in edgeProfiles) {
                        var edgeWorkspaces = GetWorkspaces(edgeInstance, profile);
                        if (edgeWorkspaces == null) { continue; }
                        foreach (var workspace in edgeWorkspaces) {
                            results.Add(workspace);
                        }
                    }
                }

                return results;
            } 
        }

        private string readJson(string path) {
            string fileContent = string.Empty;
            try {
                fileContent = File.ReadAllText(path);
            }
            catch (Exception e) {
                var message = $"Error reading file: {e.Message}";
                Log.Exception(message, e, GetType());
            }
            return fileContent;
        }

        private List<EdgeWorkspace> GetWorkspaces(EdgeInstance edgeInstance, string profilePath) {
            var results = new List<EdgeWorkspace>();
            var wkfileContent = readJson(EdgeInstance.getWorkspacesCache(profilePath));
            var prefsfileContent = readJson(EdgeInstance.getProfilePreferences(profilePath));

            try {
                EdgeWorkspaceEntries entries = JsonSerializer.Deserialize<EdgeWorkspaceEntries>(wkfileContent);
                EdgeProfilePreferences prefs = JsonSerializer.Deserialize<EdgeProfilePreferences>(prefsfileContent);

                foreach (var entry in entries) {
                    var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(entry.LastActiveTime);
                    dateTime.AddSeconds(entry.LastActiveTime);
                    dateTime.ToLocalTime();
                    results.Add(new EdgeWorkspace {
                        ID = entry.Id,
                        Name = entry.Name,
                        Description = entry.MenuSubtitle,
                        Active = entry.Active,
                        IsOwner = entry.IsOwner,
                        IsShared = entry.Shared,
                        CollaboratorsCount = entry.CollaboratorsCount,
                        LastActiveTime = dateTime,
                        Version = entry.EdgeWorkspaceVersion,
                        Color = (EdgeWorkspaceColor)entry.Color,
                        EdgeInstance = edgeInstance,
                        ProfilePath = profilePath,
                        ProfileName = prefs.Profile.Name,
                        ProfileType = prefs.Sync.EdgeAccountType.ToString()
                    });
                }
            }
            catch (Exception e) {
                var message = $"Error parsing WorkspacesCache file: {e.Message}";
                Log.Exception(message, e, GetType());
            }

            return results;
        }
    }
}
