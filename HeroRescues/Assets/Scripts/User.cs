using System;
using UnityEngine;

[Serializable]
public class User
{
    public int CurrentLevel;
    public int Life;
    public int Coin;
    public int LockLevel;
    public int StartGame;

    public static User GetCurrentUser()
    {
        User currentUser = new User
        {
            StartGame = PlayerPrefs.GetInt("StartGame"),
            CurrentLevel = PlayerPrefs.GetInt("CurrentLevel"),
            Life = PlayerPrefs.GetInt("Life"),
            Coin = PlayerPrefs.GetInt("Coin"),
            LockLevel = PlayerPrefs.GetInt("LockLevel")
        };
        return currentUser;
    }

    public static void SetCurrentUser(User currentUser)
    {
        PlayerPrefs.SetInt("StartGame", currentUser.StartGame);
        PlayerPrefs.SetInt("CurrentLevel", currentUser.CurrentLevel);
        PlayerPrefs.SetInt("Life", currentUser.Life);
        PlayerPrefs.SetInt("Coin", currentUser.Coin);
        PlayerPrefs.SetInt("LockLevel", currentUser.LockLevel);
    }

}
