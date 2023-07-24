using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SkipLevel();
    [DllImport("__Internal")]
    private static extern void DoubleCoins();

    [DllImport("__Internal")]
    private static extern void GetCoins();

    [DllImport("__Internal")]
    private static extern void FinishLevel();

    public GameObject gameUIPanel, loadingPanel, resultPanel,failResultPanel,replayBtn,skipBtn,doubleCoinBtn, moreCoinPanel, moreLifePanel;

    public SpriteRenderer loadingMask;

    public static UIManager _instance;

    public TextMeshProUGUI levelTextInGame,levelTextInResult,coinBonusText, levelTextInGameOver;

    public TextMeshProUGUI coinGamePlayText, lifeGamePlayText, coinGameOverText, lifeGameOverText, coinGameWinText, lifeGameWinText;

    public Image[] iconLst = new Image[5];

    public Image[] doneLst = new Image[5];

    public Sprite[] spriteLst = new Sprite[4];

    public Image[] iconResultLst = new Image[5];

    public Image[] doneResultLst = new Image[5];

    public Image[] iconGameOverLst = new Image[5];

    public Image[] doneGameOverLst = new Image[5];

    public GameObject frontLife1, frontLife2, frontLife3, frontLifeInGame1,frontLifeInGame2,frontLifeInGame3;

    private void Awake()
    {
        _instance = this;
        StartCoroutine(Fading());
       
    }

    // Start is called before the first frame update
    void Start()
    {
        SetIconInLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideLevelPanel()
    {
       // levelSelector.GetComponent<Animator>().SetTrigger("Close");
    }

    public void LoadLevel(int _level)
    {
        PlayerPrefs.SetInt("CurrentLevel", _level);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        HideLevelPanel();
       // homePanel.SetActive(false);
        StartCoroutine(Fading());

    }

    IEnumerator Fading()
    {
     
        loadingPanel.SetActive(true);
        levelTextInGame.text = "Уровень " + PlayerPrefs.GetInt("CurrentLevel");
        yield return new WaitForSeconds(0.2f);
      /*  for (float i = 1; i >= 0; i -= Time.deltaTime * 0.99f)
        {
            // set color with i as alpha
            loadingMask.color = new Color(0.7058824f, 0.8392158f, 0.8745099f, i);
            yield return null;
        } */
        loadingPanel.SetActive(false);
    }

    IEnumerator FadingReplay()
    {
        //yield return new WaitForSeconds(5.0f);
        loadingPanel.SetActive(true);
        LevelManager._instance.LoadLevel();
        gameUIPanel.SetActive(true);
        levelTextInGame.text = "Уровень " + PlayerPrefs.GetInt("CurrentLevel");
        //yield return new WaitForSeconds(0.2f);
        for (float i = 1; i >= 0; i -= Time.deltaTime * 0.35f)
        {
            // set color with i as alpha
            loadingMask.color = new Color(0, 0, 0, i);
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    IEnumerator FadeImage(bool fadeAway, Image img)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    public void BackHomeFromGamePlay()
    {
        /*
        LevelManager._instance.CleanLevel();
        gameUIPanel.SetActive(false);
        homePanel.SetActive(true);
        resultPanel.SetActive(false);
        failResultPanel.SetActive(false);
        GameManager.instance.ResetPara();
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.orthographicSize = 5.0f;
        */
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        Application.LoadLevel("MainHome");
    }    

    public void ReplayGame()
    {
        /*
        LevelManager._instance.CleanLevel();
        resultPanel.SetActive(false);
        failResultPanel.SetActive(false);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.orthographicSize = 5.0f;
        GameManager.instance.ResetPara();
        StartCoroutine(FadingReplay());
        */
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        ShowLevelPanel();
    }

    public void ShowLevelPanel()
    {
        if (PlayerPrefs.GetInt("Life") > 0)
            Application.LoadLevel("MainGame");
        else
            ShowOutOfLife();
    }

    public void ShowOutOfLife()
    {
        moreLifePanel.SetActive(true);
        int _life = PlayerPrefs.GetInt("Life");
        UpdateLife(_life);
    }

    public void HideOutOfLife()
    {
        moreLifePanel.GetComponent<Animator>().SetTrigger("Close");
    }

    public void ShowGameClear()
    {
        StartCoroutine(ShowGameClearIE());
    }

    IEnumerator ShowGameClearIE()
    {

        if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel - 1] == LevelManager.LEVEL_TYPE.GOBLIN)
            GameManager.instance.bonusCoin += 30;
        else if(LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel - 1] == LevelManager.LEVEL_TYPE.BIG_COIN)
            GameManager.instance.bonusCoin += 60;
        else if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel - 1] == LevelManager.LEVEL_TYPE.PRINCESS)
            GameManager.instance.bonusCoin += 80;
        else if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel - 1] == LevelManager.LEVEL_TYPE.COIN)
            GameManager.instance.bonusCoin += 60;

        coinBonusText.text = "+ " + GameManager.instance.bonusCoin;
        PlayerPrefs.SetInt("Coin", GameManager.instance.currentCoin + GameManager.instance.bonusCoin);
        PlayerPrefs.SetInt("Life", GameManager.instance._life);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        yield return new WaitForSeconds(2.0f);

        ShowCoinText(coinGameWinText, GameManager.instance.currentCoin);
        UpdateLife(GameManager.instance._life);
        ShowLifeText(lifeGameWinText, GameManager.instance._life);

        SoundManager.Instance.Play(SoundManager.Instance._levelPass);
        resultPanel.SetActive(true);
        //GameObject _hero = GameObject.FindGameObjectWithTag("Hero");
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int lockLevel = PlayerPrefs.GetInt("LockLevel");
        if(currentLevel == lockLevel)
        {
            lockLevel++;
            PlayerPrefs.SetInt("LockLevel", lockLevel);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        }
        SetIconResultInLevel();
        doneResultLst[(currentLevel -1) % 5].gameObject.SetActive(true);
        levelTextInResult.text = "Уровень " + currentLevel.ToString();
        GameObject _hero = GameObject.FindGameObjectWithTag("Hero");
        Camera.main.transform.localPosition = new Vector3(_hero.transform.localPosition.x,
        _hero.transform.localPosition.y, Camera.main.transform.position.z);
        Camera.main.orthographicSize = 3.0f;
#if !UNITY_EDITOR && UNITY_WEBGL
        if (currentLevel >= 5) FinishLevel();           
#endif
        if (PlayerPrefs.GetInt("Bonus" + PlayerPrefs.GetInt("CurrentLevel")) == 1)
        {
            doubleCoinBtn.SetActive(false);
        }
    }

    public void OnSkipLevel()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        SkipLevel();
#endif
#if UNITY_EDITOR
        SkipLevelRewarded();
#endif
    }

    public void SkipLevelRewarded()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int lockLevel = PlayerPrefs.GetInt("LockLevel");
        
        if (currentLevel == 60)
            PlayerPrefs.SetInt("CurrentLevel", 1);
        else
        {
            if (currentLevel == lockLevel)
            {
                lockLevel++;
                PlayerPrefs.SetInt("LockLevel", lockLevel);
            }
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        }       
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        Application.LoadLevel("MainGame");
    }


    public void OnDoubleCoin()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        DoubleCoins();
#endif
#if UNITY_EDITOR
        DoubleCoinsRewarded();
#endif
    }

    public void DoubleCoinsRewarded()
    {

        GameManager.instance.bonusCoin = GameManager.instance.bonusCoin * 2;
        coinBonusText.text = "+ " + GameManager.instance.bonusCoin;
        PlayerPrefs.SetInt("Bonus" + PlayerPrefs.GetInt("CurrentLevel"), 1);
        doubleCoinBtn.SetActive(false);

        PlayerPrefs.SetInt("Coin", GameManager.instance.currentCoin + GameManager.instance.bonusCoin);

        ShowCoinText(coinGameWinText, GameManager.instance.currentCoin + GameManager.instance.bonusCoin);
        ShowCoinText(coinGamePlayText, GameManager.instance.currentCoin + GameManager.instance.bonusCoin);

#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif

    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (currentLevel == 60)
            PlayerPrefs.SetInt("CurrentLevel", 1);
        else
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        Application.LoadLevel("MainGame");
    }


    public void ShowGameOver()
    {
        StartCoroutine(ShowGameOverIE());
    }

    IEnumerator ShowGameOverIE()
    {
      //  gameUIPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        ShowCoinText(coinGameOverText, GameManager.instance.currentCoin);
        UpdateLife(GameManager.instance._life);
        ShowLifeText(lifeGameOverText, GameManager.instance._life);

        if (GameManager.instance._life == 0)
        {
            //replayBtn.SetActive(false);
            skipBtn.SetActive(false);
        }    

        PlayerPrefs.SetInt("Coin", GameManager.instance.currentCoin);
        PlayerPrefs.SetInt("Life", GameManager.instance._life);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        SoundManager.Instance.Play(SoundManager.Instance._levelFail);
        failResultPanel.SetActive(true);
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        SetIconGameOverInLevel();
        doneResultLst[(currentLevel - 1) % 5].gameObject.SetActive(true);

        levelTextInGameOver.text = "Уровень " + currentLevel.ToString();
        GameObject _hero = GameObject.FindGameObjectWithTag("Hero");
        Camera.main.transform.localPosition = new Vector3(_hero.transform.localPosition.x,
        _hero.transform.localPosition.y, Camera.main.transform.position.z);
        Camera.main.orthographicSize = 3.0f;
#if !UNITY_EDITOR && UNITY_WEBGL
        if (currentLevel >= 5) FinishLevel();           
#endif
    }

    public void BuyMoreLife()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        if (GameManager.instance.currentCoin >= 500 & GameManager.instance._life < 3)
        {
            GameManager.instance.currentCoin -= 500;
            GameManager.instance._life++;
            PlayerPrefs.SetInt("Coin", GameManager.instance.currentCoin);
            PlayerPrefs.SetInt("Life", GameManager.instance._life);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
            ShowCoinText(coinGameOverText, GameManager.instance.currentCoin);
            ShowLifeText(lifeGameOverText, GameManager.instance._life);
            UpdateLife(GameManager.instance._life);
            //replayBtn.SetActive(true);
            skipBtn.SetActive(true);
        }
        else
        {
            ShowMoreCoin();
        }
    }


    void SetIconFromLevelType(LevelManager.LEVEL_TYPE _type, Image _coin)
    {
        switch(_type)
        {
            case LevelManager.LEVEL_TYPE.COIN:
                _coin.sprite = spriteLst[0];
                break;
            case LevelManager.LEVEL_TYPE.BIG_COIN:
                _coin.sprite = spriteLst[1];
                break;
            case LevelManager.LEVEL_TYPE.GOBLIN:
                _coin.sprite = spriteLst[2];
                break;
            case LevelManager.LEVEL_TYPE.PRINCESS:
                _coin.sprite = spriteLst[3];
                break;
        }
    }

    public void ShowMoreCoin()
    {
        moreCoinPanel.SetActive(true);
    }

    public void OnMoreCoin()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        GetCoins();
#endif
#if UNITY_EDITOR
        GetCoinsRewarded();
#endif
    }

    public void HideMoreCoin()
    {
        moreCoinPanel.GetComponent<Animator>().SetTrigger("Close");
    }

    public void GetCoinsRewarded()
    {
        int _coin = PlayerPrefs.GetInt("Coin");
        _coin += 50;
        PlayerPrefs.SetInt("Coin", _coin);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        GameManager.instance.currentCoin = _coin;
        ShowCoinText(coinGameOverText, _coin);
    }

    void SetIconInLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int _index = Mathf.CeilToInt(currentLevel / 5);
        if (currentLevel % 5 == 0)
            _index = currentLevel / 5 - 1;
       // Debug.Log("INDEX " + _index);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5], iconLst[0]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 1], iconLst[1]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 2], iconLst[2]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 3] , iconLst[3]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 4], iconLst[4]);

        int _offset = (currentLevel -1 ) % 5;

        for(int i = 0; i < 5; i++)
        {
            if (i < _offset)
                doneLst[i].gameObject.SetActive(true);
            else if(i == _offset)
            {
                doneLst[i].gameObject.SetActive(false);
                iconLst[i].color = new Color(1, 1, 1, 1);
            }   
            else
            {
                doneLst[i].gameObject.SetActive(false);
                iconLst[i].color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
            }    
        }    
    }

    public void ShowCoinText(TextMeshProUGUI _coinText, int coin)
    {
        _coinText.text = coin.ToString();
    }

    public void ShowLifeText(TextMeshProUGUI _lifeText, int life)
    {
        _lifeText.text = life.ToString();
    }


    public void UpdateLife(int life)
    {
        lifeGamePlayText.text = life.ToString();
        lifeGameOverText.text = life.ToString();
        lifeGameWinText.text = life.ToString();
        switch (life)
        {
            case 0:
               
                frontLife1.SetActive(false);
                frontLife2.SetActive(false);
                frontLife3.SetActive(false);

                frontLifeInGame1.SetActive(false);
                frontLifeInGame2.SetActive(false);
                frontLifeInGame3.SetActive(false);

                break;
            case 1:


                frontLife1.SetActive(true);
                frontLife2.SetActive(false);
                frontLife3.SetActive(false);

                frontLifeInGame1.SetActive(true);
                frontLifeInGame2.SetActive(false);
                frontLifeInGame3.SetActive(false);

                break;
            case 2:


                frontLife1.SetActive(true);
                frontLife2.SetActive(true);
                frontLife3.SetActive(false);

                frontLifeInGame1.SetActive(true);
                frontLifeInGame2.SetActive(true);
                frontLifeInGame3.SetActive(false);

                break;
            case 3:


                frontLife1.SetActive(true);
                frontLife2.SetActive(true);
                frontLife3.SetActive(true);

                frontLifeInGame1.SetActive(true);
                frontLifeInGame2.SetActive(true);
                frontLifeInGame3.SetActive(true);

                break;
        }    
    }

    void SetIconResultInLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int _index = Mathf.CeilToInt(currentLevel / 5);
        if (currentLevel % 5 == 0)
            _index = currentLevel / 5 - 1;
        Debug.Log("INDEX " + _index);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5], iconResultLst[0]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 1], iconResultLst[1]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 2], iconResultLst[2]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 3], iconResultLst[3]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 4], iconResultLst[4]);

        int _offset = (currentLevel - 1) % 5;

        for (int i = 0; i < 5; i++)
        {
            if (i < _offset)
                doneResultLst[i].gameObject.SetActive(true);
            else if (i == _offset)
            {
                doneResultLst[i].gameObject.SetActive(false);
                iconResultLst[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                doneResultLst[i].gameObject.SetActive(false);
                iconResultLst[i].color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
            }
        }


    }

    void SetIconGameOverInLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int _index = Mathf.CeilToInt(currentLevel / 5);
        if (currentLevel % 5 == 0)
            _index = currentLevel / 5 - 1;
        Debug.Log("INDEX " + _index);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5], iconGameOverLst[0]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 1], iconGameOverLst[1]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 2], iconGameOverLst[2]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 3], iconGameOverLst[3]);
        SetIconFromLevelType(LevelManager._instance.levelTypeLst[_index * 5 + 4], iconGameOverLst[4]);

        int _offset = (currentLevel - 1) % 5;

        for (int i = 0; i < 5; i++)
        {
            if (i < _offset)
                doneGameOverLst[i].gameObject.SetActive(true);
            else if (i == _offset)
            {
                doneGameOverLst[i].gameObject.SetActive(false);
                iconGameOverLst[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                doneGameOverLst[i].gameObject.SetActive(false);
                iconGameOverLst[i].color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
            }
        }


    }

}
