using UnityEngine;

public class Checkpoint : MonoBehaviour
{

	// Sets the position where the player respawns when they die
	Animator anim;
	public Vector2 RespawnPosition { get; private set; }

	// Set the camera zone that this checkpoint is a part of
	public BoxCollider2D CameraZone;

	[HideInInspector]
	bool _active;
	public bool Active { get { return _active; } set
		{
			_active = value;
			anim.Play(value ? "CheckpointUnlocked" : "CheckpointLocked");

		}
	}	


	void Start()
	{
		anim = GetComponent<Animator>();

		RespawnPosition = transform.GetChild(0).transform.position;
	}



}
