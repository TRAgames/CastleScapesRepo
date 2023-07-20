using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioList = new AudioClip[5];

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip[] GetAudioList
    {
        get { return _audioList; }
    }

}
