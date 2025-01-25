using UnityEngine;

namespace Game
{
	public class Player : MonoBehaviour
	{
		public int Score => score;
		public int PlayerID => playerID;

		[SerializeField] private int playerID = 1;       // 1 - 4 used to match InputManager keys

		private string inputPrefix = "P1";              // "P1" for example
		private int score;

		private void Awake()
		{
			inputPrefix = "P" + playerID;
			score = Random.Range(1, 10000);
		}

		private void Update()
		{
			Vector2 playerInput = new Vector3(
				Input.GetAxisRaw(inputPrefix + "Horizontal"),
				Input.GetAxisRaw(inputPrefix + "Vertical")
				);
		}
	}
}