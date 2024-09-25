// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Community.PowerToys.Run.Plugin.EdgeWorkspaces.Helpers;
using ManagedCommon;
using System.ComponentModel;
using System.Windows.Input;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.EdgeWorkspaces {
    public class Main : IPlugin, IPluginI18n, IContextMenu {
        public static string PluginID => "93A140605B1B49EFBE4312FED59E2569";

        // Options
        private PluginInitContext _context;
        private string _iconPath;
        private bool _disposed;

        // Metadata
        public string Name => Properties.Resources.plugin_name;
        public string Description => Properties.Resources.plugin_description;

        public Main() {
            EdgeInstances.LoadEdgeInstances();
        }

        private readonly EdgeWorkspacesApi _edgeWorkspaces = new EdgeWorkspacesApi();

        public List<Result> Query(Query query) {
            ArgumentNullException.ThrowIfNull(query);

            var results = new List<Result>();

            _edgeWorkspaces.Workspaces.ForEach(wk =>
                results.Add(new Result {
                    Title = wk.Name + (wk.ProfileName.Length > 0 ? $" ({wk.ProfileType}: {wk.ProfileName})" : ""),
                    SubTitle = wk.Description,
                    // IcoPath = wk.EdgeInstance.IconPath,
                    ToolTipData = new ToolTipData(Properties.Resources.launch_workspace,""),
                    Icon = wk.Icon,
                    ContextData = wk,
                    Score = StringMatcher.FuzzySearch(query.Search, wk.Name).Score,
                    Action = action => {
                        bool hide = false;
                        try {
                            var process = wk.launchWorkspace();
                            hide = true;
                        }
                        catch (Win32Exception e) {
                            var name = $"Plugin: {_context.CurrentPluginMetadata.Name}";
                            var msg = "Can't Open this file";
                            _context.API.ShowMsg(name, msg, string.Empty);
                            hide = false;
                        }
                        return hide;
                    }
                })
            );

            results = results.Where(r => r.Title.Contains(query.Search, StringComparison.InvariantCultureIgnoreCase)).ToList();
            results = results.OrderByDescending(r => r.Score).ToList();
            if (
                query.Search.Equals(string.Empty) || 
                query.Search.Replace(" ", string.Empty).Equals(string.Empty)
            ) {
                results = results.OrderBy(x => x.Title).ToList();
            }

            return results;
        }

        public List<ContextMenuResult> LoadContextMenus(Result selectedResult) {
            ArgumentNullException.ThrowIfNull(selectedResult);

            if (!(selectedResult?.ContextData is EdgeWorkspace)) {
                return new List<ContextMenuResult>();
            }
            
            var menus = new List<ContextMenuResult>();

            if (selectedResult.ContextData is EdgeWorkspace wk) {
                menus.Add(new ContextMenuResult {
                    PluginName = Properties.Resources.plugin_name,
                    Title = Properties.Resources.open_profile,
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE7EE", // OtherUser
                    AcceleratorKey = Key.P,
                    AcceleratorModifiers = ModifierKeys.Control | ModifierKeys.Shift,
                    Action = _ => {
                        var wk = selectedResult.ContextData as EdgeWorkspace;
                        var process = wk.openProfile();
                        return true;
                    }
                });
            }

            return menus;
        }

        public void Init(PluginInitContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public string GetTranslatedPluginTitle() {
            return Properties.Resources.plugin_name;
        }

        public string GetTranslatedPluginDescription() {
            return Properties.Resources.plugin_description;
        }


        private void OnThemeChanged(Theme oldtheme, Theme newTheme) {
            UpdateIconPath(newTheme);
        }

        private void UpdateIconPath(Theme theme) {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite) {
                _iconPath = "Images/EdgeWorkspaces.light.png";
            }
            else {
                _iconPath = "Images/EdgeWorkspaces.dark.png";
            }
        }

        public void ReloadData() {
            if (_context is null) {
                return;
            }

            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed && disposing) {
                if (_context != null && _context.API != null) {
                    _context.API.ThemeChanged -= OnThemeChanged;
                }

                _disposed = true;
            }
        }
    }
}
