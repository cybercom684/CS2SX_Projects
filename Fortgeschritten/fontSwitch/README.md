# fontSwitch

Nintendo Switch Homebrew App, erstellt mit [CS2SX](https://github.com/yourusername/cs2sx).

Wichtig: die font.ttf muss im selben verzeichnis wie die .nro liegen.

## Build

```bash
cs2sx build fontSwitch.csproj
```

## Entwicklung

```bash
cs2sx watch fontSwitch.csproj
```

## Clean

```bash
cs2sx clean fontSwitch.csproj
```

## Icon

Ersetze `icon.jpg` mit deinem eigenen 256x256 JPEG-Icon.

## Struktur

- `Program.cs` — Einstiegspunkt (Klasse erbt von `SwitchApp`)
- `cs2sx.json` — Projektmetadaten (Name, Version, Author, Icon)
- `icon.jpg` — App-Icon (256x256 JPEG)