using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = new GameManager();

	[SerializeField] BoardManager _boardManager;

	private int level = 3;

	void Awake()
	{
		_boardManager = GetComponent<BoardManager>();
		InitGame();
	}

	public void InitGame()
	{
		_boardManager.SetupScene(level);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
