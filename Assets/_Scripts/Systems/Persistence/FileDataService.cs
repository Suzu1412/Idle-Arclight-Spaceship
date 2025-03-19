using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class FileDataService : IDataService
{
    private const string SaveFilePath = "gameData.sav";
    private const string BackupFilePath = "gameData.sav.bak";
    private const string HashFilePath = "gameData.hash";

    ISerializer _serializer;
    string _dataPath;
    string _fileExtension;
    string _backUpFileExtension;
    string _hashFilePath;

    public FileDataService(ISerializer serializer)
    {
        _dataPath = Application.persistentDataPath;
        _fileExtension = "sav";
        _backUpFileExtension = "sav.bak";
        _serializer = serializer;
    }

    string GetPathToFile()
    {
        return Path.Combine(_dataPath, SaveFilePath);
    }

    string GetPathToBackUpFile()
    {
        return Path.Combine(_dataPath, BackupFilePath);
    }

    string GetPathToHashPath()
    {
        return Path.Combine(_dataPath, HashFilePath);
    }

    public void Save(GameDataSO data, bool overwrite = true)
    {
        string fileLocation = GetPathToFile();
        string fileBackUpLocation = GetPathToBackUpFile();
        string hashFilePath = GetPathToHashPath();

        if (!overwrite && File.Exists(fileLocation))
        {
            throw new IOException($"The file '{data.Name}.{_fileExtension}' already exists and cannot be overwritten.");
        }

        //File.WriteAllText(fileLocation, data.ToJson());
        //File.WriteAllText(fileBackUpLocation, _serializer.Serialize(data));

        // Serialize GameData to JSON
        string json = data.ToJson();
        string hash = ComputeHash(json);

        // Write main save file
        File.WriteAllText(fileLocation, json);

        // Write hash file
        File.WriteAllText(hashFilePath, hash);

        // Create or update the backup
        File.WriteAllText(fileBackUpLocation, json);

        Debug.Log("Game saved successfully!");
    }

    public async Awaitable<GameDataSO> Load(GameDataSO gameData)
    {
        string fileLocation = GetPathToFile();
        string hashFilePath = GetPathToHashPath();

        await Awaitable.WaitForSecondsAsync(0.1f);

        if (!File.Exists(fileLocation))
        {
            Debug.Log("No File Exists. Create New one");
            gameData.Initialize();
            return gameData;
        }

        // Ensure save file and hash file exist
        if (!File.Exists(fileLocation) || !File.Exists(hashFilePath))
        {
            Debug.LogWarning("Save file or hash file not found. Attempting to load from backup...");
            return LoadFromBackup(gameData);
        }

        // Read save file and hash
        string json = File.ReadAllText(fileLocation);
        string savedHash = File.ReadAllText(hashFilePath);
        string computedHash = ComputeHash(json);

        // Check for tampering
        if (savedHash != computedHash)
        {
            Debug.LogError("Save file tampered with! Attempting to load from backup...");
            return LoadFromBackup(gameData);
        }

        // Load game data
        JsonUtility.FromJsonOverwrite(json, gameData);
        Debug.Log("Game loaded successfully!");
        return gameData;

    }


    public void Delete(string name)
    {
        string fileLocation = GetPathToFile();

        if (File.Exists(fileLocation))
        {
            Debug.Log("existe el archivo");
            File.Delete(fileLocation);
            Debug.Log("borrando archivo");
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

    private GameDataSO LoadFromBackup(GameDataSO gameData)
    {
        string fileLocation = GetPathToBackUpFile();
        string hashFilePath = GetPathToHashPath();

        if (!File.Exists(BackupFilePath))
        {
            Debug.LogError("Backup file not found! Unable to load game data.");
            gameData.Initialize();
            return gameData;
        }

        string backupJson = File.ReadAllText(fileLocation);

        // Validate the backup file by rehashing and checking
        string backupHash = ComputeHash(backupJson);
        if (!File.Exists(hashFilePath) || backupHash != File.ReadAllText(hashFilePath))
        {
            Debug.LogError("Backup file is also tampered with or corrupted.");
            gameData.Initialize();
            return gameData;
        }

        Debug.LogWarning("Game loaded from backup!");

        //return _serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        JsonUtility.FromJsonOverwrite(backupJson, gameData);
        return gameData;
        //return true;
    }

    private string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}