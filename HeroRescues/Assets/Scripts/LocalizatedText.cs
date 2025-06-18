using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LocalizatedText : MonoBehaviour
{
    [SerializeField] private bool _isManual = false;
    public string key;

    public bool IsManual { get => _isManual; set => _isManual = value; }

    private void OnEnable()
    {
        if (!_isManual)
        {
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
            text.text = LocalizationManager.Instance.GetLocalizedValue(key);
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