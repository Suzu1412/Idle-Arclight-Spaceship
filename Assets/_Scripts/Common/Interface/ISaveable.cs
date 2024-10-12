public interface ISaveable
{
    SerializableGuid Id { get; set; }
    void SaveData(GameData gameData);
    void LoadData(GameData gameData);

}