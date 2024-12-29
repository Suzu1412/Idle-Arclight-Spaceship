using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    private Locale _currentLocale;
    [SerializeField] private StringGameEventListener _changeLocaleEventListener;

    private void OnEnable()
    {
        _changeLocaleEventListener.Register(ChangeLocale);
    }

    private void OnDisable()
    {
        _changeLocaleEventListener.DeRegister(ChangeLocale);
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
}
