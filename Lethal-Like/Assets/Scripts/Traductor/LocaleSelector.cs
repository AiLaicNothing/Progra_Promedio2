using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        StartCoroutine(Init());

    }

    IEnumerator Init()
    {
        yield return LocalizationSettings.InitializationOperation;
        dropdown.ClearOptions();
        dropdown.AddOptions(LocalizationStringOptions());
        SelectLocale();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private List<string> LocalizationStringOptions()
    {
        List<string> options = new List<string>();
        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (Locale locale in locales)
        {
            options.Add(locale.LocaleName);
        }
        return options;
    }

    private void SelectLocale()
    {
        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        int index = locales.IndexOf(currentLocale);
        dropdown.SetValueWithoutNotify(index);
    }

    private void OnValueChanged(int index)
    {
        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.SelectedLocale = locales[index];
    }


}
