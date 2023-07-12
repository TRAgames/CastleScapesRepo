using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma : MonoBehaviour
{
    public GameObject _rock;

    bool convert = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water" && convert == false)
        {
            convert = true;
            GameObject _rockObj1 = (GameObject) Instantiate(_rock, transform.position, transform.rotation);
            SoundManager.Instance.Play(SoundManager.Instance._rock);
            // LevelPool._instance.AddObject(_rockObj1);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Rock" & convert == false)
        {
             convert = true;
             GameObject _rockObj2 = (GameObject)Instantiate(_rock, transform.position, transform.rotation);
          //  LevelPool._instance.AddObject(_rockObj2);
            Destroy(gameObject);
           
        }

    }

}
