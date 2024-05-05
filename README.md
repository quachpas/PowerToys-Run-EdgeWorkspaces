# PowerToys Run - Edge Workspaces

This is a plugin for [PowerToys Run](https://github.com/microsoft/PowerToys) 
that allows you to launch [Edge workspaces](https://www.microsoft.com/en-us/edge/features/workspaces).

## How to use

1. Download the latest release.
2. Extract the contents to `%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins`, or where PowerToys is installed					.
3. Restart PowerToys Run and enable the plugin in the settings.

## How to debug

1. Build the project.
2. Modify and run `debug.ps1`.
3. Attach to the process `PowerToys.PowerLauncher`.

## Architecture

The architecture is heavily based on the PowerToys VSCodeWorkspaces plugin. The plugin itself relies on the following files:
- `WorkspacesCache` to read workspaces' IDs (`...\User Data\Profile\Workspaces\WorkspacesCache`)
- `Preferences` to read profiles' name and type at (`...\User Data\Profile\Preferences`)

The plugin then launches the workspace by starting a new Edge process 
with the workspace ID as an argument equivalent to the following:

```cmd
msedge.exe --profile-directory=Default --launch-workspace=$workspaceId
```

If there is no existing Edge process for the corresponding profile, 
an empty Edge window will be opened and the workspace will be launched.

## Acknowledgements

- [Template](https://github.com/8LWXpg/PowerToysRun-PluginTemplate)
- [PowerToys VSCodeWorkspaces](https://github.com/microsoft/PowerToys/tree/main/src/modules/launcher/Plugins/Community.PowerToys.Run.Plugin.VSCodeWorkspaces)
- [Bootstrap Icons](https://github.com/twbs/icons/tree/main)
- [Edge Favorite](https://github.com/davidegiacometti/PowerToys-Run-EdgeFavorite)
