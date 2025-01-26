using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    public List<String> Contents;
    [SerializeField] private int playerID;
    [SerializeField] private PlayerScript playermatch;
    [SerializeField] private GameManagerMod gm;

    public RecipeScriptableObject recipe;
    private string inputPrefix;	// InputManager uses "P1Button1", "P1Horizontal", etc. 

    private bool controllable;
    private bool shaking;

    [SerializeField] private int shakeCount;
    [SerializeField] private int shakeTarget; //change this in inspector to test


    void Awake(){
        inputPrefix = "P" + playerID; // Set inputPrefix using correct playerID
        controllable = true;
        shaking = false;

        recipe = gm.GetNewRecipe();
    }

    // Start is called before the first frame update    
    void Start()
    {
        Contents = new List<String>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controllable && !shaking){ // if allowing control and not being shaken
         if (Input.GetButtonDown(inputPrefix + "Button" + (3))) //discard contents of cup
         {
            Debug.Log("discarded contents of cup " + playerID);
            Contents = new List<string>();
         }
        }

        if(shaking){ //allow button mashing if shaking
            ShakeDrink();
        } else{
                //If there are 3 ingredients, player must now shake the drink
                 if(Contents.Count==3)
                    {
                    Debug.Log("cup full");
                    shaking = true;
                    //ShakeDrink();
                    }
        }
    }

    public void AddIngredient(string ingr, int playerNum){ //called by IngredientScript class
    Debug.Log(playerNum + " | " + playerID);
        if(Contents.Count<3 && playerNum == playerID){ //check that no more than 3 drinks have been added
            //Debug.Log("true?");
            //if(Contents.Contains(ingr) ==false){ //check that ingredient isn't already in there
                Contents.Add(ingr);
                Debug.Log("cup " + playerID +" added " + ingr);
                //}
        }


    }

    public void SetControllable(bool con){ //should buttons effect the cup
        controllable = con;
    }

    public void ShakeDrink()
    {
        //Put code for turning the emptying cup function off here
        
        if(Input.GetButtonDown(inputPrefix + "Button" + (1)) || Input.GetButtonDown(inputPrefix + "Button" + (2)) || Input.GetButtonDown(inputPrefix + "Button" + (3)))
        { 
            //number of shakes goes up each time the player presses an input
            Debug.Log("shakes: " + shakeCount);
            shakeCount++;
        }

        if(shakeCount >= shakeTarget){ //Drink is finished shaking

            Debug.Log("Drink done!");
            shaking = false;

            //ANIM?

            //check if drink is correct
            bool correct = true;
            foreach(string ingr in recipe.ingredients){
                if(!Contents.Contains(ingr)){
                    correct =false;
                }
            }

            if(correct){ // if correct up points and get new recipe
                recipe = gm.GetNewRecipe();
                playermatch.Score++;
            }

            Contents = new List<String>(); //clear contents and shake count
            shakeCount = 0;
        }
    }
}
