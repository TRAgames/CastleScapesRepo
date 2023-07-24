using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Progress : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    public static Progress Instance;

    public User User;

    public bool IsAdsShowing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if !UNITY_EDITOR && UNITY_WEBGL
            LoadExtern();
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && Input.GetKeyDown(KeyCode.Home))
        {
            DeleteAllData();
        }

        if (Input.GetKeyDown(KeyCode.Insert) && Input.GetKeyDown(KeyCode.End))
        {
            PlayerPrefs.SetInt("LockLevel", 59);
        }
    }

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(User.GetCurrentUser());
        SaveExtern(jsonString);
    }

    public void Load(string value)
    {
        User = JsonUtility.FromJson<User>(value);
        User.SetCurrentUser(User);
        if (PlayerPrefs.GetInt("StartGame") == 0)
        {
            PlayerPrefs.SetInt("StartGame", 1);
            PlayerPrefs.SetInt("Coin", 200);
            PlayerPrefs.SetInt("Life", 3);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Vir", 1);
            PlayerPrefs.SetInt("LockLevel", 1);
            Save();
        }
        HomeManager.Instance.UpdateCoinText();
        HomeManager.Instance.UpdateLifeText();
        HomeManager.Instance.UpdateSetting();
    }

    private void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
#if !UNITY_EDITOR && UNITY_WEBGL
        User = new User();
        Save();
#endif
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        if (!IsAdsShowing)
            AudioListener.pause = silence;
    }

    public void PauseMusic()
    {
        IsAdsShowing = true;
        AudioListener.pause = true;
    }

    public void UnpauseMusic()
    {
        IsAdsShowing = false;
        AudioListener.pause = false;
    }

}
