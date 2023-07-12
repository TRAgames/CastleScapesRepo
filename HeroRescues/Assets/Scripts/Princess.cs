using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Princess : MonoBehaviour
{
    SkeletonAnimation _skeleton;

    Rigidbody2D _rigidbody;

	GameObject fireEffect;

    public float speed;

    [Header("STATE")]
    public PlayerState state;

    public bool _isWin = false;

    public enum PlayerState
    {
        Idle,
        Move,
        Crouch,
        Jump,
        Die,
        Win
    }

    public bool isDied = false;
    // Start is called before the first frame update
    void Start()
    {
		_rigidbody = GetComponent<Rigidbody2D>();

		_skeleton = transform.GetChild(0).GetComponent<SkeletonAnimation>();

		fireEffect = transform.GetChild(1).gameObject;

		_skeleton.AnimationState.SetAnimation(0, "idle", true);
		
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
				_skeleton.AnimationState.SetAnimation(0, "run", true);
				break;
			case PlayerState.Crouch:

				break;
			case PlayerState.Jump:

				break;
			case PlayerState.Die:
				_skeleton.AnimationState.SetAnimation(0, "die(red)", false);
				Debug.Log("PLAY DIE");
				break;
			case PlayerState.Win:
				_skeleton.AnimationState.SetAnimation(0, "win", true);
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{


		if (GameManager.instance.isGameOver || GameManager.instance.isGameWin)
			return;

		if (collision.gameObject.tag == "Magma")
		{
			if(!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);
				GameObject _fire = Instantiate(fireEffect, transform.position, transform.rotation);
				_fire.GetComponent<ParticleSystem>().Play();
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));
				GameManager.instance.GameOver();
			}
			else
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Hero"), LayerMask.NameToLayer("Lava"));

		}


		if ((collision.gameObject.tag == "Weight" || collision.gameObject.tag == "Rock" || collision.gameObject.tag == "Saw") & (transform.position.y + 0.5f)< collision.transform.position.y)
		{
			if (!isDied)
			{
				isDied = true;
				SwitchState(PlayerState.Die);		
				GameManager.instance.GameOver();
			}
			

		}

		if (collision.gameObject.tag == "Hero")
		{
			_isWin = true;
			SwitchState(PlayerState.Win);
		}
	}
}
