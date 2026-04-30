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

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.CompareTag("CameraZone"))
			return;

		if (collider is not BoxCollider2D box)
		{
			print($"Camera zone {collider.name} does not have a box collider.");
			return;
		}

		var pos = new Vector2(box.transform.position.x, box.transform.position.y) + box.offset;
		var halfWidth = box.size.x / 2;
		var halfHeight = box.size.y / 2;

		LimitLeft = pos.x - halfWidth;
		LimitRight = pos.x + halfWidth;
		LimitBottom = pos.y - halfHeight;
		LimitTop = pos.y + halfHeight;
		
	}

	private void UpdatePosition(float delta)
	{
		TargetPosition = player.transform.position;
		var actualTarget = TargetPosition;

		// Left and right limits
		var halfWidth = cam.orthographicSize * cam.aspect;
		actualTarget.x = Mathf.Clamp(actualTarget.x, LimitLeft + halfWidth, LimitRight - halfWidth);

		// Top and bottom limits
		var halfHeight = cam.orthographicSize;
		actualTarget.y = Mathf.Clamp(actualTarget.y, LimitBottom + halfHeight, LimitTop - halfHeight);

		// Apply smoothing
		var newPos = Vector2.Lerp(cam.transform.position, actualTarget, CameraSmoothing * delta);

		// Apply new position
		cam.transform.position = new Vector3(newPos.x, newPos.y, cam.transform.position.z);

	}


}
