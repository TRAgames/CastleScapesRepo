using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinLong : MonoBehaviour
{
   
	public const float MAX_SWIPE_TIME = 0.5f;

	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;

	public Transform _to, _from,_edge;

	public float speed = 10.0f;

	private bool _canMove = false;


	Vector2 startPos;
	float startTime;

	public Vector3[] waypoints;
	int current = 0;
	float WPradius = 1;

	RaycastHit2D hit,_hitEdge;
	bool isCheckHitEdge;

	bool isShake = false, isMoving = false;


	private void Awake()
	{
		_to = transform.Find("Path").Find("To");
		_from = transform.Find("Path").Find("From");
		_edge = transform.Find("Box").Find("Edge");
		LoadWayPoint();
		isShake = false;
		isMoving = false;
		isCheckHitEdge = false;
	}

	void LoadWayPoint()
	{
		waypoints = new Vector3[2];
		waypoints[0] = transform.position;
		/*
		if (_from.position.x > _to.position.x)
			waypoints[1] = new Vector3(transform.position.x - 10.0f, transform.position.y, transform.position.z);
		else if(_from.position.x < _to.position.x)
			waypoints[1] = new Vector3(transform.position.x + 10.0f, transform.position.y, transform.position.z);
			*/
		Vector2 nr = (_to.position - _from.position).normalized;

		float angle = Mathf.Atan(nr.y / nr.x);
		//Debug.Log("Angle " + nr.x + " " + nr.y + " " + angle);
		waypoints[1] = new Vector3(transform.position.x + Mathf.Sign(nr.x) * Mathf.Cos(Mathf.Abs(angle)) * 100.0f,
			transform.position.y + Mathf.Sign(nr.y) * Mathf.Sin(Mathf.Abs(angle)) * 100.0f, transform.position.z);
	}

	public void Update()
	{
        if (_canMove) MoveOject();

        if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		/*
		if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
				startTime = Time.time;
			}
			if (t.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{ // Horizontal swipe
					if (swipe.x > 0)
					{
						swipedRight = true;
						Debug.Log("Swipe Right");
					}
					else
					{
						swipedLeft = true;
						Debug.Log("Swipe Left");
					}
				}
				else
				{ // Vertical swipe
					if (swipe.y > 0)
					{
						swipedUp = true;
						Debug.Log("Swipe Up");
					}
					else
					{
						swipedDown = true;
						Debug.Log("Swipe Down");
					}
				}
			}
		}
		*/
		/*

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 1.0f, LayerMask.GetMask("Drag"));
			

			startPos = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.width);
			startTime = Time.time;

		}
		if (Input.GetMouseButtonUp(0))
		{
			

				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{ // Horizontal swipe
					if (swipe.x > 0)
					{
						swipedRight = true;
						//Debug.Log("Swipe Right");
					}
					else
					{
						swipedLeft = true;
						//Debug.Log("Swipe Left");
					}
				}
				else
				{ // Vertical swipe
					if (swipe.y > 0)
					{
						swipedUp = true;
						//Debug.Log("Swipe Up");
					}
					else
					{
						swipedDown = true;
						//Debug.Log("Swipe Down");
					}
				}
			

			
		}
		if (hit.collider == null)
			return;
		if (hit.collider.name != gameObject.name)
			return;
		if (Input.GetMouseButtonUp(0))
			SoundManager.Instance.Play(SoundManager.Instance._pin);
		if (!isMoving)
		_hitEdge = Physics2D.Raycast(_edge.position, _to.position - _edge.position, 0.2f, LayerMask.GetMask("Drag"));
			
		
		if (_hitEdge)
		{
			Debug.Log("HIT EDGE " + _hitEdge.collider.name);
			
		}	
		
		
			if (((_from.position - _to.position).normalized.x > 0) && swipedLeft && hit.collider.name == gameObject.name && !_hitEdge)
		{
			isMoving = true;
			MoveOject();
		}	

	     	else if (((_from.position - _to.position).normalized.x > 0) && swipedRight && hit.collider.name == gameObject.name && !_hitEdge)
			StartCoroutine(Shake(this.transform));
	     	else if (_hitEdge && !isShake &&swipedLeft)
			 StartCoroutine(Shake(this.transform));

		   if (((_from.position - _to.position).normalized.x < 0) && swipedRight && hit.collider.name == gameObject.name && !_hitEdge)
		{
			isMoving = true;
			MoveOject();
		}		
		   else
		   if (((_from.position - _to.position).normalized.x < 0) && swipedLeft && hit.collider.name == gameObject.name && !_hitEdge)
			StartCoroutine(Shake(this.transform));
		else if (_hitEdge && !isShake && swipedRight)
			 StartCoroutine(Shake(this.transform));

		if (((_from.position - _to.position).normalized.y < 0) && swipedUp && hit.collider.name == gameObject.name && !_hitEdge)
		{
			isMoving = true;
			MoveOject();
		}		
		else if (((_from.position - _to.position).normalized.y < 0) && swipedDown && hit.collider.name == gameObject.name && !_hitEdge)
			StartCoroutine(Shake(this.transform));
		else if (_hitEdge && !isShake && swipedUp)
			 StartCoroutine(Shake(this.transform));


		
		if (((_from.position - _to.position).normalized.y > 0) && swipedDown && hit.collider.name == gameObject.name && !_hitEdge)
		{
			isMoving = true;
			MoveOject();
		}	
			
		else if (((_from.position - _to.position).normalized.y > 0) && swipedUp && hit.collider.name == gameObject.name && !_hitEdge)
			StartCoroutine(Shake(this.transform));
		else if (_hitEdge && !isShake && swipedDown)
			 StartCoroutine(Shake(this.transform));
		*/
	}

	void MoveOject()
	{
		if (Vector3.Distance(waypoints[current], transform.position) < WPradius)
		{
			current++;
			if (current >= waypoints.Length)
			{
				current = 0;
				speed = 0.0f;
				swipedUp = false;
				swipedDown = false;
				swipedLeft = false;
				swipedRight = false;
			}
		}
		transform.position = Vector3.MoveTowards(transform.position , waypoints[current], Time.deltaTime * speed);
	}

	IEnumerator Shake(Transform thisTransform)
	{
		
		Vector2 nr = (_to.position - _from.position).normalized;

		float angle = Mathf.Atan(nr.y / nr.x);
		/*
		waypoints[1] = new Vector3(transform.position.x + Mathf.Sign(nr.x) * Mathf.Cos(Mathf.Abs(angle)) * 10.0f,
			transform.position.y + Mathf.Sign(nr.y) * Mathf.Sin(Mathf.Abs(angle)) * 10.0f, transform.position.z);
			*/

	   isShake = true;
		Vector3 startPos = thisTransform.position;
		Vector3 endPos = Vector3.zero;
		/*
		if (swipedLeft)
			endPos = new Vector3(startPos.x - 0.18f, startPos.y, startPos.z);
		if (swipedRight)
			endPos = new Vector3(startPos.x + 0.18f, startPos.y, startPos.z);
		if (swipedUp)
			endPos = new Vector3(startPos.x, startPos.y + 0.18f, startPos.z);
		if (swipedDown)
			endPos = new Vector3(startPos.x, startPos.y - 0.18f, startPos.z);
			*/
			endPos = new Vector3(transform.position.x - Mathf.Sign(nr.x) * Mathf.Cos(Mathf.Abs(angle)) * 0.18f,
			transform.position.y - Mathf.Sign(nr.y) * Mathf.Sin(Mathf.Abs(angle)) * 0.18f, transform.position.z);
		/*
		if (swipedLeft)
			endPos = new Vector3(startPos.x - 0.18f, startPos.y,startPos.z);
		if(swipedRight)
			endPos = new Vector3(startPos.x + 0.18f, startPos.y, startPos.z);
		if(swipedUp)
			endPos = new Vector3(startPos.x, startPos.y + 0.18f, startPos.z);
		if(swipedDown)
			endPos = new Vector3(startPos.x, startPos.y - 0.18f, startPos.z);
			*/
		var i = 0.0f;
		var rate = 1.0f / 0.1f;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
			yield return null;
		}
		i = 0.0f;
		rate = 1.0f / 0.1f;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.localPosition = Vector3.Lerp(endPos, startPos, i);
			yield return null;
		}
		swipedUp = false;
		swipedDown = false;
		swipedLeft = false;
		swipedRight = false;
		isShake = false;
	}

    public void OnMouseDown()
    {
		_canMove = true;
    }
}
