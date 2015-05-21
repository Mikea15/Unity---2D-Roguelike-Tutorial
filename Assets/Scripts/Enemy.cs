using UnityEngine;
using System.Collections;

public class Enemy : MovingObject 
{
	[SerializeField] private int _playerDamage;

	[SerializeField] private AudioClip[] _enemyAttack;

	private Animator _anim;
	private Transform _target;

	private bool _skipMove;
	
	protected override void Start () 
	{
		_anim = this.GetComponent<Animator>();
		_target = GameObject.FindGameObjectWithTag("Player").transform;

		GameManager.Instance.AddEnemyToList(this);

		base.Start();
	}

	protected override void OnCantMove<T> (T component)
	{
		Player hitPlayer = component as Player;

		_anim.SetTrigger("enemyAttack");

		SoundManager.Instance.RandomizeFx(_enemyAttack);

		hitPlayer.loseFood(_playerDamage);
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		if( _skipMove ) {
			_skipMove = false;
			return;
		}
			
		base.AttemptMove<T> (xDir, yDir);

		_skipMove = true;
	}

	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;

		if( Mathf.Abs (_target.position.x - transform.position.x ) < float.Epsilon )
			yDir = _target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = _target.position.x > transform.position.x ? 1 : -1;

		AttemptMove<Player>(xDir, yDir);
	}
}
