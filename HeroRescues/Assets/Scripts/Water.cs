using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    public GameObject _rock,_smoke;
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
        if (collision.gameObject.tag == "Magma")
        {
             GameObject _smokeObj = (GameObject)Instantiate(_smoke, transform.position, transform.rotation);
             Destroy(_smokeObj, 3.0f);
             Destroy(gameObject);
          
        }
        if (collision.gameObject.tag == "Rock")
        {
            Destroy(gameObject);
          
        }
    }
}
