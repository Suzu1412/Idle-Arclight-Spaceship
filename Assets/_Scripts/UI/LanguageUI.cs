using Cysharp.Text;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageUI : MonoBehaviour
{
    [SerializeField] private StringGameEvent OnChangeLocaleEvent;
    [SerializeField] private TextMeshProUGUI _languageText;
    [SerializeField] private List<string> _languageList = new();
    private int _currentIndex = 0;

    private void OnEnable()
    {
        UpdateCurrentIndex(LocalizationSettings.SelectedLocale.Identifier.Code);
    }
    public void PreviousSelection()
    {
        _currentIndex = _languageList.MoveBackward(_currentIndex);

        UpdateText();
        ChangeLocale();

    }

    public void NextSelection()
    {
        _currentIndex = _languageList.MoveForward(_currentIndex);

        UpdateText();
        ChangeLocale();
    }

    private void UpdateText()
    {
        string language = "";

        switch (_languageList[_currentIndex])
        {
            case "en":
                language = "English";
                break;

            case "es":
                language = "Español";
                break;
        }

        _languageText.SetTextFormat("{0}", language);
    }

    private void ChangeLocale()
    {
        OnChangeLocaleEvent.RaiseEvent(_languageList[_currentIndex], this);
    }

    private void UpdateCurrentIndex(string locale)
    {
        for (int i = 0; i < _languageList.Count; i++)
        {
            if (_languageList[i] == locale)
            {
                _currentIndex = i;
                break;
            }
        }

        UpdateText();
        ChangeLocale();
    }

}
