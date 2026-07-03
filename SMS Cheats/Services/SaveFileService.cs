using SMS_Cheats.Models;

namespace SMS_Cheats.Services;

public class SaveFileService : ISaveFileService
{
    public SaveFile Load(string filePath)
    {
        return new SaveFile
        {
            FilePath = filePath,
            RawLines = File.ReadAllLines(filePath)
        };
    }

    public void Save(SaveFile saveFile)
    {
        File.WriteAllLines(saveFile.FilePath, saveFile.RawLines);
    }

    public string CreateBackup(string filePath)
    {
        var backupPath = GetUniqueFilePath(Path.ChangeExtension(filePath, "bak"));
        File.Copy(filePath, backupPath, true);
        return backupPath;
    }

    public string? FindMostRecentSave(string folder)
    {
        return new DirectoryInfo(folder)
            .GetFiles("*.es3")
            .OrderByDescending(f => f.LastWriteTime)
            .FirstOrDefault()
            ?.FullName;
    }

    public T ReadValue<T>(SaveFile saveFile, string searchKey)
    {
        for (int i = 0; i < saveFile.RawLines.Length; i++)
        {
            if (saveFile.RawLines[i].Contains(searchKey))
            {
                var foundValue = saveFile.RawLines[i].Split(':').Last();
                foundValue = foundValue.Replace("\"", "");
                foundValue = foundValue.Remove(foundValue.Length - 1, 1).Trim();
                return (T)Convert.ChangeType(foundValue, typeof(T));
            }
        }
        return default!;
    }

    public void Apply<T>(SaveFile saveFile, string searchKey, string newValue, bool isArray)
    {
        // Validate the value converts correctly before modifying anything
        _ = (T)Convert.ChangeType(newValue, typeof(T));

        for (int i = 0; i < saveFile.RawLines.Length; i++)
        {
            var line = saveFile.RawLines[i];
            if (!line.Contains(searchKey))
                continue;

            // Preserve indentation and trailing comma so the file format survives round-trips
            var indent = line[..(line.Length - line.TrimStart().Length)];
            var comma = line.TrimEnd().EndsWith(',') ? "," : "";
            saveFile.RawLines[i] = typeof(T) == typeof(string)
                ? $"{indent}{searchKey} : \"{newValue}\"{comma}"
                : $"{indent}{searchKey} : {newValue}{comma}";

            if (!isArray)
                break;
        }
    }

    public void ReplaceArray(SaveFile saveFile, string searchKey, string joinedValues)
    {
        var lines = saveFile.RawLines.ToList();
        for (int i = 0; i < lines.Count; i++)
        {
            if (!lines[i].Contains(searchKey))
                continue;

            // Find the closing bracket, then swap everything between for one line of values
            int close = i + 1;
            while (close < lines.Count && !lines[close].TrimStart().StartsWith(']'))
                close++;
            if (close >= lines.Count)
                return;

            var closeLine = lines[close];
            var indent = closeLine[..(closeLine.Length - closeLine.TrimStart().Length)] + "\t";
            lines.RemoveRange(i + 1, close - (i + 1));
            lines.Insert(i + 1, indent + joinedValues);
            saveFile.RawLines = [.. lines];
            return;
        }
    }

    private static string GetUniqueFilePath(string filePath)
    {
        if (!File.Exists(filePath))
            return filePath;

        var directory = Path.GetDirectoryName(filePath)!;
        var nameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        var extension = Path.GetExtension(filePath);

        int counter = 1;
        string newPath;
        do
        {
            newPath = Path.Combine(directory, $"{nameWithoutExt} ({counter}){extension}");
            counter++;
        } while (File.Exists(newPath));

        return newPath;
    }
}
