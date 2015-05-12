using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = null;

	[SerializeField] BoardManager _boardManager;



	[SerializeField] private int _playerFoodPoints = 100;
	public int PlayerFoodPoints { 
		get { return _playerFoodPoints; } 
		set { _playerFoodPoints = value; }
	}

	public bool PlayersTurn {
		get; set;
	}

	private int level = 3;

	void Awake()
	{
		if( Instance == null )
			Instance = this;
		else if( Instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		_boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	public void InitGame()
	{
		_boardManager.SetupScene(level);
	}

	public void GameOver( )
	{
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
