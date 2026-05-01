using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

	public float FadingTime = 0.5f;
	public float WaitTimeBetweenFade = 0.1f;

	Image fade;
	Timer fadeTime;

	// Called when the fade is finished and the screen is back to normal
	public Action FadeCleared;
	// Called when the screen is black during a fade
	public Action FadeBlack;
	

	enum FadeState { FadingOut, FadingIn, Black, NotFading }
	FadeState fadeState = FadeState.NotFading;


	private void Start()
	{
		fade = GetComponentInChildren<Image>();
		fadeTime = GetComponentInChildren<Timer>();
		fadeTime.Timeout += FadeTime_Timeout;

		// Set initial fade
		fade.color = new(fade.color.r, fade.color.g, fade.color.b, 1f);
		fadeState = FadeState.Black;
		fadeTime.time = WaitTimeBetweenFade;
		fadeTime.Run();
	}





	private void Update()
	{
		HandleFade();
	}



	private void HandleFade()
	{
		float fadePercent;
		switch (fadeState)
		{
			case FadeState.FadingOut:
				fadePercent = fadeTime.ElapsedTime / FadingTime;
				fade.color = new(fade.color.r, fade.color.g, fade.color.b, fadePercent);
				break;


			case FadeState.FadingIn:
				fadePercent = fadeTime.ElapsedTime / FadingTime;
				fade.color = new(fade.color.r, fade.color.g, fade.color.b, 1f - fadePercent);
				break;

			case FadeState.Black:
				fade.color = new(fade.color.r, fade.color.g, fade.color.b, 1);
				break;

			case FadeState.NotFading:
				fade.color = new(fade.color.r, fade.color.g, fade.color.b, 0);
				break;
		}


	}


	private void FadeTime_Timeout()
	{
		switch (fadeState)
		{
			case FadeState.FadingOut:
				fadeState = FadeState.Black;
				fadeTime.time = WaitTimeBetweenFade;
				fadeTime.Run();
				FadeBlack?.Invoke();
				break;

			case FadeState.Black:
				fadeState = FadeState.FadingIn;
				fadeTime.time = FadingTime;
				fadeTime.Run();
				break;

			case FadeState.FadingIn:
				fadeState = FadeState.NotFading;
				FadeCleared?.Invoke();
				break;
		}
	}




	// PUBLIC METHODS
	public void StartFade()
	{
		fadeState = FadeState.FadingOut;
		fadeTime.time = FadingTime;
		fadeTime.Run();
	}


}
