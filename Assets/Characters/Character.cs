using UnityEngine;


// Base class for characters in who move and collide in a 2D space
// Must have a RigidBody2D and a BoxCollider2D hitbox
public abstract class Character : MonoBehaviour
{

	[HideInInspector]
	public Vector2 Velocity = new();


	private Rigidbody2D body;
	private BoxCollider2D hitbox;
	private int TerrainLayerMask = -1;
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
	public bool IsOnWall(float dir)
	{
		if (dir == 0)
			return false;
		dir = Mathf.Sign(dir);
		return CheckTouchingGround(new(dir, 0));
	}

	// Apply velocity
	public void Move()
	{
		body.linearVelocity = Velocity;
	}




	// PRIVATE METHODS

	// Checks whether the character is touching ground in a direction
	private bool CheckTouchingGround(Vector2 dir)
		=> Physics2D.BoxCast((Vector2)transform.position + hitbox.offset, hitbox.size, 0, dir, 0.04f, TerrainLayerMask);



}
