using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
	public event Action OnChangeLanguageEvent;

	private Dictionary<string, string> localizedText;
	private bool isReady = false;
	private string missingTextString = "Localized text not found";

	public static LocalizationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    public void SetLanguage(string fileName)
	{       
        LoadLocalizedText(fileName);

        //Progress.Instance.Language = fileName;

        OnChangeLanguageEvent?.Invoke();
    }

	public string GetLocalizedValue(string key)
	{
		string result = missingTextString;
		if (localizedText.ContainsKey(key))
		{
			result = localizedText[key];
		}

		return result;
	}

	private void LoadLocalizedText(string fileName)
	{
		localizedText = new Dictionary<string, string>();
		localizedText.Clear();
		switch (fileName)
		{
			case "ru":
                localizedText.Add("level", "Уровень");
                localizedText.Add("play", "Играть");


                break;
            case "es":
                localizedText.Add("level", "Nivel");
                localizedText.Add("play", "Играть");

                break;
            case "de":
                localizedText.Add("level", "Level");
                localizedText.Add("play", "Играть");

                break;
            case "en":
                localizedText.Add("level", "Level");
                localizedText.Add("play", "Play");


                break;
            case "tr":
                localizedText.Add("level", "Seviye");
                localizedText.Add("play", "Играть");


                break;
        }		
	}

	public bool GetIsReady()
	{
		return isReady;
	}

}

[System.Serializable]
public class LocalizationData
{
	public LocalizationItem[] items;

}

[System.Serializable]
public class LocalizationItem
{
	public string key;
	public string value;
}