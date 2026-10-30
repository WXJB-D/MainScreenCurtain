# MainScreenCurtain

[中文](README.zh-CN.md) · English

<img src="docs/icon.png" width="88" align="right" alt="MainScreenCurtain icon">

A small Windows utility that covers the primary display with a solid black window. Your other monitors stay usable. Left-click the primary display and the window closes.

![MainScreenCurtain demo](docs/demo.gif)

## What it does

- Covers only the primary monitor with pure black.
- Leaves every other monitor alone. The mouse cursor stays visible.
- Closes on a single left click, so the desktop comes back immediately.
- Ignores clicks during the first 0.7 seconds, which keeps the second click of a double-click from closing it.
- Exits by itself when the primary display changes or the window gets minimized.

It does not change your resolution, brightness, display mode, registry, or power settings. Close the process and everything is back.

## Requirements

Windows 10 or Windows 11.

The prebuilt executable runs on the .NET 10 Desktop Runtime. Install it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0) if Windows asks for it.

Building from source needs the .NET 10 SDK.

## Download

Get the latest `MainScreenCurtain-vX.Y.Z-win-x64.zip` from [Releases](https://github.com/WXJB-D/MainScreenCurtain/releases/latest), unpack it, and run `MainScreenCurtain.exe`.

The executable is not code signed. Windows SmartScreen may warn on first launch. Choose "More info" and then "Run anyway" if you trust the source, or build it yourself.

## Usage

1. Start `MainScreenCurtain.exe`.
2. The primary display turns black. The cursor stays visible, and the other displays keep working.
3. Left-click anywhere on the primary display to close the curtain.

The first 0.7 seconds are a safety window. A double-click that launches the app will not immediately close it.

Move the mouse to another display if you want the cursor out of the way. A white cursor sitting still on an OLED panel for a long time can also leave a mark, though it takes much longer than static content.

## Build from source

```bat
.\build.cmd
```

The script publishes a single-file, framework-dependent executable to `publish\MainScreenCurtain.exe`.

Or run the publish command directly.

```bat
dotnet publish MainScreenCurtain.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o publish
```

## Notes

- Windows notifications usually appear on the primary display and can float above the black window. Turn on Focus Assist for long sessions.
- The primary taskbar is covered too. A taskbar on a secondary display still works.
- The app does not stop Windows from turning displays off according to your power plan.
- If the primary display changes, for example with `Win+P` set to "Second screen only", the curtain closes instead of covering the wrong screen.
- To force an exit, end `MainScreenCurtain.exe` in Task Manager. Pressing `Win+D` minimizes the window, and the app exits on its own.

## License

MIT. See [LICENSE](LICENSE).
