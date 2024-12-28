using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour, ISaveable
{
    [SerializeField] private SaveableRunTimeSetSO _saveable;

    private Locale _currentLocale;
    [SerializeField] private StringGameEventListener _changeLocaleEventListener;

    private void OnEnable()
    {
        _saveable.Add(this);
        _changeLocaleEventListener.Register(ChangeLocale);
    }

    private void OnDisable()
    {
        _saveable.Remove(this);
        _changeLocaleEventListener.DeRegister(ChangeLocale);
    }

    public void LoadData(GameDataSO gameData)
    {
        if (gameData.LocalizationData.Locale == null) return;
        ChangeLocale(gameData.LocalizationData.Locale);
    }

    public void SaveData(GameDataSO gameData)
    {
        _currentLocale = LocalizationSettings.SelectedLocale;
        gameData.SaveLocalization(_currentLocale.LocaleName);
    }

    private void ChangeLocale(string localeName)
    {
        switch (localeName)
        {
            case "en":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                break;
            case "es":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                break;
            default:
                break;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
