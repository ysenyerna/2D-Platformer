using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{

	[SerializeField] private Sprite[] digits;

	private void Start()
	{
		GameObject.Find("RestartButton").GetComponent<SpriteButton>().Pressed += RestartButton_Pressed;
		GameObject.Find("TitleButton").GetComponent<SpriteButton>().Pressed += TitleButton_Pressed;
		SetCoinCount();
	}

	private void RestartButton_Pressed()
	{
		GlobalData.LoadScene("World");
	}


	private void TitleButton_Pressed()
	{
		GlobalData.LoadScene("Title");
	}

	private void SetCoinCount()
	{
		var coinCount = GlobalData.CoinCount;
		coinCount = Math.Min(coinCount, 99);
		

		int digit1 = (coinCount == 0) ? 0 : (int)Math.Floor(coinCount / 10f);
		int digit2 = (coinCount == 0) ? 0 : (int)Math.Floor(coinCount % 10f);

		GameObject.Find("CoinCount/digit1").GetComponent<SpriteRenderer>().sprite = digits[digit1];
		GameObject.Find("CoinCount/digit2").GetComponent<SpriteRenderer>().sprite = digits[digit2];
	}

}
