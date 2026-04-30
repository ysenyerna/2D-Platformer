using UnityEngine;

public class ParallaxLayer : MonoBehaviour 
{


	public Vector2 scrollSpeed = new(1, 1);



	[HideInInspector]
	public GameObject mid;

	[HideInInspector]
	public float length;

	[HideInInspector]
	public Vector2 offset;


}
