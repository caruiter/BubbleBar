using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Game
{
	public class GameManagerMod : MonoBehaviour
	{
		public PlayerScript[] Players => players;

		[SerializeField] public PlayerScript[] players;
		[SerializeField] private LeaderboardManagerMod leaderboardManager;

		[SerializeField] private List<CupScript> cups;

		[SerializeField] private List<RecipeScriptableObject> drinklist;

		private static GameManagerMod instance;

		private void Awake()
		{
			if(instance != null) { Destroy(instance.gameObject); }
			instance = this;
		}

		public RecipeScriptableObject GetNewRecipe(){ //returns 
			int index = Random.Range(0,drinklist.Count);
			return drinklist[index];
		}

	}
}