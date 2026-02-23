# SMS Cheats

Windows desktop save editor for *Supermarket Simulator*. This is a WPF (.NET Framework 4.8) app that loads a `.es3` save file, edits a few fields, and writes the changes back after creating a backup.

**What it can edit**
1. Money
2. Current store level
3. Completed checkout count
4. Current day
5. Store name
6. Vehicle fuel (sets gas level to max `200`)

**How it works**
1. Load a save file by browsing for a `.es3` file, or click **Load Most Recent Save** to auto-select the newest save in:
   `AppData/LocalLow/Nokta Games/Supermarket Simulator`
2. Edit values and click **Apply Changes**.
3. A `.bak` backup is created next to the save before writing the updated file.

**Build**
Open `SMS Cheats.sln` in Visual Studio and build. The project targets .NET Framework 4.8.
