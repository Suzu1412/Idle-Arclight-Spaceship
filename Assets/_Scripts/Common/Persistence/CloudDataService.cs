using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;

public class CloudDataService : IDataService
{
    private const string KEY_PLAYER_NAME = "Player";

    public CloudDataService() 
    {
        Initialize();
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();

        // Actualmente usamos un método anónimo. Esto solo servirá en el actual dispositivo
        // Para cualquier plataforma tenemos que tener un método persistente
        // Google Para android, Facebook. Steam para PC. Etc. 

        // TODO: Implementar lo anterior mencionado

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
    }

    public void Delete(string name)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteAll()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<string> ListSaves()
    {
        throw new System.NotImplementedException();
    }

    public GameData Load(string name)
    {
        var a = LoadDictionary();
        GameData gameData = new GameData();
        
        return gameData;
    }

    public void Save(GameData data, bool overwrite = true)
    {
    }

    private async Awaitable<GameData> LoadDictionary()
    {
        var dictionary = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        GameData gameData = new();

        if (dictionary.TryGetValue(KEY_PLAYER_NAME, out var keyName))
        {
            gameData = keyName.Value.GetAs<GameData>();
            Debug.Log($"keyName: {keyName.Value.GetAs<GameData>()}");
        }

        return gameData;
    }
}
