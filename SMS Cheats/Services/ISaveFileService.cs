using SMS_Cheats.Models;

namespace SMS_Cheats.Services;

public interface ISaveFileService
{
    SaveFile Load(string filePath);
    void Save(SaveFile saveFile);
    string CreateBackup(string filePath);
    string? FindMostRecentSave(string folder);
    T ReadValue<T>(SaveFile saveFile, string searchKey);
    void Apply<T>(SaveFile saveFile, string searchKey, string newValue, bool isArray);
}
