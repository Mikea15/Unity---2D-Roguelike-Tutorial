using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour 
{
	public GameObject _gameManager;

	// Use this for initialization
	void Start () 
	{
		if( !GameManager.Instance )
		{
			Instantiate(_gameManager);
		}
	}

}
