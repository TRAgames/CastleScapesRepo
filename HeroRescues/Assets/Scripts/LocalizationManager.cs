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
                localizedText.Add("level", "Уровень ");
                localizedText.Add("play", "Играть");
                localizedText.Add("press_to_remove", "НАЖМИ, ЧТОБЫ УБРАТЬ");
                localizedText.Add("save_labubu", "СПАСИ ЛАБУБУ");
                localizedText.Add("lava_water_stone", "ЛАВА + ВОДА = КАМНИ");
                localizedText.Add("tap_to_cut", "НАЖМИ, ЧТОБЫ ОБОРВАТЬ");
                localizedText.Add("loading", "ЗАГРУЗКА...");
                localizedText.Add("prev", "Предыдущая");
                localizedText.Add("next", "Следующая");
                localizedText.Add("page", "Страница ");
                localizedText.Add("buy", "Купить");
                localizedText.Add("add_life", "Добавить жизни");
                localizedText.Add("cost", "Стоимость:");
                localizedText.Add("add_coins", "Добавить монет");
                localizedText.Add("get", "Получить:");
                localizedText.Add("ads", "Посмотреть рекламу");
                localizedText.Add("menu", "Меню");
                localizedText.Add("again", "Снова");
                localizedText.Add("next_gameplay", "Далее");
                localizedText.Add("win", "ПОБЕДА");
                localizedText.Add("x2_coins", "x2 монет");
                localizedText.Add("fail", "ПОРАЖЕНИЕ");
                localizedText.Add("skip", "Пропустить");


                break;
            case "es":
                localizedText.Add("level", "Nivel");
                localizedText.Add("play", "Jugar");
                localizedText.Add("press_to_remove", "HAGA CLIC PARA ELIMINAR");
                localizedText.Add("save_labubu", "SALVA A LABUBU");
                localizedText.Add("lava_water_stone", "LAVA + AGUA = PIEDRAS");
                localizedText.Add("tap_to_cut", "HAGA CLIC PARA CORTAR");
                localizedText.Add("loading", "CARGA...");
                localizedText.Add("prev", "Anterior");
                localizedText.Add("next", "Siguiente");
                localizedText.Add("page", "Página ");
                localizedText.Add("buy", "Comprar");
                localizedText.Add("add_life", "Añadir vida");
                localizedText.Add("cost", "Costo:");
                localizedText.Add("add_coins", "Añadir monedas");
                localizedText.Add("get", "Obtener:");
                localizedText.Add("ads", "Ver anuncios");
                localizedText.Add("menu", "Menú");
                localizedText.Add("again", "Otra vez");
                localizedText.Add("next_gameplay", "Continuación");
                localizedText.Add("win", "VICTORIA");
                localizedText.Add("x2_coins", "X2 monedas");
                localizedText.Add("fail", "DERROTA");
                localizedText.Add("skip", "Omitir");

                break;
            case "de":
                localizedText.Add("level", "Level");
                localizedText.Add("play", "Spielen");
                localizedText.Add("press_to_remove", "KLICKE, UM ES ZU ENTFERNEN");
                localizedText.Add("save_labubu", "RETTE LABUBU");
                localizedText.Add("lava_water_stone", "LAVA + WASSER = STEINE");
                localizedText.Add("tap_to_cut", "KLICKE, UM ZU ZERREIßEN");
                localizedText.Add("loading", "LADEN...");
                localizedText.Add("prev", "Vorherige");
                localizedText.Add("next", "Nächste");
                localizedText.Add("page", "Seite ");
                localizedText.Add("buy", "Kaufen");
                localizedText.Add("add_life", "Leben hinzufügen");
                localizedText.Add("cost", "Wert:");
                localizedText.Add("add_coins", "Fügen Sie Münzen hinzu");
                localizedText.Add("get", "Bekommen:");
                localizedText.Add("ads", "Anzeigen ansehen");
                localizedText.Add("menu", "Menü");
                localizedText.Add("again", "Wieder");
                localizedText.Add("next_gameplay", "Weiter");
                localizedText.Add("win", "Gewinn");
                localizedText.Add("x2_coins", "x2 Münzen");
                localizedText.Add("fail", "Niederlage");
                localizedText.Add("skip", "Verpassen");

                break;
            case "en":
                localizedText.Add("level", "Level");
                localizedText.Add("play", "Play");
                localizedText.Add("press_to_remove", "TAP TO REMOVE");
                localizedText.Add("save_labubu", "SAVE LABUBU");
                localizedText.Add("lava_water_stone", "LAVA + WATER = STONES");
                localizedText.Add("tap_to_cut", "TAP TO CUT");
                localizedText.Add("loading", "LOADING...");
                localizedText.Add("prev", "Previous");
                localizedText.Add("next", "Next");
                localizedText.Add("page", "Page ");
                localizedText.Add("buy", "Buy");
                localizedText.Add("add_life", "Add life");
                localizedText.Add("cost", "Cost:");
                localizedText.Add("add_coins", "Add coins");
                localizedText.Add("get", "Get:");
                localizedText.Add("ads", "Watch ads");
                localizedText.Add("menu", "Menu");
                localizedText.Add("again", "Again");
                localizedText.Add("next_gameplay", "Next");
                localizedText.Add("win", "VICTORY");
                localizedText.Add("x2_coins", "x2 coins");
                localizedText.Add("fail", "FAIL");
                localizedText.Add("skip", "Skip");

                break;
            case "tr":
                localizedText.Add("level", "Seviye");
                localizedText.Add("play", "Oynamak");
                localizedText.Add("press_to_remove", "TEMİZLEMEK İÇİN TIKLA");
                localizedText.Add("save_labubu", "LABUBU'YI KURTAR");
                localizedText.Add("lava_water_stone", "LAV + SU = TAŞLAR");
                localizedText.Add("tap_to_cut", "KESMEK İÇİN TIKLA");
                localizedText.Add("loading", "YÜKLENİYOR...");
                localizedText.Add("prev", "Önceki");
                localizedText.Add("next", "Gelecek");
                localizedText.Add("page", "Sayfa ");
                localizedText.Add("buy", "Almak");
                localizedText.Add("add_life", "Hayat ekle");
                localizedText.Add("cost", "Maliyet:");
                localizedText.Add("add_coins", "Madeni para ekle");
                localizedText.Add("get", "Almak:");
                localizedText.Add("ads", "Reklamı gör");
                localizedText.Add("menu", "Menü");
                localizedText.Add("again", "Tekrar");
                localizedText.Add("next_gameplay", "Sonra");
                localizedText.Add("win", "ZAFER");
                localizedText.Add("x2_coins", "x2 madeni para");
                localizedText.Add("fail", "YENİLGİ");
                localizedText.Add("skip", "Atla");


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