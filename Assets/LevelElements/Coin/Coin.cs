using UnityEngine;

public class Coin : MonoBehaviour
{

	[HideInInspector]
	public bool collected = false;
	
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void Pickup()
	{
		anim.Play("CoinPickup");
	}

	void AnimationFinished()
	{
		Destroy(gameObject);
	}


}
