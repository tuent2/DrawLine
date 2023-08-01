using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Settings;
public class Locales : MonoBehaviour
{
    private bool active = false;
    public void ChangeLocale(int locationId)
    {
        if (active == true) 
            return;
        StartCoroutine(SetLocale(locationId));
    }

    IEnumerator SetLocale (int _localeId)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
        PlayerPrefs.SetInt("LocaleKey", _localeId);
        active = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        int Id = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(Id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
