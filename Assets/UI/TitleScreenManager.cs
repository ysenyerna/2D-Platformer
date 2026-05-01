using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

	SpriteRenderer fade;
	Timer fadeTime;

	void Start()
	{
		fade = GameObject.Find("Fade").GetComponent<SpriteRenderer>();
		fadeTime = fade.GetComponent<Timer>();
		fadeTime.Timeout += FadeTime_Timeout;

		// Tie button event
		GameObject.Find("StartButton").GetComponent<SpriteButton>().Pressed += StartButton_Pressed;
	}

	void Update()
	{
		if (fadeTime.Running)
		{
			float fadePercent = fadeTime.ElapsedTime / fadeTime.time;
			fade.color = new( fade.color.r, fade.color.g, fade.color.b, fadePercent);
		}

	}

	void StartButton_Pressed()
	{
		fadeTime.Run();
	}

	void FadeTime_Timeout()
	{
		SceneManager.LoadScene("World");
		fade.color = new( fade.color.r, fade.color.g, fade.color.b, 1f);
	}

}
