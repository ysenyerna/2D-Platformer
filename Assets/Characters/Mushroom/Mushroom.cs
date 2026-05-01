using UnityEngine;

public class Mushroom : Character
{

	public float dir = 1f;
	private const float Speed = 2f;

	private SpriteRenderer sprite;

	private Animator anim;
	private bool moving = true;
	public BoxCollider2D damageBox { get; private set; }


	protected override void Start()
	{
		base.Start();

		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		damageBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		// Set sprite direction
		sprite.flipX = dir < 0;
	}

	private void FixedUpdate()
	{
		var onFloor = IsOnFloor();

		if (moving)
		{
			// Turn around when reaching an edge or when hitting a wall or enemy
			if (onFloor)
			{
				var ray = Physics2D.Raycast(new(transform.position.x + dir * 0.3f, transform.position.y), Vector2.down, 0.55f, TerrainLayerMask);
				var onWall = IsOnWall(dir, TerrainLayerMask | LayerMask.GetMask("Enemy"));
				if (onWall || !ray)
					dir = -dir;
			}
		
			// Move
			Velocity.x = dir * Speed;
		}
		

		// Apply gravity
		if (onFloor)
		{
			Velocity.y = 0;
		}
		else
		{
			Velocity.y -= 0.5f;
		}

		Move();

	}

	public void Die()
	{
		anim.Play("MushroomCrush");
		moving = false;
		damageBox.enabled = false;
		Velocity.x = 0;
	}

	private void CrushFinished()
	{
		anim.Play("MushroomWalk");
		moving = true;
		damageBox.enabled = true;
	}

}