using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
		
		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	
	
	[SerializeField] private int _columns = 8;
	[SerializeField] private int _rows = 8;
	
	[SerializeField] private Count _wallCount = new Count(5, 9);
	[SerializeField] private Count _foodCount = new Count(5, 9);
	
	[SerializeField] private GameObject exit;
	[SerializeField] private GameObject[] _floorTiles;
	[SerializeField] private GameObject[] _wallTiles;
	[SerializeField] private GameObject[] _foodTiles;
	[SerializeField] private GameObject[] _enemyTiles;
	[SerializeField] private GameObject[] _outerWallTitles;
	
	private Transform _boardHolder;
	private List<Vector3> _gridPositions = new List<Vector3>();
	
	public void InitializeList() 
	{
		_gridPositions.Clear ();
		
		for( int x = 1; x < _columns -1; x++ )
		{
			for( int y = 1; y < _rows -1; y++ )
			{
				_gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}
	
	public void BoardSetup()
	{
		_boardHolder = new GameObject("Board").transform;
		
		for( int x = -1; x < _columns +1; x++ )
		{
			for( int y = -1; y < _rows +1; y++ )
			{
				GameObject g0 = _floorTiles[Random.Range(0 ,_floorTiles.Length)];
				if( x == -1 || x == _columns || y == -1 || y == _rows )
					g0 = _outerWallTitles[Random.Range(0, _outerWallTitles.Length)];
				
				GameObject instantiate = Instantiate( g0, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				
				instantiate.transform.SetParent(_boardHolder);
			}
		}
	}
	
	public Vector3 RandomPosition( )
	{
		int index = Random.Range(0, _gridPositions.Count);
		Vector3 randPosition = _gridPositions[index];
		_gridPositions.RemoveAt(index);
		return randPosition;
	}
	
	public void LayoutObjectAtRandom( GameObject[] tiles, int min, int max )
	{
		int objectCount = Random.Range(min, max + 1);
		for( int i = 0; i < objectCount; i++)
		{
			Vector3 randomPos = RandomPosition ();
			var tileChoice = tiles[Random.Range(0, tiles.Length)] as GameObject;
			Instantiate( tileChoice, randomPos, Quaternion.identity );
		}
	}
	
	public void SetupScene( int level )
	{
		BoardSetup();
		InitializeList();
		LayoutObjectAtRandom(_wallTiles, _wallCount.minimum, _wallCount.maximum);
		LayoutObjectAtRandom(_foodTiles, _foodCount.minimum, _foodCount.maximum);
		
		int enemyCount = (int) Mathf.Log( level, 2f );
		LayoutObjectAtRandom(_enemyTiles, enemyCount, enemyCount);
		Instantiate( exit, new Vector3( _columns -1, _rows -1, 0f), Quaternion.identity);
	}
}
