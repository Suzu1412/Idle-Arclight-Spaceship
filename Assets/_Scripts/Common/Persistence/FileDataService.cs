using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataService : IDataService
{
    ISerializer _serializer;
    string _dataPath;
    string _fileExtension;
    string _backUpFileExtension;

    public FileDataService(ISerializer serializer)
    {
        _dataPath = Application.persistentDataPath;
        _fileExtension = "sav";
        _backUpFileExtension = "sav.bak";
        _serializer = serializer;
    }

    string GetPathToFile(string fileName)
    {
        return Path.Combine(_dataPath, string.Concat(fileName, ".", _fileExtension));
    }

    string GetPathToBackUpFile(string fileName)
    {
        return Path.Combine(_dataPath, string.Concat(fileName, ".", _backUpFileExtension));
    }

    public void Save(GameData data, bool overwrite = true)
    {
        string fileLocation = GetPathToFile(data.Name);
        string fileBackUpLocation = GetPathToBackUpFile(data.Name);

        if (!overwrite && File.Exists(fileLocation))
        {
            throw new IOException($"The file '{data.Name}.{_fileExtension}' already exists and cannot be overwritten.");
        }

        File.WriteAllText(fileLocation, data.ToJson());
        File.WriteAllText(fileBackUpLocation, _serializer.Serialize(data));
    }

    public async Awaitable<GameData> Load(string name)
    {
        string fileLocation = GetPathToFile(name);

        await Awaitable.WaitForSecondsAsync(0.1f);

        if (!File.Exists(fileLocation))
        {
            Debug.Log("No File Exists. Create New one");
            return null;
        }

        return _serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
    }


    public void Delete(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
    }

    public void DeleteAll()
    {
        foreach (string filePath in Directory.GetFiles(_dataPath))
        {
            File.Delete(filePath);
        }
    }

    public IEnumerable<string> ListSaves()
    {
        foreach (string path in Directory.EnumerateFiles(_dataPath))
        {
            if (Path.GetExtension(path) == _fileExtension)
            {
                yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}