

using System;
using UnityEngine.SceneManagement;

public static class GlobalData
{
	
	private static uint _coinCount;
	public static uint CoinCount { get { return _coinCount; } private set
		{
			_coinCount = value;
			CoinCountUpdated?.Invoke();
		} }


	public static Action CoinCountUpdated;


	public static void CollectCoin(uint amount = 1)
	{
		CoinCount += 1;
	}

	public static void ResetCoinCount()
	{
		CoinCount = 0;
	}

	public static void LoadScene(string sceneName)
	{
		CoinCountUpdated = null;

		if (sceneName == "World")
			ResetCoinCount();

		SceneManager.LoadScene(sceneName);
	}

}