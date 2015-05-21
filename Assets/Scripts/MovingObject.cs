using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour 
{
	// seconds
	[SerializeField] private float _moveTime = 0.1f;
	public float MoveTime {
		get { return _moveTime; }
	}

	[SerializeField] private LayerMask _blockingLayer;

	private BoxCollider2D _boxCollider;
	private Rigidbody2D _rigidbody2D;

	private float _inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () 
	{
		_boxCollider = this.GetComponent<BoxCollider2D>();
		_rigidbody2D = this.GetComponent<Rigidbody2D>();
		_inverseMoveTime = 1f / _moveTime;
	}

	protected bool Move( int xDir, int yDir, out RaycastHit2D hit )
	{
		Vector2 start = this.transform.position; // discards automatically.
		Vector2 end = start + new Vector2(xDir, yDir);

		_boxCollider.enabled = false;
		hit = Physics2D.Linecast(start, end, _blockingLayer);
		_boxCollider.enabled = true;

		if( hit.transform == null )
		{
			StartCoroutine(SmoothMovement(end));
			return true;
		}
		return false;
	}

	protected IEnumerator SmoothMovement( Vector3 end )
	{
		float sqrRemainingDistance = ( this.transform.position -end ).sqrMagnitude;

		while( sqrRemainingDistance > float.Epsilon )
		{
			Vector3 newPos = Vector3.MoveTowards(_rigidbody2D.position, end, _inverseMoveTime * Time.deltaTime );

			_rigidbody2D.MovePosition(newPos);

			sqrRemainingDistance = ( this.transform.position -end ).sqrMagnitude;

			yield return null;
		}
	}

	protected virtual void AttemptMove<T>(int xDir, int yDir)
		where T : Component 
	{
		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit);
		if( hit.transform == null )
			return;

		T hitComponent = hit.transform.GetComponent<T>();
		if( !canMove && hitComponent != null )
		{
			OnCantMove(hitComponent);
		}
	}

	protected abstract void OnCantMove<T>(T component)
		where T : Component;



}
