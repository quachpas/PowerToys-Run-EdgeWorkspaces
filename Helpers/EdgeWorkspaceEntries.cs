// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {
    public class EdgeWorkspaceEntries {
        [JsonPropertyName("edgeWorkspaceCacheVersion")]
        public int Version { get; set; }

        [JsonPropertyName("workspaces")]
        public List<EdgeWorkspaceEntry> Entries { get; set; }

        public IEnumerator<EdgeWorkspaceEntry> GetEnumerator() {
            return Entries.GetEnumerator();
        }
    }


}
