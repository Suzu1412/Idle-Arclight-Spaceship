using System.Collections.Generic;
using UnityEngine;

public interface ICloudService
{
    void Save(GameData data, bool overwrite = true);
    Awaitable<GameData> Load(string name);
    void Delete(string name);
    void DeleteAll();
    IEnumerable<string> ListSaves();
}
