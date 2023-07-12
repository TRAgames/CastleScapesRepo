using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 1.0f, LayerMask.GetMask("Rope"));

            if(hit.collider != null)
            {
                if (hit.collider.gameObject.name == this.transform.gameObject.name)
                    Destroy(transform.parent.gameObject);

            }

        }
    }
}
