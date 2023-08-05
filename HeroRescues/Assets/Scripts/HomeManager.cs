using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetCoins();

    public GameObject levelSelector, homePanel, loadingPanel,settingPanel,outLifePanel, moreCoinPanel, frontLife1,frontLife2,frontLife3;

    public Image loadingMask, musicImg,soundImg,virImg;

    public Sprite onMusicBtn, offMusicBtn, onSoundBtn, offSoundBtn;

    private static HomeManager _instance;

    public TextMeshProUGUI coinText, lifeText, lockLevelText;

    public static HomeManager Instance { get => _instance; set => _instance = value; }

    private void Awake()
    {
        _instance = this;
#if UNITY_EDITOR
        if (PlayerPrefs.GetInt("StartGame") == 0)
        {
            PlayerPrefs.SetInt("StartGame", 1);
            PlayerPrefs.SetInt("Coin", 200);
            PlayerPrefs.SetInt("Life", 3);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Vir", 1);
            PlayerPrefs.SetInt("LockLevel", 1);
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        //lockLevelText.text = "LEVEL " + PlayerPrefs.GetInt("LockLevel");
        UpdateCoinText();
        UpdateLifeText();
        UpdateSetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowLevelPanel()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        if (PlayerPrefs.GetInt("Life") > 0)
            levelSelector.SetActive(true);
        else
            ShowOutOfLife();
    }

    public void ShowSettingPanel()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        settingPanel.SetActive(true);
        homePanel.SetActive(false);
    }

    public void CloseSettingPanel()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        //settingPanel.GetComponent<Animator>().SetTrigger("Close");
        homePanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void ShowOutOfLife()
    {
        outLifePanel.SetActive(true);
        int _life = PlayerPrefs.GetInt("Life");
        UpdateLive(_life);
    }   
    
    public void HideOutOfLife()
    {
        outLifePanel.GetComponent<Animator>().SetTrigger("Close");
    }


    public void ShowMoreCoin()
    {
        moreCoinPanel.SetActive(true);
    }

    public void HideMoreCoin()
    {
        moreCoinPanel.GetComponent<Animator>().SetTrigger("Close");
    }

    public void HideLevelPanel()
    {
        levelSelector.GetComponent<Animator>().SetTrigger("Close");
    }

    public void LoadLevel(int _level)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }
        PlayerPrefs.SetInt("CurrentLevel", _level);
        HideLevelPanel();
        homePanel.SetActive(false);
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        yield return new WaitForSeconds(0.5f);
        loadingPanel.SetActive(true);
        Application.LoadLevel("MainGame");

        //yield return new WaitForSeconds(0.2f);
        for (float i = 1; i >= 0; i -= Time.deltaTime * 0.55f)
        {
            // set color with i as alpha
            loadingMask.color = new Color(0.7058824f, 0.8392158f, 0.8745099f, i);
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    IEnumerator FadingReplay()
    {
        //yield return new WaitForSeconds(5.0f);
        loadingPanel.SetActive(true);
        Application.LoadLevel("MainGame");

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

    public void UpdateCoinText()
    {
        coinText.text = PlayerPrefs.GetInt("Coin") + "";
    }

    public void UpdateLifeText()
    {
        lifeText.text = PlayerPrefs.GetInt("Life") + "";
    }

    public void MoreLive()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        int _coin = PlayerPrefs.GetInt("Coin");
        int _life = PlayerPrefs.GetInt("Life");
        if (_coin >= 500 && _life < 3)
        {
            _coin -= 500;
            PlayerPrefs.SetInt("Coin", _coin);
            UpdateCoinText();
            _life++;
            PlayerPrefs.SetInt("Life", _life);
            UpdateLive(_life);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        }
        else if (_coin < 500)
            ShowMoreCoin();
    }

    public void MoreCoin()
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

    public void GetCoinsRewarded()
    {
        int _coin = PlayerPrefs.GetInt("Coin");
        _coin += 100;
        PlayerPrefs.SetInt("Coin", _coin);
#if !UNITY_EDITOR && UNITY_WEBGL
        Progress.Instance.Save();
#endif
        UpdateCoinText();
    }

    private IEnumerator LockButtonRoutine(Button button)
    {
        button.interactable = false;
        yield return new WaitForSeconds(0.5f);
        button.interactable = true;
    }

    public void LockButton(Button button)
    {
        StartCoroutine(LockButtonRoutine(button));
    }

    public void UpdateLive(int life)
    {
        UpdateLifeText();
        switch(life)
        {

        case 0: 
        frontLife1.SetActive(false);
        frontLife2.SetActive(false);
        frontLife3.SetActive(false);

        break;

         case 1:
        frontLife1.SetActive(true);
        frontLife2.SetActive(false);
        frontLife3.SetActive(false);



        break;
        case 2:
         frontLife1.SetActive(true);
        frontLife2.SetActive(true);
        frontLife3.SetActive(false);



        break;
        case 3:
        frontLife1.SetActive(true);
        frontLife2.SetActive(true);
        frontLife3.SetActive(true);



        break;
    }
            
            
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        int _music = PlayerPrefs.GetInt("Music");

        if(_music == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        UpdateSetting();
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        int _sound = PlayerPrefs.GetInt("Sound");

        if(_sound == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        UpdateSetting();
    }

    public void ToggleVir()
    {
        int _vir = PlayerPrefs.GetInt("Vir");

        if(_vir == 1)
        {
            PlayerPrefs.SetInt("Vir", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Vir", 1);
        }
        UpdateSetting();
    }

    public void UpdateSetting()
    {
        int _music = PlayerPrefs.GetInt("Music");

        int _sound = PlayerPrefs.GetInt("Sound");

        int _vir = PlayerPrefs.GetInt("Vir");


        if (_music == 1)
        {
            SoundManager.Instance.MusicSource.mute = false;
            musicImg.sprite = onMusicBtn;
        }
        else
        {
            SoundManager.Instance.MusicSource.mute = true;
            musicImg.sprite = offMusicBtn;
        }

        if(_sound == 1)
        {
            SoundManager.Instance.EffectsSource.mute = false;
            soundImg.sprite = onSoundBtn;
        }
        else
        {
            SoundManager.Instance.EffectsSource.mute = true;
            soundImg.sprite = offSoundBtn;
        }
/*
        if (_vir == 1)
        {
            //SoundManager.Instance.EffectsSource.mute = false;
            virImg.sprite = onMusicBtn;
        }
        else
        {
           // SoundManager.Instance.EffectsSource.mute = true;
            virImg.sprite = offMusicBtn;
        } */
    }

}
