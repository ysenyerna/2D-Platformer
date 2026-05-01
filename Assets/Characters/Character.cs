using UnityEngine;


// Base class for characters in who move and collide in a 2D space
// Must have a RigidBody2D and a BoxCollider2D hitbox
public abstract class Character : MonoBehaviour
{

	[HideInInspector]
	public Vector2 Velocity = new();


	private Rigidbody2D body;
	public BoxCollider2D hitbox { get; private set; }
	protected int TerrainLayerMask = -1;

	protected virtual void Start()
	{
		TerrainLayerMask = LayerMask.GetMask("Terrain");
		// Get components
		body = GetComponent<Rigidbody2D>();
		if (body == null) 
			Debug.LogWarning($"'{this}' does not have a RigidBody2D!");
		hitbox = GetComponent<BoxCollider2D>();
		if (hitbox == null) 
			Debug.LogWarning($"'{this}' does not have a BoxCollider2D hitbox!");

	}

	// METHODS

	// Returns whether the character is touching the floor
	public bool IsOnFloor()
		=> CheckTouchingGround(Vector2.down);

	// Returns whether the character is touching the ceiling
	public bool IsOnCeil() 
		=> CheckTouchingGround(Vector2.up);

	// Returns whether the character is touching a wall in the given direction (negative number for left, position number for right). Returns false if direction is 0
	public bool IsOnWall(float dir, LayerMask? mask = null)
	{
		if (dir == 0)
			return false;
		dir = Mathf.Sign(dir);
		return CheckTouchingGround(new(dir, 0), mask);
	}

	// Apply velocity
	public void Move()
	{
		body.linearVelocity = Velocity;
	
	}

	// Handle moving platforms
	void OnCollisionStay2D(Collision2D collider)
	{
		if (!collider.gameObject.TryGetComponent<MovingPlatform>(out var platform))
			return;

		body.position += platform.Velocity;
	}



	// PRIVATE METHODS

	// Checks whether the character is touching ground in a direction
	private bool CheckTouchingGround(Vector2 dir, LayerMask? mask = null)
	{
		mask ??= TerrainLayerMask;

		var hitboxPos = (Vector2)transform.position + hitbox.offset;
		var offset = hitbox.size / 2 * dir;

		Vector2 raySize;
		if (dir.x == 0)
			raySize = new(hitbox.size.x, 0.04f);
		else 
			raySize = new(0.04f, hitbox.size.y - 0.2f);

		var hits = Physics2D.BoxCastAll(hitboxPos + offset, raySize, 0, dir, 0.0f, (int)(LayerMask)mask);
		foreach (var hit in hits)
		{
			// Ignore self
			if (hit.collider == hitbox)
				continue;

			return true;
		}
		return false;
	}
		




}
