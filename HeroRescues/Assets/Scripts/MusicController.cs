using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    [SerializeField] private AudioSource _audioSource;

    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else
        {
            Destroy(gameObject);
        }
    }    
}
