using UnityEngine;

namespace Game
{
	public class GameManagerMod : MonoBehaviour
	{
		public PlayerScript[] Players => players;

		[SerializeField] private PlayerScript[] players;
		[SerializeField] private LeaderboardManagerMod leaderboardManager;

		private static GameManagerMod instance;

		private void Awake()
		{
			if(instance != null) { Destroy(instance.gameObject); }
			instance = this;
		}

	}
}