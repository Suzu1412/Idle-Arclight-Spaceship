public interface ISaveable
{
    void SaveData(GameDataSO gameData);
    void LoadDataAsync(GameDataSO gameData);
}