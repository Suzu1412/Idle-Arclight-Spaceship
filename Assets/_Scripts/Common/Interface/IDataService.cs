using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    void Save(GameData data, bool overwrite = true);
    Awaitable<GameData> Load(string name);
    void Delete(string name);
    void DeleteAll();
    IEnumerable<string> ListSaves();
}