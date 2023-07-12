using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePad : MonoBehaviour
{
    public Transform boxZone;

    RaycastHit2D hit;

    Transform leftPoint, rightPoint,centerPoint;

    bool isMoving = false;

    public bool isVertical;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        boxZone = transform.parent;
        leftPoint = transform.parent.transform.parent.Find("Path").GetChild(2);
        rightPoint = transform.parent.transform.parent.Find("Path").GetChild(1);
        centerPoint = transform.parent.transform.parent.Find("Path").GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero,1.0f, LayerMask.GetMask("Ignore Raycast"));

            if (hit.collider == null)
                return;
            if (hit.collider.transform.parent.transform.parent.gameObject.name == transform.parent.transform.parent.gameObject.name)
            {
                if (isMoving)
                    return;
                if(isVertical)
                {
                    if (boxZone.localPosition.x < centerPoint.localPosition.x)
                    {
                        //Debug.Log("go to from " +boxZone.position.x +" to " + leftPoint.position.x);
                        //StartCoroutine(Move_Routine(boxZone, boxZone.localPosition, leftPoint.localPosition));
                        //oxZone.localPosition = leftPoint.localPosition;
                        StartCoroutine(MoveObject(boxZone, boxZone.localPosition, leftPoint.localPosition, 0.5f));
                    }

                    else
                    {
                        // Debug.Log("go to from " + boxZone.position.x + " to " + leftPoint.position.x);
                        // StartCoroutine(Move_Routine(boxZone, boxZone.localPosition, rightPoint.localPosition));
                        // boxZone.localPosition = rightPoint.localPosition;
                        StartCoroutine(MoveObject(boxZone, boxZone.localPosition, rightPoint.localPosition, 0.5f));
                    }
                }
                else
                {
                    if (boxZone.localPosition.y > centerPoint.localPosition.y)
                    {
                     
                        StartCoroutine(MoveObject(boxZone, boxZone.localPosition, leftPoint.localPosition, 0.5f));
                    }

                    else
                    {
                       
                        StartCoroutine(MoveObject(boxZone, boxZone.localPosition, rightPoint.localPosition, 0.5f));
                    }
                }
                
                   
            }
        }

        
        
    }
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        isMoving = true;
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        isMoving = false;
    }
}
