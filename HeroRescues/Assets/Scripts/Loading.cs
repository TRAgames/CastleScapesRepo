using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2.0f);
        Application.LoadLevel("MainHome");
    }    
}
