using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
public class Goblin : MonoBehaviour
{
	SkeletonAnimation _skeleton;

	Rigidbody2D _rigidbody;

	public float speed = 25.0f;

	[Header("STATE")]
	public PlayerState state;

	public bool _isWin = false;

	GameObject fireEffect;

	public GameObject _princess,_hero;

	public float moveHorizontal, moveVertical = 0.0f;

	public bool runWayPoint;

	public enum GoblinType
	{
		NO_WEAPON,
		SWORD,
		BOW
	};

	public GoblinType _currentType;



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

	private void Awake()
	{

		_rigidbody = GetComponent<Rigidbody2D>();

		_skeleton = transform.GetChild(0).GetComponent<SkeletonAnimation>();

		fireEffect = transform.GetChild(1).gameObject;

		if (runWayPoint)
			moveHorizontal = 1.0f;
		
	}
	// Start is called before the first frame update
	void Start()
	{
		//_skeleton = GetComponent<SkeletonAnimation>();
		//_princess = GameObject.FindGameObjectWithTag("Princess");
		//_hero = GameObject.FindGameObjectWithTag("Hero");

		
		switch(_currentType)
		{
			case GoblinType.NO_WEAPON:
				_skeleton.skeleton.SetSkin("default");
				break;
			case GoblinType.SWORD:
				_skeleton.skeleton.SetSkin("sword");
				break;
			case GoblinType.BOW:
				_skeleton.skeleton.SetSkin("bow");
				break;
			default:
				break;
		}
		//_skeleton.skeleton.SetSkin("sword");
		_skeleton.AnimationState.SetAnimation(0, "idle", true);

		InvokeRepeating("SeekTarget", 2.0f, 0.5f);

	}


	void SeekTarget()
	{
		//	Debug.Log("FIND" + transform.position.y + " " + _target.transform.position.y);

		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		if (_princess == null)
			_princess = GameObject.FindGameObjectWithTag("Princess");
		if (_hero == null)
			_hero = GameObject.FindGameObjectWithTag("Hero");
		if (_hero != null)
		{
			if (Mathf.Abs(transform.position.y - _hero.transform.position.y) <= 0.5f)
			{
				if(!CheckHitTarget(_hero.transform))
				moveHorizontal = (transform.position.x > _hero.transform.position.x) ? -1.0f : 1.0f;
				//Debug.Log("FIND");
			}
		}
		if (_princess != null)
		{
			if (Mathf.Abs(transform.position.y - _princess.transform.position.y) <= 0.5f)
			{
				if(!CheckHitTarget(_princess.transform))
				moveHorizontal = (transform.position.x > _princess.transform.position.x) ? -1.0f : 1.0f;
				//Debug.Log("FIND");
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
				_skeleton.AnimationState.SetAnimation(0, "idle", true);
				//this.StopMoving();
				break;
			case PlayerState.Move:
				_skeleton.AnimationState.SetAnimation(0, "idle_run", true);
				break;
			case PlayerState.Attack:
				_skeleton.AnimationState.SetAnimation(0, "sword", false);

				break;
			case PlayerState.Crouch:

				break;
			case PlayerState.Jump:

				break;
			case PlayerState.Die:
				_skeleton.AnimationState.SetAnimation(0, "die", false);
				Destroy(gameObject, 1.5f);
				break;
			case PlayerState.Win:
				_skeleton.AnimationState.SetAnimation(0, "win", true);
				break;
		}
	}

	IEnumerator EndTheLevel()
	{
		yield return new WaitForSeconds(1.0f);
		SoundManager.Instance.Play(SoundManager.Instance._sword);
		SwitchState(PlayerState.Idle);

		if (_princess != null)
		{
			_princess = GameObject.FindGameObjectWithTag("Princess");
			_princess.GetComponent<Princess>().SwitchState(Princess.PlayerState.Die);
		}	
			

		Level._instance._hero.isDied = true;
		Level._instance._hero.SwitchState(Hero.PlayerState.Die);

		GameManager.instance.GameOver();

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{


		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		if (collision.gameObject.tag == "Princess")
		{
			SwitchState(PlayerState.Attack);
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Goblin"));
			CancelInvoke("SeekTarget");
			moveHorizontal = 0.0f;
			StartCoroutine(EndTheLevel());
		}

		
		if (collision.gameObject.tag == "Magma")
		{
			if (!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);
				GameManager.instance._totalGoblinKilled++;
				fireEffect.GetComponent<ParticleSystem>().Play();
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));
			}
			else
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));
			if(LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel - 1] == LevelManager.LEVEL_TYPE.GOBLIN
				&& GameManager.instance._totalGoblinKilled == GameManager.instance._totalGoblin)
			{
				GameManager.instance.GameWin();
				if (_hero != null)
					_hero.GetComponent<Hero>().SwitchState(Hero.PlayerState.Win);
			}	
				

		}

		if(collision.gameObject.tag == "Saw")
		{
			if (!isDied)
			{
				isDied = true;
				GameManager.instance._totalGoblinKilled++;
				SwitchState(PlayerState.Die);
				if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel -1] == LevelManager.LEVEL_TYPE.GOBLIN
					&& GameManager.instance._totalGoblinKilled == GameManager.instance._totalGoblin)
				{
					GameManager.instance.GameWin();
					if (_hero != null)
						_hero.GetComponent<Hero>().SwitchState(Hero.PlayerState.Win);
				}

			}
		}

		if ((collision.gameObject.tag == "Chest" ||
			collision.gameObject.tag == "Sword"|| collision.gameObject.tag == "Rock" || collision.gameObject.tag == "Weight" ) && transform.position.y < collision.transform.position.y)
		{
			if (!isDied)
			{
				isDied = true;
				GameManager.instance._totalGoblinKilled++;
				SwitchState(PlayerState.Die);
				if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel -1] == LevelManager.LEVEL_TYPE.GOBLIN
					&& GameManager.instance._totalGoblinKilled == GameManager.instance._totalGoblin)
				{
					GameManager.instance.GameWin();
					if (_hero != null)
						_hero.GetComponent<Hero>().SwitchState(Hero.PlayerState.Win);
				}

			}

		}

		if (collision.gameObject.tag == "ToxicCloud")
		{
			if (!isDied)
			{
				isDied = true;
				GameManager.instance._totalGoblinKilled++;
				SwitchState(PlayerState.Die);
				//fireEffect.GetComponent<ParticleSystem>().Play();
				//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Air"));
				if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel-1] == LevelManager.LEVEL_TYPE.GOBLIN
					&& GameManager.instance._totalGoblinKilled == GameManager.instance._totalGoblin)
				{
					GameManager.instance.GameWin();
					if (_hero != null)
						_hero.GetComponent<Hero>().SwitchState(Hero.PlayerState.Win);
				}
			}
			//else
			//	Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Air"));

		}

		if (collision.gameObject.tag == "WayPoint")
		{
			if (moveHorizontal == -1.0f)
				moveHorizontal = 1.0f;
			else
				moveHorizontal = -1.0f;
		}
		}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Rock" && !_isWin &&  !isDied)
		{
			if (moveHorizontal != 0.0f)
				_rigidbody.AddForce(new Vector2(0.0f, 150.0f));
		}
	}

	public void GetDamage()
	{
		moveHorizontal = 0.0f;
		isDied = true;
		if(!this._skeleton.skeleton.FlipX)
		_rigidbody.AddForce(new Vector2(-100.0f, 0.0f));
		else
			_rigidbody.AddForce(new Vector2(100.0f, 0.0f));
		SwitchState(PlayerState.Idle);
		StartCoroutine(GetDamageIE());
	}

	IEnumerator GetDamageIE()
	{
		yield return new WaitForSeconds(1.0f);
		if (!this._skeleton.skeleton.FlipX)
			_rigidbody.AddForce(new Vector2(-200.0f, 0.0f));
		else
			_rigidbody.AddForce(new Vector2(200.0f, 0.0f));
		SwitchState(PlayerState.Die);
		GameManager.instance._totalGoblinKilled++;
	//	Debug.Log("Goblin " + GameManager.instance._totalGoblinKilled + " " + GameManager.instance._totalGoblin + " " + LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel]);
		if (LevelManager._instance.levelTypeLst[LevelManager._instance.currentLevel-1] == LevelManager.LEVEL_TYPE.GOBLIN
					&& GameManager.instance._totalGoblinKilled == GameManager.instance._totalGoblin)
		{
			GameManager.instance.GameWin();
			if (_hero != null)
				_hero.GetComponent<Hero>().SwitchState(Hero.PlayerState.Win);
		}

	}

	bool CheckHitTarget(Transform _target)
	{
		bool _check = false;
		RaycastHit2D _hitObj = Physics2D.Raycast(transform.position, _target.position - transform.position, Vector3.Distance(_target.position , transform.position), LayerMask.GetMask("Static"));
		//Debug.DrawRay(transform.position, _target.position - transform.position, Color.green);
		//foreach (RaycastHit2D hitObj in _hit)
		//	Debug.Log("HIT " + _hitObj.collider.gameObject.name);
		
		if (_hitObj)
		{
			_check = true;
			//Debug.Log("HIT " + _hitObj.collider.gameObject.name);
		}
			
		return _check;
	}

}
