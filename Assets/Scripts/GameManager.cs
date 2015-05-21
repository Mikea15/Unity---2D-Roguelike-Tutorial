using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = null;

	[SerializeField] private float _levelStartDelay = 2f;
	[SerializeField] BoardManager _boardManager;
	[SerializeField] private float _turnDelay = .1f;
	
	[SerializeField] private int _playerFoodPoints = 100;
	public int PlayerFoodPoints { 
		get { return _playerFoodPoints; } 
		set { _playerFoodPoints = value; }
	}

	public bool PlayersTurn {
		get; set;
	}

	private int level = 1;
	private Text _levelText;
	private GameObject _levelImage;
	private bool _doingSetup;

	private List<Enemy> _enemies;
	private bool _enemiesMoving;

	void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( Instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		_boardManager = GetComponent<BoardManager>();

		_enemies = new List<Enemy>();


		InitGame();
	}

	private void OnLevelWasLoaded( int index )
	{
		level++;

		InitGame();
	}

	public void InitGame()
	{
		_doingSetup = true;

		_levelImage = GameObject.Find ("LevelImage");
		_levelText = GameObject.Find ("LevelText").GetComponent<Text>();

		_levelText.text = "Day " + level;
		_levelImage.SetActive(true);

		Invoke ("HideLevelImage", _levelStartDelay);

		_enemies.Clear();
		_boardManager.SetupScene(level);
	}

	private void HideLevelImage()
	{
		_levelImage.SetActive(false);
		_doingSetup = false;
	}

	public void GameOver( )
	{
		_levelText.text = "After " + level + " days, you starved.";
		_levelImage.SetActive(true);


		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( PlayersTurn || _enemiesMoving || _doingSetup )
			return;

		StartCoroutine(MoveEnemies());
	}

	public void AddEnemyToList( Enemy script )
	{
		_enemies.Add(script);
	}

	IEnumerator MoveEnemies()
	{
		_enemiesMoving = true;
		yield return new WaitForSeconds(_turnDelay);
		if( _enemies.Count == 0 )
		{
			yield return new WaitForSeconds(_turnDelay);
		}

		for( int i = 0; i < _enemies.Count; i++ )
		{
			_enemies[i].MoveEnemy();
			yield return new WaitForSeconds(_enemies[i].MoveTime);
		}

		PlayersTurn = true;
		_enemiesMoving = false;
	}
}
