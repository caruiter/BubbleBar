using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using TMPro;
using System.Collections;
//using Unity.VisualScripting.Dependencies.Sqlite;

namespace Game
{
	/// <summary>
	/// Class to handle getting username for high scores. 
	/// Calling EnterHighScores should show the leaderboard screen and allow players to enter their names.
	/// 
	/// Requires a GameManager class with an array of Players. 
	/// Players must have public int "Score"
	/// 
	/// Tweening using Dotween is commented out, add Dotween from Package Manager and uncomment to enable.
	/// </summary>
	public class LeaderboardManagerMod : MonoBehaviour
	{
		const string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.!$?_";

		[SerializeField] private GameManagerMod gameManager;
		[SerializeField] private GameObject container;      // Toggle on/off when active
		[SerializeField] private Transform box;
		[SerializeField] private TMP_Text[] letterTextArr;	// array of tmptext for each letter of name entry
		[SerializeField] private Image[] upArrowArr;		
		[SerializeField] private Image[] downArrowArr;
		[SerializeField] private Color colArrowsIdle;
		[SerializeField] private Color colArrowsHit;
		[SerializeField] private TMP_Text headerText;

		[SerializeField] private TMP_Text topText;
		private int activePlayerId = 1;							// Player currently entering score
		private int activeSlotIndex = 0;					// Letter slot 1/2/3
		private int[] activeLetter = new int[3];           // The index of the letter in slot 1/2/3
		private Vector2 input;
		private Vector2 prevInput;
		private bool waitingForScoreEntry;
		private string newEntry;

		private void Start()
		{
			letterTextArr[0].color = Color.red;
			letterTextArr[1].color = Color.black;
			letterTextArr[2].color = Color.black;
			container.SetActive(false);
		}

		private void Update()
		{
			if (!waitingForScoreEntry)
				return;

			input = new Vector2(
				Input.GetAxisRaw($"P{activePlayerId}Horizontal"),
				Input.GetAxisRaw($"P{activePlayerId}Vertical"));
			Horizontal();
			Vertical();

			// Submit name
			if (Input.GetButtonDown($"P{activePlayerId}Button1"))
			{
				string s = "";
				for(int i = 0; i < letterTextArr.Length; i++)
				{
					s += chars[activeLetter[i]]	;
				}

				Debug.Log("added char:" + s);
				AddEntry(s);
			}
			// Cancel
			else if (Input.GetButtonDown($"P{activePlayerId}Button2"))
			{
				Debug.Log("cancelled");
				AddEntry(null);
			}
		}

		public void EnterHighScores()
		{
			StartCoroutine(HighScoreCoroutine());
		}

		private IEnumerator HighScoreCoroutine()
		{
			Debug.Log("yeah?");
			// Check scores for leaderboard entry
			for (int i = 0; i < gameManager.Players.Length; i++)    // Look at each player
			{
				//Debug.Log("here");
				gameManager.Players[i].Immobilize(false); //keep players from moving cursors or cups
				Debug.Log("check");
				Leaderboard.GetLeaderboard();
				//Leaderboard.AddScore("EXA",0);
				if (gameManager.Players[i].Score > 0 && Leaderboard.IsHighScore(gameManager.Players[i].Score, 10)) // Check that the player earned points and their score is within the top 10 high scores.
				{
					Debug.Log("a");
					container.gameObject.SetActive(true);
					//box.transform.DOPunchScale(Vector3.one, 0.5f);
					activeSlotIndex = 0;
					for (int j = 0; j < activeLetter.Length; j++)
					{
						activeLetter[j] = 0;
						letterTextArr[j].text = chars[activeLetter[j]].ToString();
					}
					activePlayerId = i + 1;
					topText.text = $"High Score!\n<size=+20><color=yellow>Player {activePlayerId}</color></size>";
					headerText.text = $"High Score - Rank: {Leaderboard.PositionOnLeaderboard(gameManager.Players[i].Score)}\n<size=+20><color=white>Player {activePlayerId}</color></size>\n<size=+30><color=yellow>{gameManager.Players[i].Score}</color></size>";
					waitingForScoreEntry = true;
					while (waitingForScoreEntry) yield return null;
					if (newEntry != null)
					{
						Leaderboard.AddScore(newEntry, gameManager.Players[i].Score);
						waitingForScoreEntry = true; // reset
						newEntry = null;
					}
				} else{
					Debug.Log(gameManager.Players[i].Score);
				}
			}
			container.gameObject.SetActive(false);
			Debug.Log("Quit Game");
			Application.Quit();
		}

		public void AddEntry(string entry)
		{
			newEntry = entry;
			waitingForScoreEntry = false;
		}

		private void Horizontal()
		{

			if (Mathf.Abs(input.x) > 0.1f) // Holding horizontal
			{
				if (Mathf.Abs(prevInput.x) > 0.1f && Mathf.Sign(input.x) == Mathf.Sign(prevInput.x))
				{
					return;
				}
			}

			prevInput.x = input.x;

			// Change active slot
			if (Mathf.Abs(input.x) > 0.01f)
			{
				letterTextArr[activeSlotIndex].color = Color.black;
				activeSlotIndex += (int)Mathf.Sign(input.x);
				if (activeSlotIndex < 0) activeSlotIndex = 2;
				else if (activeSlotIndex > 2) activeSlotIndex = 0;
				letterTextArr[activeSlotIndex].color = Color.red;
			}
		}

		private void Vertical()
		{
			if (Mathf.Abs(input.y) > 0.1f) // Holding horizontal
			{
				if (Mathf.Abs(prevInput.y) > 0.1f && Mathf.Sign(input.y) == Mathf.Sign(prevInput.y))
				{
					Debug.Log("ignore vert");
					return;
				}
			}

			prevInput.y = input.y;

			// Change active char
			if (Mathf.Abs(input.y) > 0.01f)
			{
				int next = activeLetter[activeSlotIndex] + (int)Mathf.Sign(input.y);
				if (next < 0) next = chars.Length-1;
				else if(next >= chars.Length) next = 0;
				activeLetter[activeSlotIndex] = next;
				letterTextArr[activeSlotIndex].text = chars[activeLetter[activeSlotIndex]].ToString();
				if(Mathf.Sign(input.y) > 0)
				{
					//upArrowArr[activeSlotIndex].DOKill();
					Debug.Log("up one");

					foreach (Image i in upArrowArr){ // reset colors
						i.color = colArrowsIdle;
					} foreach(Image b in downArrowArr){
						b.color = colArrowsIdle;
					}

					upArrowArr[activeSlotIndex].color = colArrowsHit;
					//upArrowArr[activeSlotIndex].DOColor(colArrowsIdle, 0.5f);
				}
				else
				{
					Debug.Log("down one");

					foreach (Image i in upArrowArr){ // reset colors
						i.color = colArrowsIdle;
					} foreach(Image b in downArrowArr){
						b.color = colArrowsIdle;
					}
					//downArrowArr[activeSlotIndex].DOKill();
					downArrowArr[activeSlotIndex].color = colArrowsHit;
					//downArrowArr[activeSlotIndex].DOColor(colArrowsIdle, 0.5f);
				}
			}
		}
	}
}