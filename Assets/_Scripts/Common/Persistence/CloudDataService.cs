using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;

public class CloudDataService : ICloudService
{
    private const string KEY_PLAYER_NAME = "Player";

    public CloudDataService()
    {
        Initialize();
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();

        // Actualmente usamos un m?todo an?nimo. Esto solo servir? en el actual dispositivo
        // Para cualquier plataforma tenemos que tener un m?todo persistente
        // Google Para android, Facebook. Steam para PC. Etc. 

        // TODO: Implementar lo anterior mencionado

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

    public async Awaitable<GameData> Load(string name)
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var dictionary = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        GameData gameData = new();

        if (dictionary.TryGetValue(KEY_PLAYER_NAME, out var keyName))
        {
            gameData = keyName.Value.GetAs<GameData>();
        }

        return gameData;
    }

    public async void Save(GameData data, bool overwrite = true)
    {
        var dataDictionary = new Dictionary<string, object> { { KEY_PLAYER_NAME, data } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(dataDictionary);

    }
}
