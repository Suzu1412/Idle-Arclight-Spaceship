using System.Collections.Generic;
using UnityEngine;

public interface ICloudService
{
    void Save(GameDataSO data, bool overwrite = true);
    Awaitable<GameDataSO> Load(string name);
    void Delete(string name);
    void DeleteAll();
    IEnumerable<string> ListSaves();
}
