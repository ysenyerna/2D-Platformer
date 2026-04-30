using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{

	// MOVEMENT VALUES
	[Header("Movement Values")]
	[SerializeField] float MaxSpeed = 8f;
	[SerializeField] float AccelerationSpeed = 1f;
	[SerializeField] float DecelerationSpeed = 1.5f;
	[SerializeField] float Gravity = -0.6f;
	[SerializeField] float JumpVelocity = 12f;
	[SerializeField] float ReleasedJumpGravityMultiplier = 0.7f;

	// Components
	Timer coyoteTime;
	Timer jumpBuffer;
	Animator anim;
	SpriteRenderer sprite;

	// Input
	InputActionMap input; 

	protected override void Start()
	{
		base.Start();

		// Get player input action map
		input = InputSystem.actions.actionMaps.First(m => m.name == "Player");

		// Get components
		coyoteTime = GetComponents<Timer>().First(t => t.id == "CoyoteTime");
		jumpBuffer = GetComponents<Timer>().First(t => t.id == "JumpBuffer");
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();

	}

	private void Update()
	{
		HandleHorizontalMovement();
		HandleVerticalMovement();
		HandleAnimation();
		Move();
	}


	private void HandleHorizontalMovement()
	{
		var delta = Time.deltaTime * 60;

		// Get move direction
		var dir = 0f;
		if (input["MoveLeft"].IsPressed()) dir -= 1f;
		if (input["MoveRight"].IsPressed()) dir += 1f;

		// Set speed to either acceleration or deceleration
		var speed = (System.Math.Sign(dir) != System.Math.Sign(Velocity.x)) ? DecelerationSpeed : AccelerationSpeed;
		
		Velocity.x = Mathf.MoveTowards(Velocity.x, MaxSpeed * dir, speed * delta);

		if (dir != 0 && IsOnWall(dir))
		{
			Velocity.x = 0f;
		}
	}

	bool onFloor = false;
	private void HandleVerticalMovement()
	{
		var delta = Time.deltaTime * 60;

		// Get buffered jump input
		if (input["Jump"].WasPressedThisFrame())
			jumpBuffer.Run();

		// Handle coyote time timer
		if (IsOnFloor())
			onFloor = true;
		else {
			if (onFloor && Velocity.y <= 0)
				coyoteTime.Run();
			onFloor = false;
		}

		// Stop falling when on floor
		if (onFloor && Velocity.y < 0)
			Velocity.y = 0;

		// Jump
		if (jumpBuffer.Running && (onFloor || coyoteTime.Running))
		{
			jumpBuffer.Stop();
			coyoteTime.Stop();
			Velocity.y = JumpVelocity;
		}

		// Fall
		if (!onFloor)
		{
			Velocity.y += Gravity * delta;

			if (IsOnCeil() && Velocity.y > 0)
				Velocity.y = 0f;

			// Fall faster when not holding jump
			if (Velocity.y > 0 && !input["Jump"].IsPressed())
			{
				float diff = Velocity.y - Velocity.y * ReleasedJumpGravityMultiplier;
				Velocity.y -= diff * delta;
			}
		}
	}

	private void HandleAnimation()
	{
		// Flip
		if (Velocity.x != 0)
			sprite.flipX = Velocity.x < 0;

		// Animations
		if (onFloor && Velocity.x == 0)
			anim.Play("PlayerIdle");
		else if (onFloor) 
			anim.Play("PlayerRun");
		else if (Velocity.y > 0)
			anim.Play("PlayerJump");
		else 
			anim.Play("PlayerFall");
		
	}

}
