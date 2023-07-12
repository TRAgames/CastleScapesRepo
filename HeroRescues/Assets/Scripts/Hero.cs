using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hero : MonoBehaviour
{
	SkeletonAnimation _skeleton;

	 Rigidbody2D _rigidbody;

	public float speed = 15.0f;

	[Header("STATE")]
	public PlayerState state;

	public bool _isWin = false, reachTarget = false;

	GameObject fireEffect;

	GameObject _princess,_coin, _target,_sword,_chest;

	float moveHorizontal, moveVertical = 0.0f;

	RaycastHit2D _hitWall;



	public enum PlayerState
	{
		Idle,
		Move,
		Crouch,
		Jump,
		Die,
		Win,
		Attack
	}

	public bool isDied = false;

	bool hasWeapon;

	private void Awake()
	{

		_rigidbody = GetComponent<Rigidbody2D>();

		_skeleton = transform.GetChild(1).GetComponent<SkeletonAnimation>();

		fireEffect = transform.GetChild(3).gameObject;

		hasWeapon = false;
		reachTarget = false;
	}
	// Start is called before the first frame update
	void Start()
	{
		

		_skeleton.AnimationState.SetAnimation(0, "idle", true);

		if (_target == null)
		{
			_coin = GameObject.FindGameObjectWithTag("Coin");
			_sword = GameObject.FindGameObjectWithTag("Sword");
			_princess = GameObject.FindGameObjectWithTag("Princess");
			_chest = GameObject.FindGameObjectWithTag("Chest");

			if (_princess != null)
				_target = _princess;
			if (_coin != null)
				_target = _coin;
			if (_chest != null)
				_target = _chest;
		}
		//moveHorizontal = 1.0f;

		InvokeRepeating("SeekTarget", 2.0f, 0.5f);
		InvokeRepeating("SeekSword", 2.0f, 0.5f);
		

	}


	void SeekTarget()
	{

		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		

		if (_target != null)
		{
			if (_target.tag == "Coin")
			{
                bool _canGo = false;
                _hitWall = Physics2D.Raycast(new Vector3(transform.position.x - 0.35f * transform.localScale.x,
                      transform.position.y + 0.5f, transform.position.z)
                    , -Vector2.right * transform.localScale.x, 0.5f,
                    LayerMask.GetMask("Static"));
                Debug.DrawLine(new Vector3(transform.position.x - 0.5f * transform.localScale.x,
                  transform.position.y, transform.position.z), new Vector3(transform.position.x - 1.0f * transform.localScale.x,
                  transform.position.y,transform.position.z) , Color.red);
                if (_hitWall.collider != null)
                {
                    if ((_hitWall.collider.tag == "Ground" || _hitWall.collider.tag == "Pin"))
                        _canGo = false;
                    else
                        _canGo = true;
                    //Debug.Log("TAG" + _hitWall.collider.tag);
                }
                else
                {
                   // Debug.Log("GO");
                    _canGo = true;
                }
               

                if (_target.GetComponent<Gold>().isGround && transform.position.y >= _target.transform.position.y && _canGo)
				{
					reachTarget = true;
					moveHorizontal = (transform.position.x > _target.transform.position.x) ? -1.0f : 1.0f;
				}	
					

				/*
				if(Vector3.Distance(transform.position,_target.transform.position) < 2.0f)
				moveHorizontal = (transform.position.x > _target.transform.position.x)? -1.0f : 1.0f;
				else
				{
					moveHorizontal = 1.0f;
				}	
				*/
			}
			else if(_target.tag == "Princess" && Vector3.Distance(transform.position, _target.transform.position) < 3.0f && _target.transform.position.y < -2.0f)
			{
                bool _canGo = false;
                _hitWall = Physics2D.Raycast(new Vector3(transform.position.x - 0.35f * transform.localScale.x,
                       transform.position.y + 0.5f, transform.position.z)
                     , -Vector2.right * transform.localScale.x, 0.5f,
                     LayerMask.GetMask("Static"));
                if (_hitWall.collider != null)
                {
                    if ((_hitWall.collider.tag == "Ground" || _hitWall.collider.tag == "Pin"))
                        _canGo = false;
                    else
                        _canGo = true;
                   // Debug.Log("TAG" + _hitWall.collider.tag);
                }
                else
                {
                   // Debug.Log("GO");
                    _canGo = true;
                }

				

				if (_canGo)
                {
                    reachTarget = true;
                    moveHorizontal = (transform.position.x > _target.transform.position.x) ? -1.0f : 1.0f;
                }

			}
			else if(_target.tag == "Princess")
			{
				bool _nearPrincess = false;

				RaycastHit2D _hitPrincessLeft = Physics2D.Raycast(new Vector3(transform.position.x - 0.35f * transform.localScale.x,
					   transform.position.y + 0.5f, transform.position.z)
					 , -Vector2.right * transform.localScale.x, 1.0f,
					 LayerMask.GetMask("Hero"));

				RaycastHit2D _hitPrincessRight = Physics2D.Raycast(new Vector3(transform.position.x + 0.35f * transform.localScale.x,
					   transform.position.y + 0.5f, transform.position.z)
					 , -Vector2.left * transform.localScale.x, 1.0f,
					 LayerMask.GetMask("Hero"));

				Debug.DrawLine(new Vector3(transform.position.x + 0.35f * transform.localScale.x,
					   transform.position.y + 0.5f, transform.position.z),
					   new Vector3(transform.position.x + 0.35f * transform.localScale.x, transform.position.y + 0.5f, transform.position.z) - Vector3.left * transform.localScale.x, Color.white);

				if ((_hitPrincessLeft.collider != null))
				{
					if (_hitPrincessLeft.collider.tag == "Princess")
					{
						//Debug.Log("HIT PRINCESS");
						_nearPrincess = true;
					}	
						
				}

				if ((_hitPrincessRight.collider != null))
				{
					if ( _hitPrincessRight.collider.tag == "Princess")
					{
						//Debug.Log("HIT PRINCESS");
						_nearPrincess = true;
					}

				}

				if (_nearPrincess)
				{
					reachTarget = true;
					moveHorizontal = (transform.position.x > _target.transform.position.x) ? -1.0f : 1.0f;
				}


			}

			else if (_target.tag == "Chest")
			{

				if (_target.GetComponent<Gold>().isGround && transform.position.y >= (_target.transform.position.y - 0.5f))
				{
                    bool _canGo = false;
                    _hitWall = Physics2D.Raycast(new Vector3(transform.position.x - 0.35f * transform.localScale.x,
                      transform.position.y + 0.5f, transform.position.z)
                    , -Vector2.right * transform.localScale.x, 0.5f,
                    LayerMask.GetMask("Static"));

                    if (_hitWall.collider != null)
                    {
                        if ((_hitWall.collider.tag == "Ground" || _hitWall.collider.tag == "Pin"))
                            _canGo = false;
                        else
                            _canGo = true;
                        //Debug.Log("TAG" + _hitWall.collider.tag);
                    }
                    else
                    {
                        //Debug.Log("GO");
                        _canGo = true;
                    }

                    if(_canGo)
                    {
                        reachTarget = true;
                        moveHorizontal = (transform.position.x > _target.transform.position.x) ? -1.0f : 1.0f;
                    }

				}
			}


			}
	}

	void SeekSword()
	{
		//	Debug.Log("FIND" + transform.position.y + " " + _target.transform.position.y);
		if (_sword != null)
		{
			if (Mathf.Abs(transform.position.y - _sword.transform.position.y) < 0.5f)
			{
				moveHorizontal = (transform.position.x > _sword.transform.position.x) ? -1.0f : 1.0f;
				reachTarget = true;
			}
		}
	}



	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;
		if (_isWin)
			return;
		if (isDied)
			return;
		if (state == PlayerState.Attack)
			return;

        _hitWall = Physics2D.Raycast(new Vector3(transform.position.x - 0.3f * transform.localScale.x,
                 transform.position.y + 0.5f, transform.position.z)
               , -Vector2.right * transform.localScale.x, 0.1f,
               LayerMask.GetMask("Static"));
        if (_hitWall.collider != null)
		{
			if ((_hitWall.collider.tag == "Ground" || _hitWall.collider.tag == "Pin") && reachTarget)
			{
				//Debug.Log("HIT WALL");
				if (moveHorizontal == 1.0f)
					moveHorizontal = -1.0f;
				else
					moveHorizontal = 1.0f;
			}
		}

		//moveHorizontal = 1.0f;
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		if (moveHorizontal < 0)
			this.transform.localScale = new Vector3(1, 1, 1);
		else if (moveHorizontal > 0)
			this.transform.localScale = new Vector3(-1, 1, 1);

		//ebug.Log("x  " + moveHorizontal);

		if (moveHorizontal != 0)
			SwitchState(PlayerState.Move);
		else
			SwitchState(PlayerState.Idle);

		_rigidbody.AddForce(movement * speed);
	}

	public void SwitchState(PlayerState newState)
	{
		if (this.state == newState)
		{
			return;
		}
		this.state = newState;
		switch (this.state)
		{
			case PlayerState.Idle:
				if(!hasWeapon)
				    _skeleton.AnimationState.SetAnimation(0, "idle", true);
				else
					_skeleton.AnimationState.SetAnimation(0, "idle_sword", true);
				//this.StopMoving();
				break;
			case PlayerState.Move:
				if (!hasWeapon)
					_skeleton.AnimationState.SetAnimation(0, "run", true);
				else
					_skeleton.AnimationState.SetAnimation(0, "run_sword", true);
				break;
			case PlayerState.Crouch:

				break;
			case PlayerState.Attack:
				_skeleton.AnimationState.SetAnimation(0, "kill_sword", true);
				break;
			case PlayerState.Jump:

				break;
			case PlayerState.Die:
				if (!hasWeapon)
					_skeleton.AnimationState.SetAnimation(0, "lose", false);
				else
					_skeleton.AnimationState.SetAnimation(0, "lose_sword", true);
				//Debug.Log("PLAY DIE");
				break;
			case PlayerState.Win:
				if (!hasWeapon)
					_skeleton.AnimationState.SetAnimation(0, "win", true);
				else
					_skeleton.AnimationState.SetAnimation(0, "win_sword", true);
				break;
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{

		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		if (collision.gameObject.tag == "Coin" || collision.gameObject.tag == "Chest")
		{
			if (!_isWin)
			{
				//GameObject[] coinAll = GameObject.FindGameObjectsWithTag("Coin");
				//GameManager.instance.AddCoin(coinAll.Length);
				_isWin = true;
				//GameManager.instance.bonusCoin += coinAll.Length;
			}

			SwitchState(PlayerState.Win);
			GameManager.instance.GameWin();
			SoundManager.Instance.Play(SoundManager.Instance._coinDrop1);

		}


		if (collision.gameObject.tag == "Sword")
		{
			hasWeapon = true;
			moveHorizontal = 0.0f;
			_skeleton.AnimationState.SetAnimation(0, "idle_sword", true);
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.tag == "Goblin")
		{
			//Debug.Log("KILL");
			if (hasWeapon)
			{
				//GameManager.instance.isGameWin = true;
				if (transform.position.x > collision.transform.position.x)
					this.transform.localScale = new Vector3(1, 1, 1);
				else 
					this.transform.localScale = new Vector3(-1, 1, 1);
				SwitchState(PlayerState.Attack);
				collision.gameObject.GetComponent<Goblin>().GetDamage();
				StartCoroutine(ReleaseAttack());
				
					
			}
			else
			{
				isDied = true;
				collision.gameObject.GetComponent<Goblin>().SwitchState(Goblin.PlayerState.Attack);
				if (GameManager.instance._life > 0)
					GameManager.instance._life--;
				UIManager._instance.UpdateLife(GameManager.instance._life);
				SwitchState(PlayerState.Die);
				SoundManager.Instance.Play(SoundManager.Instance._princessDie);
				GameManager.instance.GameOver();
			}


		}


		if (collision.gameObject.tag == "Princess")
		{
			_isWin = true;
			SwitchState(PlayerState.Win);
			moveHorizontal = 0.0f;
			GameManager.instance.GameWin();
		}

		if (collision.gameObject.tag == "Magma")
		{
			if (!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);
				SoundManager.Instance.Play(SoundManager.Instance._princessDie);
				fireEffect.GetComponent<ParticleSystem>().Play();
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));
				if (GameManager.instance._life > 0)
					GameManager.instance._life--;
				UIManager._instance.UpdateLife(GameManager.instance._life);
				GameManager.instance.GameOver();
			}
			else
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));

		}

		if (collision.gameObject.tag == "Saw")
		{
			if (!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);
				SoundManager.Instance.Play(SoundManager.Instance._princessDie);
				if (GameManager.instance._life > 0)
					GameManager.instance._life--;
				UIManager._instance.UpdateLife(GameManager.instance._life);
				GameManager.instance.GameOver();
			}
			

		}

		if (collision.gameObject.tag == "ToxicCloud")
		{
			if (_isWin)
				return;
			if (!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);
				SoundManager.Instance.Play(SoundManager.Instance._princessDie);
				//fireEffect.GetComponent<ParticleSystem>().Play();
				//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Air"));
				if (GameManager.instance._life > 0)
					GameManager.instance._life--;
				UIManager._instance.UpdateLife(GameManager.instance._life);
				GameManager.instance.GameOver();
			}
			else
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Air"));

		}

	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Rock" && !_isWin && !isDied)
		{
			if(moveHorizontal != 0.0f)
			_rigidbody.AddForce(new Vector2(0.0f, 150.0f));
		}
	}

	IEnumerator ReleaseAttack()
	{
		yield return new WaitForSeconds(0.5f);
		SoundManager.Instance.Play(SoundManager.Instance._sword);
		yield return new WaitForSeconds(0.5f);
		if (_princess == null && _coin == null && _chest == null)
		{
			SwitchState(PlayerState.Win);
			GameManager.instance.GameWin();
		}
		else
		SwitchState(PlayerState.Idle);
	}
}
