using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    public GameObject coin1, coin2, coin3,coin4;

    public int totalCoin = 30;

    GameObject coinPre;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCoin()
    {
        int _currentTotal = 0;
        int _random = 0;

        while(_currentTotal < totalCoin)
        {
          
            _random = Random.Range(0, 10);
            Vector3 pos = new Vector3(gameObject.transform.position.x + Random.Range(-0.25f, 0.25f),
                    gameObject.transform.position.y + Random.Range(-0.25f, 0.25f), 0.0f);
            if (_random >= 0 && _random < 3)
                coinPre =  Instantiate(coin1, pos, transform.rotation);
            else if(_random >= 3 && _random < 6)
                coinPre = Instantiate(coin2, pos, transform.rotation);
            else if (_random >= 6 && _random < 8)
                coinPre = Instantiate(coin4, pos, transform.rotation);
            else
                coinPre = Instantiate(coin3, pos, transform.rotation);
            _currentTotal++;
          //  LevelPool._instance.AddObject(coinPre);

        }
    }
}
