# Supermarket Save Editor

A cross-platform save file editor for *Supermarket Simulator*. Built with Avalonia UI and .NET 8 — runs on Windows and Linux.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square)
![Avalonia](https://img.shields.io/badge/Avalonia-11-7c68ee?style=flat-square)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux-30363d?style=flat-square)

---

## What you can edit

| Field | Type |
|---|---|
| Store Level | Integer |
| Store Points (XP) | Integer |
| Money | Decimal |
| Completed Checkouts | Integer |
| Current Day | Integer |
| Store Name | Text |
| Store Expansion Level | Integer |
| Vehicle Fuel | Toggle (sets all vehicles to max `200`) |
| Product Licenses | Toggle (unlocks all licenses, IDs 21–47) |
| Loans | Toggle (clears all outstanding debt) |

---

## How to use

1. **Load a save** — click **Browse File** to pick a `.es3` file manually, or **Auto Load** to grab the most recent save from:
   ```
   %USERPROFILE%\AppData\LocalLow\Nokta Games\Supermarket Simulator
   ```
2. **Edit** the fields you want to change. Toggle **REFUEL ALL** to max out all vehicle fuel.
3. **Apply Changes** — a `.bak` backup is automatically created next to the original before anything is written.

> Backups are numbered to avoid overwriting (`save.bak`, `save (1).bak`, etc.)

---

## Building

**Requirements:** .NET 8 SDK

```bash
git clone https://github.com/JEwaldt27/Super-Market-Simulator-Game-Save-Editor
cd "Super-Market-Simulator-Game-Save-Editor/SMS Cheats"
dotnet build
dotnet run
```

Or open `SMS Cheats.sln` in Visual Studio 2022 / Rider.

---

## Credits

[JEwaldt27](https://github.com/JEwaldt27) · [rthomas2395](https://github.com/rthomas2395)
