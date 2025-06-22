using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void SetLeaderboardExtern(int value);

    public static GameManager instance;

    public bool isGameOver, isGameWin = false;

    public int currentCoin,bonusCoin, _life, _totalGoblin,_totalGoblinKilled; 

    private void Awake()
    {
        instance = this;
        isGameOver = false;
        isGameWin = false;
        bonusCoin = 0;
        ResetCollision();
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentCoin = PlayerPrefs.GetInt("Coin");
        _life = PlayerPrefs.GetInt("Life");
        _totalGoblinKilled = 0;
        if(GameObject.FindGameObjectWithTag("Goblin") != null)
        _totalGoblin = GameObject.FindGameObjectsWithTag("Goblin").Length;
        //Debug.Log(currentCoin);
        UIManager._instance.ShowCoinText(UIManager._instance.coinGamePlayText, currentCoin);
        UIManager._instance.UpdateLife(_life);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin(int add)
    {
        currentCoin += add;
        UIManager._instance.ShowCoinText(UIManager._instance.coinGamePlayText, currentCoin);
    }    

    public void GameOver()
    {
        if (isGameOver)
            return;
        isGameOver = true;
        Camera.main.cullingMask = Progress.Instance.DesireMask;
        UIManager._instance.ShowGameOver();
    }

    public void GameWin()
    {
        if (isGameWin)
            return;

#if !UNITY_EDITOR && UNITY_WEBGL
            SetLeaderboardExtern(PlayerPrefs.GetInt("CurrentLevel"));
#endif

        LevelFinishedSend(PlayerPrefs.GetInt("CurrentLevel").ToString());
        isGameWin = true;
        Camera.main.cullingMask = Progress.Instance.DesireMask;
        UIManager._instance.ShowGameClear();
    }

    public void LevelFinishedSend(string name)
    {
        var eventParams = new Dictionary<string, string>
            {
                { "level_finished", name }
            };
        YandexMetrica.Send("level_finished", eventParams);
    }

    public void ResetPara()
    {
        isGameWin = false;
        isGameOver = false;
    }

    public void ResetCollision()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Goblin"),false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"),false);
    }    
}
