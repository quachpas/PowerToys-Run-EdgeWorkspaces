// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers {

    public class EdgeWorkspaceEntry {

        [JsonPropertyName("accent")]
        public bool Accent { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("collaboratorsCount")]
        public int CollaboratorsCount { get; set; }

        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonPropertyName("connectionUrl")]
        public string ConnectionUrl { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("edgeWorkspaceVersion")]
        public int EdgeWorkspaceVersion { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("isOwner")]
        public bool IsOwner { get; set; }

        [JsonPropertyName("isolated")]
        public bool Isolated { get; set; }

        // unix epoch time
        [JsonPropertyName("lastActiveTime")]
        public double LastActiveTime { get; set; }

        [JsonPropertyName("menuSubtitle")]
        public string MenuSubtitle { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("shared")]
        public bool Shared { get; set; }

        [JsonPropertyName("showDisconnectedUI")]
        public bool ShowDisconnectedUI { get; set; }

        [JsonPropertyName("workspaceFluidStatus")]
        public int WorkspaceFluidStatus { get; set; }
    }
}
