# this script uses [gsudo](https://github.com/gerardog/gsudo)

Push-Location
Set-Location $PSScriptRoot

sudo {
	$ptPath = "C:\Users\Pascal\AppData\Local\PowerToys"

	@(
		'PowerToys.Common.UI.dll',
		'PowerToys.ManagedCommon.dll',
		'PowerToys.Settings.UI.Lib.dll',
		'Wox.Infrastructure.dll',
		'Wox.Plugin.dll'
	) | ForEach-Object {
		Copy-Item $ptPath\$_ ./Lib/x64
	}
}

Pop-Location
