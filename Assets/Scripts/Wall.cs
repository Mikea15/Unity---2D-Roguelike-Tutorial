using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour 
{
	[SerializeField] private Sprite _dmgSprite;
	[SerializeField] private int _hp = 4;

	private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Awake() 
	{
		_spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	

	public void DamageWall( int loss ) 
	{
		_spriteRenderer.sprite = _dmgSprite;
		_hp -= loss;
		if( _hp <= 0 )
			gameObject.SetActive(false);
	}
}
