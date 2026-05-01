using UnityEngine;

public class PlayerDustParticle : MonoBehaviour
{

	private void AnimationFinished()
	{
		Destroy(gameObject);
	}

}
