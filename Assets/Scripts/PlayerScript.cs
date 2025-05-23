using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int playerID;
    [SerializeField] private CupScript matchedCup;


	private string inputPrefix;	// InputManager uses "P1Button1", "P1Horizontal", etc. 
    private float playerSpeed = 5; //how fast does the player move?
    public bool carrying; //is the player carrying an ingredient?
    public IngredientScript carried;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    public int Score;
    private bool moveEnabled;

    [SerializeField] private List<Sprite> handSprites;
    [SerializeField] private TextMeshProUGUI playerScoreText;


	private void Awake()
	{
		inputPrefix = "P" + playerID; // Set inputPrefix using correct playerID
        carrying = false;
        carried = null;
        Score = 0;
        moveEnabled = true;
        rb= GetComponent<Rigidbody2D>();
        spr= GetComponent<SpriteRenderer>();
	}

    // Start is called before the first frame update
    void Start()
    {
        spr.sprite = handSprites[0]; //Sets player sprite as open hand to start 
    }

    // Update is called once per frame
    void Update()
    {
        if(moveEnabled){
        Vector2 playerInput = new Vector2( //find direction player is moving in
				Input.GetAxisRaw(inputPrefix + "Horizontal"),
				Input.GetAxisRaw(inputPrefix + "Vertical")
				);

			//transform.localPosition = Vector3.Lerp(transform.localPosition, playerInput.normalized * 0.5f, 10f * Time.deltaTime);
            //transform.localPosition = Vector3.Lerp(transform.localPosition, playerInput.normalized * 0.5f, 10f * Time.deltaTime);

			rb.velocity = playerInput * playerSpeed; //move player


            if (Input.GetButtonDown(inputPrefix + "Button" + (1))) //ingredient interaction button
			    {
                    if(!carrying) //attempt to pick up ingredient
                        {
                        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
                        Collider2D[] otherColliders = Physics2D.OverlapAreaAll(myCollider.bounds.min, myCollider.bounds.max);

                        // Check for any colliders that are on top

                         foreach (var otherCollider in otherColliders)
                            {
                                //Debug.Log(otherCollider.transform.name);
                                if (otherCollider.gameObject.CompareTag("Ingredient"))
                                    {
                                       // Debug.Log(otherCollider.transform.name + "!");
                                        otherCollider.GetComponent<IngredientScript>().PickUp(this);
                                        break;
                                    }
                            }
                        } else{ //set down what is being carried | UNCOMMENT THIS SECTION TO MAKE CARRYING EASIER
                            /**carried.SetDown();**/
                        }
                } else if (Input.GetButtonUp(inputPrefix + "Button" + (1)) && carrying){ //COMMENT OUT THIS SECTION TO MAKE CARRYING HARDER
                        //put down item when button is released
                        carried.SetDown();
                }
            

        }
        
    }

    public void GrabItem(IngredientScript ingr){ //accessed by Ingredient class
        carrying = true;
        carried = ingr;
        spr.sprite = handSprites[1]; //Switches to closed hand sprite when player successfully grabs ingredient
    }
    public void ReleaseItem(){ //accessed by Ingredient class
        carrying = false;
        carried = null;
        spr.sprite = handSprites[0]; //Switches back to open hand
    }

    public int GetPlayerID(){
        return playerID;
    }

    public void Immobilize(bool set){
        if(rb.velocity.magnitude!=0){
            rb.velocity = new Vector2(0,0);
        }
        moveEnabled = set;
        matchedCup.SetControllable(set);
    }

    public void IncreaseScore(){
        Score++;
        playerScoreText.text = ""+Score;
    }
}
