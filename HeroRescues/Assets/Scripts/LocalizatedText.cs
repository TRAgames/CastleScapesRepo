using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LocalizatedText : MonoBehaviour
{
    [SerializeField] private bool _isManual = false;
    [SerializeField] private bool _isGameplay = false;
    public string key;

    public bool IsManual { get => _isManual; set => _isManual = value; }

    private void OnEnable()
    {
        if (!_isManual)
        {
            if (!_isGameplay)
            {
                TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
                text.text = LocalizationManager.Instance.GetLocalizedValue(key);
            }
            else
            {
                TextMeshPro text = GetComponent<TextMeshPro>();
                text.text = LocalizationManager.Instance.GetLocalizedValue(key);
            }
        }
    }

    public void SetText(string localKey)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(localKey);      
    }

    public void SetTextGameplay(string localKey)
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        text.text = LocalizationManager.Instance.GetLocalizedValue(localKey);
    }
}