using UnityEngine;
using System.Collections;


namespace Lockstep
{
	//TODO: Set up default functions to implement LSManager
	public static class LockstepManager
	{
		private static GameManager _mainGameManager;

		public static GameManager MainGameManager {
			get {
				if (_mainGameManager == null)
					throw new System.Exception("MainGameManager has exploded!");
				return _mainGameManager;
			}
			private set {
				_mainGameManager = value;
			}
		}

		internal static void Initialize(GameManager gameManager)
		{
			MainGameManager = gameManager;
			GridManager.Initialize();
		}
	}
}

