using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

	public float leftMostPos;
	public float rightMostPos;
	public bool vertical = false;

	public float moveTime = 1f;
	public float waitTime = 1f;


	// Use to move characters who are on the platform
	[HideInInspector]
	public Vector2 Velocity { get; private set; } = Vector2.zero;

	private Timer timer;
	private Rigidbody2D body;


	[SerializeField] private bool goingLeft = false;
	private bool moving;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		timer = gameObject.AddComponent<Timer>();
		timer.Timeout += Timer_Timeout;
		moving = false;
		timer.time = waitTime;
		timer.Run();
	}


	private void FixedUpdate()
	{
		var oldPos = body.position;
		if (moving)
		{
			var movePercent = timer.ElapsedTime / timer.time;
			if (goingLeft)
				movePercent = 1 - movePercent;
			var fullDistance = rightMostPos - leftMostPos;
			var currentDistance = movePercent * fullDistance;
			if (!vertical)
				body.position = new(leftMostPos + currentDistance, body.position.y);
			else
				body.position = new(body.position.x, leftMostPos + currentDistance );
		}

		Velocity = body.position - oldPos;
		if (Velocity.y < 0)
			Velocity = new(Velocity.x, Velocity.y - 0.14f);
		
	}


	private void Timer_Timeout()
	{
		if (moving)
		{
			goingLeft = !goingLeft;
			moving = false;
			timer.time = waitTime;
			timer.Run();

		}
		else
		{
			moving = true;
			timer.time = moveTime;
			timer.Run();
		}

	}





}
