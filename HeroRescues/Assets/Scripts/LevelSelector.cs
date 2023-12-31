﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Transform[] levelItemLst = new Transform[20];
    [HideInInspector]
    public int currentPage = 0,totalPage;

    [SerializeField] private Button _btnPreviousPage, _btnNextPage;
    public TextMeshProUGUI pageText;

    public Sprite unlockLevel,lockLevel;
    private void Awake()
    {
        currentPage = (int)(PlayerPrefs.GetInt("LockLevel")/ 20);
        if (currentPage == 3) currentPage = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("Item total " + GetChildByName("Holder").transform.childCount);
        for (int i = 0; i < GetChildByName("Holder").transform.childCount; i++)
            levelItemLst[i] = GetChildByName("Holder").transform.GetChild(i);
        ShowLevelItemInfo();
        if (currentPage == 2) _btnNextPage.gameObject.SetActive(false);
        else if (currentPage == 0) _btnPreviousPage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadItem()
    {
        
    }

    GameObject GetChildByName(string _name)
    {
        GameObject _child = null;
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        if (ts == null)
            return _child;
        foreach (Transform t in ts)
        {
            if (t != null && t.gameObject != null)
            {
                if (t.gameObject.name == _name)
                    _child = t.gameObject;

            }
        }

        return _child;
       
    }

    public void ShowLevelItemInfo()
    {
        pageText.text = "Страница " + (currentPage + 1).ToString();
        int _lockLevel = PlayerPrefs.GetInt("LockLevel");
        Debug.Log(_lockLevel);
        for(int i = 0; i < levelItemLst.Length; i++)
        {
            levelItemLst[i].Find("LevelText " + "(" + i + ")").GetComponent<TextMeshProUGUI>().text = (i + 1 + levelItemLst.Length * currentPage).ToString() + "";
            if (i + 1 + levelItemLst.Length * currentPage <= _lockLevel)
                levelItemLst[i].Find("Panel").GetComponent<Image>().sprite = unlockLevel;
            else
                levelItemLst[i].Find("Panel").GetComponent<Image>().sprite = lockLevel;
            //levelItemLst[i].GetComponent<Button>().onClick.AddListener(() => LoadLevel(i));
            /*
            levelItemLst[i].GetComponent<Button>().onClick.AddListener(() => {

                UIManager._instance.LoadLevel((i));
            
            });
            */
        }
    }

    public void LoadLevel(int _level)
    {
        if(_level <= PlayerPrefs.GetInt("LockLevel"))
         HomeManager.Instance.LoadLevel(_level + levelItemLst.Length * currentPage);
    }

    public void NextPage()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        if (currentPage < 2)
        {
            currentPage++;
            ShowLevelItemInfo();
        }

        if (currentPage == 2)
        {
            _btnNextPage.gameObject.SetActive(false);
            _btnPreviousPage.gameObject.SetActive(true);
        }
        else if (currentPage == 0)
        {
            _btnPreviousPage.gameObject.SetActive(false);
            _btnNextPage.gameObject.SetActive(true);
        }
        else
        {
            _btnNextPage.gameObject.SetActive(true);
            _btnPreviousPage.gameObject.SetActive(true);
        }

    }

    public void PrePage()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundManager.Instance.Play(SoundManager.Instance._btnClick);
        }

        if (currentPage > 0)
        {
            currentPage--;
            ShowLevelItemInfo();
        }
        if (currentPage == 2)
        {
            _btnNextPage.gameObject.SetActive(false);
            _btnPreviousPage.gameObject.SetActive(true);
        }
        else if (currentPage == 0)
        {
            _btnPreviousPage.gameObject.SetActive(false);
            _btnNextPage.gameObject.SetActive(true);
        }
        else
        {
            _btnNextPage.gameObject.SetActive(true);
            _btnPreviousPage.gameObject.SetActive(true);
        }
    }
}
