using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager _instance;

    public GameObject currentLevelObj;

    public int currentLevel;

    public GameObject bg1, bg2, bg3;


    public enum LEVEL_TYPE
    {
        COIN,
        BIG_COIN,
        GOBLIN,
        PRINCESS

    };

    public LEVEL_TYPE[] levelTypeLst;
    

    private void Awake()
    {
        _instance = this;
        LoadLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevelObj = (GameObject)Instantiate(Resources.Load<GameObject>("levels/Level " + currentLevel.ToString()));

        if(currentLevel >=1 && currentLevel <= 25)
        {
            bg1.SetActive(true);
            bg2.SetActive(false);
            bg3.SetActive(false);
        }    
        else
            if (currentLevel >= 26 && currentLevel <= 50)
        {
            bg1.SetActive(false);
            bg2.SetActive(true);
            bg3.SetActive(false);
        }
        else
            if (currentLevel >= 51 && currentLevel <= 75)
        {
            bg1.SetActive(false);
            bg2.SetActive(false);
            bg3.SetActive(true);
        }
    }

    public void CleanLevel()
    {
        Destroy(currentLevelObj);
        LevelPool._instance.CleanAlllObjects();
    }    
}
