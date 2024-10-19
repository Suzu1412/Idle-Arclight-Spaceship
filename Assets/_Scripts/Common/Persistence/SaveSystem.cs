using System;
using UnityEngine;

public class SaveSystem : Singleton<SaveSystem>
{
    private GameData _gameData;
    private GameData _fileGameData;
    //private GameData _cloudGameData;
    IDataService _dataService;
    //ICloudService _cloudDataService;

    protected override void Awake()
    {
        base.Awake();

        //_cloudDataService = new CloudDataService();

    }

    public async Awaitable<GameData> LoadGame(string gameName)
    {
        _dataService = new FileDataService(new JsonSerializer());

        _fileGameData = await _dataService.Load(gameName);

        return _fileGameData;
        //SceneManager.LoadScene(gameData.CurrentLevelName);
    }

    public void SaveGame(GameData gameData)
    {
        //_cloudDataService.Save(gameData);
        _dataService.Save(gameData);
    }



}
