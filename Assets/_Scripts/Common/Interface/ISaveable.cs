using UnityEngine;

public interface ISaveable
{
    void SaveData(GameDataSO gameData);
    void LoadData(GameDataSO gameData);

    /// <summary>
    /// Use this in order to show on the inspector. Only used on Editor
    /// </summary>
    /// <returns></returns>
    GameObject GetGameObject();
}