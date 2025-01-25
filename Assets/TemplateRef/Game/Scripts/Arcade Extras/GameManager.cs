using UnityEngine;

namespace Game
{
	public class GameManager : MonoBehaviour
	{
		public Player[] Players => players;

		[SerializeField] private Player[] players;
		[SerializeField] private LeaderboardManager leaderboardManager;

		private static GameManager instance;

		private void Awake()
		{
			if(instance != null) { Destroy(instance.gameObject); }
			instance = this;
		}

	}
}