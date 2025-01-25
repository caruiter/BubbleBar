using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Example class utilizing input for the Quinnipiac Arcade Machine.
	/// Each Joystick and 3 Button set is mapped to work with a player id between 1 - 4
	/// Example: "P1Horizontal" will get the left/right value of the first joystick.
	///				- Detect with 'Input.GetAxis(inputPrefix + "Horizontal")'
	///			 "P3Button3" will get the button state of player 3 button 3.
	///				- Detect with 'Input.GetButtonDown(inputPrefix + "Button3")'
	/// </summary>
	public class PlayerInputVisualizer : MonoBehaviour
	{
		[SerializeField] private int playerID = 1;				// 1 - 4, used to match input names in InputManager
		[SerializeField] private Transform joyTransform;
		[SerializeField] private Transform joyContainerTransform;
		[SerializeField] private LineRenderer lineRenderer;
		[SerializeField] private SpriteRenderer joySprRend;
		[SerializeField] private SpriteRenderer[] buttons;
		[SerializeField] private Color col;

		private float buttonUpOffset = 0.2f;
		private string inputPrefix;								// InputManager uses "P1Button1", "P1Horizontal", etc. 

		private void Awake()
		{
			inputPrefix = "P" + playerID;						// Set inputPrefix using correct playerID
		}

		private void Start()
		{
			joySprRend.color = col;
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].color = col * (0.5f + (i * 0.05f));
			}
		}

		private void Update()
		{
			Vector2 playerInput = new Vector3(
				Input.GetAxisRaw(inputPrefix + "Horizontal"),
				Input.GetAxisRaw(inputPrefix + "Vertical")
				);

			joyTransform.localPosition = Vector3.Lerp(joyTransform.localPosition, playerInput.normalized * 0.5f, 10f * Time.deltaTime);
			lineRenderer.SetPositions(new Vector3[] { joyTransform.position, joyContainerTransform.position });

			for(int i = 0; i < buttons.Length; i++)
			{
				if (Input.GetButtonDown(inputPrefix + "Button" + (i + 1)))
				{
					buttons[i].transform.localPosition = Vector3.zero;
					buttons[i].DOKill();
					buttons[i].color = Color.yellow;
					buttons[i].DOColor(Color.gray, 0.25f);
				}
				else if (Input.GetButtonUp(inputPrefix + "Button" + (i + 1)))
				{
					buttons[i].transform.localPosition = new Vector3(0, buttonUpOffset);
					buttons[i].DOKill();
					buttons[i].color = col * (0.5f + (i * 0.05f));
				}
			}
		}
	}
}