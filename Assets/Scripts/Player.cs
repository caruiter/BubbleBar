using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerID;


	private string inputPrefix;								// InputManager uses "P1Button1", "P1Horizontal", etc. 
    private float playerSpeed = 3;
    private Rigidbody2D rb;

	private void Awake()
	{
		inputPrefix = "P" + playerID;						// Set inputPrefix using correct playerID
	}

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 playerInput = new Vector3(
				Input.GetAxisRaw(inputPrefix + "Horizontal"),
				Input.GetAxisRaw(inputPrefix + "Vertical")
				);

			//transform.localPosition = Vector3.Lerp(transform.localPosition, playerInput.normalized * 0.5f, 10f * Time.deltaTime);
            //transform.localPosition = Vector3.Lerp(transform.localPosition, playerInput.normalized * 0.5f, 10f * Time.deltaTime);
			rb.velocity = playerInput * playerSpeed;
            //lineRenderer.SetPositions(new Vector3[] { joyTransform.position, joyContainerTransform.position });

			for(int i = 0; i < 3; i++)
			{
				if (Input.GetButtonDown(inputPrefix + "Button" + (i + 1)))
				{
					
				}
				else if (Input.GetButtonUp(inputPrefix + "Button" + (i + 1)))
				{
					
				}
			}
        
    }
}
