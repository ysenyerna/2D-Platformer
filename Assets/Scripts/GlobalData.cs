

using System;

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

	public static void ResetCointCount()
	{
		CoinCount = 0;
	}

}