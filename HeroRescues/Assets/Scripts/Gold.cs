using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GameObject goldEffect, magmaEffect;

    public bool isGround;

    RaycastHit2D _hitGround;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 20) == 5 && goldEffect != null)
        {
            GameObject _effect = Instantiate(goldEffect, transform.position, transform.rotation);
            _effect.transform.parent = transform;
        }
        isGround = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Chest")
            _hitGround = Physics2D.Raycast(
                new Vector3(transform.position.x,
                transform.position.y - 0.5f,
                transform.position.z)
            , Vector2.down, 1.0f, LayerMask.GetMask("Static"));
        else
            _hitGround = Physics2D.Raycast(
               new Vector3(transform.position.x,
               transform.position.y,
               transform.position.z)
           , Vector2.down, 1.0f, LayerMask.GetMask("Static"));
        if (_hitGround.collider != null)
        {
             if(_hitGround.collider.tag == "Ground")
            isGround = true;
            //Debug.Log("COIN HIT " + _hitGround.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magma")
        {
            Destroy(gameObject);
            GameObject _magma = Instantiate(magmaEffect, transform.position, transform.rotation);
            Destroy(_magma, 1.0f);



            if (!Level._instance._hero.isDied)
            {
                Level._instance._hero.isDied = true;
                Level._instance._hero.SwitchState(Hero.PlayerState.Die);
                GameManager.instance.GameOver();
            }
        }


    }
}
