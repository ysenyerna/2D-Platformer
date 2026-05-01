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

	// GameObjects
	Checkpoint currentCheckpoint; 

	LevelUI ui;


	// Components
	Timer coyoteTime;
	Timer jumpBuffer;
	Animator anim;
	SpriteRenderer sprite;

	// Input
	InputActionMap input; 

	// States
	enum State { Playing, Dying, Respawning }
	State state = State.Playing;

	protected override void Start()
	{
		base.Start();

		// Get player input action map
		input = InputSystem.actions.actionMaps.First(m => m.name == "Player");

		// Get Objects
		ui = GameObject.FindWithTag("UI").GetComponent<LevelUI>();
		ui.FadeBlack += GoToRespawnPosition;
		ui.FadeBlack += GetComponent<PlayerCamera>().UpdatePositionNoSmoothing;
		ui.FadeCleared += StartRespawn;


		// Get components
		coyoteTime = GetComponents<Timer>().First(t => t.id == "CoyoteTime");
		jumpBuffer = GetComponents<Timer>().First(t => t.id == "JumpBuffer");
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();

	}

	private void Update()
	{
		if (state == State.Playing)
		{
			HandleHorizontalMovement();
			HandleVerticalMovement();
			
		}
		else
		{
			Velocity = Vector2.zero;
		}

		HandleAnimation();
		Move();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		// Handle checkpoints
		if (collider.gameObject.TryGetComponent<Checkpoint>(out var checkpoint))
		{
			if (currentCheckpoint is not null)
			{
				if (checkpoint == currentCheckpoint)
					return;
				currentCheckpoint.Active = false;
			}

			currentCheckpoint = checkpoint;
			currentCheckpoint.Active = true;
		}

		// Handle death
		else if (collider.CompareTag("Death"))
		{
			Die();
		}
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
		if (state == State.Playing)
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
		else if (state == State.Dying)
		{
			anim.Play("PlayerDeath");
		}
		else if (state == State.Respawning)
		{
			anim.Play("PlayerFall");
		}
		
	}

	private void Die()
	{
		state = State.Dying;

		ui.StartFade();
	}

	private void GoToRespawnPosition()
	{
		GetComponent<PlayerCamera>().UpdateCameraLimitsToCameraZone(currentCheckpoint.CameraZone);
		transform.position = new (currentCheckpoint.RespawnPosition.x, currentCheckpoint.RespawnPosition.y, transform.position.z);
		state = State.Respawning;
	}

	private void StartRespawn()
	{
		state = State.Playing;
	}


}
