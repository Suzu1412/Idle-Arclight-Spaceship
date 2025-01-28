using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    void Save(GameDataSO data, bool overwrite = true);
    Awaitable<GameDataSO> Load(GameDataSO gameData);
    void Delete(string name);
    void DeleteAll();
    IEnumerable<string> ListSaves();
}