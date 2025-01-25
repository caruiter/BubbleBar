using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Subscribes to the Leaderboard class OnLeaderboardUpdated event to update the leaderboard text
	/// </summary>
	public class ShowHighScores : MonoBehaviour
	{
		[SerializeField] private TMP_Text highScoreText;

		private void OnEnable()
		{
			Leaderboard.OnLeaderboardUpdated += HandleLBUpdate;
		}
		private void OnDisable()
		{
			Leaderboard.OnLeaderboardUpdated -= HandleLBUpdate;
		}

		private void Start()
		{
			HandleLBUpdate();
		}

		private void HandleLBUpdate()
		{
			List<LeaderboardEntry> entries = Leaderboard.GetLeaderboard();
			entries = entries.OrderBy(x => x.Score).ToList();
			entries.Reverse();
			highScoreText.text = "High Scores\n";
			for(int i = 0; i < Math.Min( entries.Count, 5); i++)
			{
				highScoreText.text += $"{i+1}     {entries[i].Name}     {entries[i].Score}\n";
			}
		}

	}
}