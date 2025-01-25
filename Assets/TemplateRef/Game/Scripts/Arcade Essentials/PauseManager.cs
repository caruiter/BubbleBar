using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	/// <summary>
	/// PauseManager listens for the START button on the arcade machine.
	/// When active, Time.timeScale is set to 0. This affects anything that is multiplied by Time.deltaTime, effectively pausing it.
	/// Use QuitGame function to quit from the pause menu, or TogglePause(false) to unpause the game.
	/// </summary>
	public class PauseManager : MonoBehaviour
	{
		[SerializeField] private GameObject pauseScreen;
		[SerializeField] private Button[] buttons;
		private bool gamePaused = false;
		private Button selectedButton;
		private int selectedIndex;
		private float previousGamespeed = 1;
		private float moveDelay = 0f;

		private void Awake()
		{
			TogglePause(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space)) // Start button on arcade machine
			{
				TogglePause(!gamePaused);
			}

			if (moveDelay > 0f)
			{
				moveDelay -= Time.unscaledDeltaTime;
				return;
			}
			float move = 0; //Move up/down the menu
			for (int i = 1; i <= 4; i++)
			{
				move = Input.GetAxisRaw($"P{i}Vertical");
				if (move != 0)
				{
					selectedIndex -= (int)Mathf.Sign(move);
					if (selectedIndex < 0) selectedIndex = buttons.Length - 1;
					else if (selectedIndex > buttons.Length - 1) selectedIndex = 0;
					buttons[selectedIndex].Select();
					moveDelay = 0.2f;
				}
			}
		}

		public void TogglePause(bool isPaused)
		{
			gamePaused = isPaused;
			pauseScreen.SetActive(isPaused);
			Debug.Log("Game paused: " + isPaused);
			if (gamePaused )
			{
				previousGamespeed = Time.timeScale;
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = previousGamespeed;
			}
		}

		public void QuitGame()
		{
			Debug.Log("Quitting game.");
			Application.Quit();
		}
	}
}