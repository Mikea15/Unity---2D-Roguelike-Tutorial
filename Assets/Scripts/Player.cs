﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject 
{
	[SerializeField] private int _wallDamage = 1;
	[SerializeField] private int _pointsPerFood = 10;
	[SerializeField] private int _pointsPerSoda = 20;

	[SerializeField] private float _restartLevelDelay = 1f;

	[SerializeField] private Text _foodText;

	[SerializeField] private AudioClip[] _moveSounds;
	[SerializeField] private AudioClip[] _eatSounds;
	[SerializeField] private AudioClip[] _drinkSounds;
	[SerializeField] private AudioClip _gameOverSound;


	private Animator _anim;
	private int _food;

	// Use this for initialization
	protected override void Start( )
	{
		_anim = this.GetComponent<Animator>();
		_food = GameManager.Instance.PlayerFoodPoints;

		_foodText.text = "Food:  " + _food;

		base.Start();
	}

	private void OnDisable( )
	{
		GameManager.Instance.PlayerFoodPoints = _food;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !GameManager.Instance.PlayersTurn )
			return;

		int horizontal = (int) Input.GetAxisRaw("Horizontal");
		int vertical = (int) Input.GetAxisRaw("Vertical");

		if( horizontal != 0 )
			vertical = 0;

		if( horizontal != 0 || vertical != 0)
			AttemptMove<Wall>(horizontal, vertical);
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		_food--;
		_foodText.text = "Food: " + _food;

		base.AttemptMove<T>(xDir, yDir);

		RaycastHit2D hit;

		if( Move (xDir, yDir, out hit) )
		{
			SoundManager.Instance.RandomizeFx(_moveSounds);
		}

		CheckIfGameOver();
		GameManager.Instance.PlayersTurn = false;
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		if( other.tag == "Exit" )
		{
			Invoke ("Restart", _restartLevelDelay);
			enabled = false;
		}
		else if( other.tag == "Food" )
		{
			_food += _pointsPerFood;
			_foodText.text = "+" + _pointsPerFood + " Food: " + _food;

			SoundManager.Instance.RandomizeFx(_eatSounds);

			other.gameObject.SetActive(false);
		}
		else if( other.tag == "Soda" )
		{
			_food += _pointsPerSoda;
			_foodText.text = "+" + _pointsPerSoda + " Food: " + _food;

			SoundManager.Instance.RandomizeFx(_drinkSounds);

			other.gameObject.SetActive(false);
		}
	}

	protected override void OnCantMove<T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall(_wallDamage);
		_anim.SetTrigger("playerChop");
	}

	private void Restart( )
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void loseFood( int loss )
	{
		_anim.SetTrigger("playerHit");

		_food -= loss;
		_foodText.text = "-" + loss + " Food: " + _food;

		CheckIfGameOver();
	}

	private void CheckIfGameOver( )
	{
		if ( _food <= 0 ) {
			SoundManager.Instance.PlaySingle(_gameOverSound);
			SoundManager.Instance.MusicSource.Stop();
			GameManager.Instance.GameOver();

		}
	}
}
