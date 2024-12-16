using UnityEngine;

public class LanguageUI : MonoBehaviour
{
    [SerializeField] private StringGameEvent OnChangeLocaleEvent;

    public void ChangeLocale(string locale)
    {
        OnChangeLocaleEvent.RaiseEvent(locale);
    }

}
