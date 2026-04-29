using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

	public Vector2 TargetPosition = Vector2.zero;
	public float LimitLeft = float.MinValue;
	public float LimitRight = float.MaxValue;
	public float LimitTop = float.MaxValue;
	public float LimitBottom  = float.MinValue;
	public float CameraSmoothing = 10f;
	public bool SyncToFixedUpdate = false;

	Camera cam;
	GameObject player;
	
	


	private void Start()
	{
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		player = GameObject.FindWithTag("Player");
	}


	private void Update()
	{
		if (!SyncToFixedUpdate)
			UpdatePosition(Time.deltaTime);
	}

	private void FixedUpdate()
	{
		if (SyncToFixedUpdate)
			UpdatePosition(Time.fixedDeltaTime);
	}

	private void UpdatePosition(float delta)
	{
		TargetPosition = player.transform.position;
		var actualTarget = TargetPosition;


		// Handle limits
		var halfHeight = cam.orthographicSize;
		var halfWidth = cam.orthographicSize * cam.aspect;
		
		// Left limit
		if (TargetPosition.x - halfWidth < LimitLeft)
			actualTarget.x = LimitLeft + halfWidth;
		// Right limit
		if (TargetPosition.x + halfWidth > LimitRight)
			actualTarget.x = LimitRight - halfWidth;
		// Top limit
		if (TargetPosition.y + halfHeight > LimitTop)
			actualTarget.y = LimitTop - halfHeight;
		// Bottom limit
		if (TargetPosition.y - halfHeight < LimitBottom)
			actualTarget.y = LimitBottom + halfHeight;

		// Apply smoothing
		var newPos = Vector2.Lerp(cam.transform.position, actualTarget, CameraSmoothing * delta);

		// Apply new position
		cam.transform.position = new Vector3(newPos.x, newPos.y, cam.transform.position.z);

	}


}
