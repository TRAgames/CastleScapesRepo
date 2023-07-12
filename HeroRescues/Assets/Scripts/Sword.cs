using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Transform _fire;
    // Start is called before the first frame update
    void Start()
    {
        _fire = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magma")
        {
            _fire.gameObject.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject, 2.0f);
        }
    }
}
