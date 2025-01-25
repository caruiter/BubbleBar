using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    public List<String> Contents;
    [SerializeField] private int playerID;
    private string inputPrefix;	// InputManager uses "P1Button1", "P1Horizontal", etc. 


    void Awake(){
        inputPrefix = "P" + playerID; // Set inputPrefix using correct playerID
    }

    // Start is called before the first frame update    
    void Start()
    {
        Contents = new List<String>();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetButtonDown(inputPrefix + "Button" + (3))) //discard contents of cup
         {
            Debug.Log("discarded contents of cup " + playerID);
            Contents = new List<string>();
         }
    }

    public void AddIngredient(string ingr, int playerNum){ //called by IngredientScript class
    Debug.Log(playerNum + " | " + playerID);
        if(Contents.Count<3 && playerNum == playerID){ //check that no more than 3 drinks have been added
        Debug.Log("true?");
            if(Contents.Contains(ingr) ==false){ //check that ingredient isn't already in there
                Contents.Add(ingr);
                Debug.Log("cup " + playerID +" added " + ingr);
             }
        }
    }
}
