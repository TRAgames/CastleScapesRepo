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

    public bool IsAdsShowing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if !UNITY_EDITOR && UNITY_WEBGL
            //LoadExtern();
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
    }

    public void Save()
    {
        //string jsonString = JsonUtility.ToJson(User.GetCurrentUser());
        //SaveExtern(jsonString);
    }

    public void Load(string value)
    {
        //User = JsonUtility.FromJson<User>(value);
        //User.SetCurrentUser(User);
    }

    private void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
#if !UNITY_EDITOR && UNITY_WEBGL
        //User = new User();
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
