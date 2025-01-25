using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Leaderboard class stores name/score/date data so that Arcade Launcher can access them.
	/// Leaderboard entries are cleared each semester.
	/// 
	/// To Use:
	///		- Leaderboard.AddScore(name, score); 
	///			+ Call this static method to write a name/score to the leaderboard data. It will create a new file if none exists.
	/// </summary>
	public static class Leaderboard
	{
		const string FILE_NAME = @"leaderboard.json";

		public static Action OnLeaderboardUpdated;

		/// <summary>
		/// Returns a List of LeaderboardEntries
		/// </summary>
		/// <returns></returns>
		public static List<LeaderboardEntry> GetLeaderboard()
		{
			string filepath = Application.dataPath + @"/" + FILE_NAME;
			if (!File.Exists(filepath))
			{
				Debug.Log("Leaderboard:: No saved leaderboard found. Creating new file at " + filepath);
				File.Create(filepath).Close();
			}

				LeaderboardList list = new LeaderboardList();
			string json = File.ReadAllText(filepath);
			if (!string.IsNullOrEmpty(json)) // No data
			{
				list = JsonUtility.FromJson<LeaderboardList>(json);
			}

			// Clear previous semester
			for(int i = 0; i < list.Leaderboards.Count; i++)
			{
				if (DateTime.TryParse(list.Leaderboards[i].Date, out DateTime date))
				{
					if (date.Year < DateTime.Now.Year || // Previous year
						(date.Month <= 6 && DateTime.Now.Month > 7) ||  // Saved in Spring, now Fall
						(date.Month >= 7 && DateTime.Now.Month < 7))	// Saved in Fall, now Spring
					{
						Debug.Log("Removing previous semester highscore: " + list.Leaderboards[i].ToString());
						list.Leaderboards.RemoveAt(i);
					}
				}
			}

			return list.Leaderboards;
		}
		
		/// <summary>
		/// Clears all entries from leaderboard. Saves backup before clearing.
		/// </summary>
		public static void DeleteLeaderboard()
		{
			string filepath = Application.dataPath + @"/" + FILE_NAME;

			if (!File.Exists(filepath)) // No leaderboard
			{
				Debug.LogWarning("Leaderboard:: No leaderboard file. Creating new leaderboard.");
				return;
			}
			File.Copy(filepath, Application.dataPath + @"/leaderboardBackup.json",true);
			LeaderboardList newList = new LeaderboardList();
			string json = JsonUtility.ToJson(newList);
			File.WriteAllText(filepath, json);
		}

		/// <summary>
		/// Add a new entry to the leaderboard list. Get the list with GetLeaderboard function.
		/// </summary>
		/// <param name="newName">Name for the entry.</param>
		/// <param name="newScore">Stored as double, overloads for int and float.</param>
		public static void AddScore(string newName, double newScore)
		{
			LeaderboardEntry entry = new LeaderboardEntry() 
			{ 
				Name = newName, 
				Score = newScore,
				Date = DateTime.Now.ToShortDateString()
			};

			List<LeaderboardEntry> entries = GetLeaderboard();
			entries.Add(entry);

			string filepath = Application.dataPath + @"/" + FILE_NAME;

			if (!File.Exists(filepath)) // No leaderboard
			{
				Debug.LogWarning("Leaderboard:: No leaderboard file!");
				return;
			}

			LeaderboardList newList = new LeaderboardList();
			newList.Leaderboards = entries;

			string json = JsonUtility.ToJson(newList);
			File.WriteAllText(filepath, json);
			OnLeaderboardUpdated?.Invoke();
		}

		public static void AddScore(string newName, float newScore)
		{
			AddScore(newName, double.Parse(newScore.ToString()));
		}

		public static void AddScore(string newName, int newScore)
		{
			AddScore(newName, double.Parse(newScore.ToString()));
		}

		public static void RemoveByName(string removeName)
		{
			List<LeaderboardEntry> entries = GetLeaderboard();
			LeaderboardEntry e = entries.Find(x => x.Name == removeName);
			if (e != null)
			{
				entries.Remove(e);
			}
			else
			{
				Debug.LogError($"{removeName} not found in leaderboard!");
				return;
			}

			string filepath = Application.dataPath + @"/" + FILE_NAME;

			LeaderboardList newList = new LeaderboardList();
			newList.Leaderboards = entries;

			string json = JsonUtility.ToJson(newList);
			File.WriteAllText(filepath, json);
			OnLeaderboardUpdated?.Invoke();
		}

		/// <summary>
		/// Find if a score is within a top number of spots.
		/// If spots 1-3 are 100, 200, 300: IsHighScore(250,3) will be true because 250 would be at spot 3, pushing out '300'
		/// </summary>
		/// <param name="score"></param>
		/// <param name="inTopN"></param>
		/// <returns></returns>
		public static bool IsHighScore(double score, int inTopN)
		{
			List<LeaderboardEntry> lb = GetLeaderboard();
			LeaderboardEntry e = new LeaderboardEntry() { Name = "", Score = score };
			lb.Add(e);
			lb = lb.OrderBy(x => x.Score).ToList();
			lb.Reverse();
			int i = lb.IndexOf(e)+1;
			//Debug.Log("Found " + score + " at " + i);
			return i <= inTopN;
		}

		/// <summary>
		/// Returns position of a score on the leaderboard.
		/// </summary>
		/// <param name="score"></param>
		/// <returns></returns>
		public static int PositionOnLeaderboard(double score)
		{
			List<LeaderboardEntry> lb = GetLeaderboard();
			LeaderboardEntry e = new LeaderboardEntry() { Name = "", Score = score };
			lb.Add(e);
			lb = lb.OrderBy(x => x.Score).ToList();
			lb.Reverse();
			return lb.IndexOf(e) + 1;
		}

		public static void PrintLeaderboard()
		{
			Debug.Log("Leaderboard - ");
			List<LeaderboardEntry> entries = GetLeaderboard();
			foreach(LeaderboardEntry entry in entries)
			{
				Debug.Log($" {entry.Date}- {entry.Name}: {entry.Score}");
			}
		}
	}

	[Serializable]
	public class LeaderboardEntry
	{
		public string Name;
		public double Score;
		public string Date;
	}
	
	[Serializable]
	public class LeaderboardList
	{
		public List<LeaderboardEntry> Leaderboards = new List<LeaderboardEntry>();
	}
}
