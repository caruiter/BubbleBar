using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Game;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CupScript : MonoBehaviour
{
    public List<String> Contents;
    [SerializeField] private int playerID;
    [SerializeField] private PlayerScript playermatch;
    [SerializeField] private GameManagerMod gm;
    private List<GameObject> contentIcons;

    public RecipeScriptableObject recipe;
    private string inputPrefix;	// InputManager uses "P1Button1", "P1Horizontal", etc. 

    private bool controllable;
    private bool shaking;
    [SerializeField] private GameObject shakePrompt;
    [SerializeField] private GameObject discardPrompt;

    private Animator anim;

    [SerializeField] Animator drinkAnim;

    [SerializeField] private int shakeCount;
    [SerializeField] private int shakeTarget; //change this in inspector to test

    [SerializeField] private List<UnityEngine.UI.Image> cardIcons;
    [SerializeField] private GameObject recipeCard;

    [SerializeField] private List<Sprite> cupSprites;


    void Awake(){
        inputPrefix = "P" + playerID; // Set inputPrefix using correct playerID
        controllable = true;
        shaking = false;
        anim = GetComponent<Animator>();
        //drinkAnim = GetComponentInParent<Animator>();
        Debug.Log(drinkAnim + "DRINK??");

        recipe = gm.GetNewRecipe();
        UpdateCard();
        EmptyCup();
    }

    // Start is called before the first frame update    
    void Start()
    {
        Contents = new List<String>();
        contentIcons = new List<GameObject>();
        Debug.Log(this.transform.GetChild(0).GetChild(0).GetChild(0).name);
        contentIcons.Add(this.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
        contentIcons.Add(this.transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
        contentIcons.Add(this.transform.GetChild(0).GetChild(0).GetChild(2).gameObject);
        foreach(GameObject i in contentIcons){
            i.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(controllable && !shaking){ // if allowing control and not being shaken
        if(controllable){ // if allowing control and not being shaken
         if (Input.GetButtonDown(inputPrefix + "Button" + (3))&&Contents.Count!=0) //discard contents of cup
         {
            Debug.Log("discarded contents of cup " + playerID);
            Contents = new List<string>();
            anim.SetTrigger("Discard");
            foreach(GameObject g in contentIcons){
                g.SetActive(false);
            }
         }
        }

        if(shaking){ //allow button mashing if shaking
            ShakeDrink();
        } /*else{
            //If there are 3 ingredients, player must now shake the drink
            if(Contents.Count==3)
            {
                Debug.Log("cup full");
                shaking = true;

                //switch over to new animation if correct
                bool correct = true;
                foreach(string ingr in recipe.ingredients){
                    if(!Contents.Contains(ingr)){
                        correct =false;
                    }
                }

                Debug.Log("got here");
                if(correct){ // if correct up points and get new recipe
                    TriggerFullDrinkAnim(recipe.drinkName);
                    shakePrompt.SetActive(true);
                } else{
                    discardPrompt.SetActive(true);
                }
            }
        }*/
    }

    public void AddIngredient(IngredientScript ingr, int playerNum){ //called by IngredientScript class
    Debug.Log(playerNum + " | " + playerID);
        if(Contents.Count<3 && playerNum == playerID){ //check that no more than 3 drinks have been added
            //Debug.Log("true?");
            //if(Contents.Contains(ingr) ==false){ //check that ingredient isn't already in there
            Contents.Add(ingr.theIngredient);
            Debug.Log("cup " + playerID +" added " + ingr);
            //}
            contentIcons[Contents.Count-1].SetActive(true);
            UnityEngine.UI.Image im = contentIcons[Contents.Count-1].GetComponent<UnityEngine.UI.Image>();
            im.sprite = ingr.icon; //COMMENT BACK IN AND TEST WHEN WE HAVE THESE

            if(ingr.theIngredient == "SODA"){ //trigger animation if soda
                TriggerSodaAnim("SODA");
            } else if(ingr.theIngredient == "CLEARSODA"){
                TriggerSodaAnim("CLEARSODA");
            }
                        //If there are 3 ingredients, player must now shake the drink
            if(Contents.Count==3)
            {
                Debug.Log("cup full");
                shaking = true;

                //switch over to new animation if correct
                bool correct = true;
                List<string> testCon = new List<string>();
                foreach(string ingre in Contents){
                    testCon.Add(ingre);
                }

                foreach(string ingre in recipe.ingredients){
                    if(!testCon.Contains(ingre)){
                        correct = false;
                    } else{
                        testCon.Remove(ingre);
                    }
                }

                Debug.Log("got here");
                if(correct){ // if correct up points and get new recipe
                    TriggerFullDrinkAnim(recipe.drinkName);
                    shakePrompt.SetActive(true);
                } else{
                    discardPrompt.SetActive(true);
                }
            }
                
        }


    }

    public void SetControllable(bool con){ //should buttons effect the cup
        controllable = con;
        if(con){
            recipeCard.SetActive(true);
        } else{
            recipeCard.SetActive(false);
        }
    }

    public void ShakeDrink()
    {
        //Put code for turning the emptying cup function off here
        
        //if(Input.GetButtonDown(inputPrefix + "Button" + (1)) || Input.GetButtonDown(inputPrefix + "Button" + (2)) || Input.GetButtonDown(inputPrefix + "Button" + (3)))
        if(Input.GetButtonDown(inputPrefix + "Button" + (2))) //modified to only count middle button
        { 
            //number of shakes goes up each time the player presses an input
            Debug.Log("shakes: " + shakeCount);
            shakeCount++;
        }

        if(shakeCount >= shakeTarget){ //Drink is finished shaking

            Debug.Log("Drink done!");
            shaking = false;

            //check if drink is correct
            bool correct = true;
            List<string> testCon = new List<string>();
            foreach(string ingre in Contents){
                testCon.Add(ingre);
            }

            foreach(string ingre in recipe.ingredients){
                if(!testCon.Contains(ingre)){
                    correct = false;
                } else{
                    testCon.Remove(ingre);
                }
            }

            if(correct){ // if correct up points and get new recipe
                //TriggerFullDrinkAnim(recipe.drinkName);
                recipe = gm.GetNewRecipe();
                //playermatch.Score++;
                playermatch.IncreaseScore();
                UpdateCard();
            }

            foreach(GameObject i in contentIcons){
            i.SetActive(false);
            }
            Contents = new List<String>(); //clear contents and shake count
            shakeCount = 0;

            //ANIM?
            anim.SetTrigger("Finish");

            //UI
            discardPrompt.SetActive(false);
            shakePrompt.SetActive(false);
        }
    }

    public void UpdateCard(){
        for(int i = 0; i<3; i++){
            cardIcons[i].sprite = recipe.icons[i];
        }
    }


    public void IsAnimationPlaying(){
        Debug.Log("ANIMATION PLAYING");
    }

    public void StillLooping(){
        Debug.Log("still looping");
    }

    public void TriggerSodaAnim(string type){ //switches drink anim to most recently added soda
        switch(type){
            case "CLEARSODA": 
                drinkAnim.SetTrigger("ClearSoda");
            break;
            case "SODA":
                drinkAnim.SetTrigger("Soda");
            break;
        }
    }

    public void TriggerFullDrinkAnim(string drink){ //switches drink anim to completed drink
        switch(drink){
            case "CHERRYCOLA":
                drinkAnim.SetTrigger("CherryCola");
                break;
            case "CHERRYVANILLA":
                drinkAnim.SetTrigger("CherryVanilla");
                break;
            case "CITRUSCOOLER":
                drinkAnim.SetTrigger("CitrusCooler");
                break;
            case "CREAMSODA":
                drinkAnim.SetTrigger("CreamSoda");
                break;
            case "LIMESODA":
                drinkAnim.SetTrigger("LimeSoda");
                break;
            case "ORANGESODA":
                drinkAnim.SetTrigger("OrangeSoda");
                break;
            case "SHIRLEYTEMPLE":
                drinkAnim.SetTrigger("ShirleyTemple");
                break;
            case "CREAMSICLE":
                drinkAnim.SetTrigger("Creamsicle");
                break;
        }
    }

    public string GetCupType(){ //returns type of cup drink requires
        switch(recipe.drinkName){
            case "CHERRYCOLA":
                return "TallCup";
            case "ORANGESODA":
                return "TallCup";
            case "SHIRLEYTEMPLE":
                return "ShortCup";
            case "CITRUSCOOLER":
                return "ShortCup";
            case "CHERRYVANILLA":
                return "WineGlass";
            case "CREAMSODA":
                return "WineGlass";
            case "LIMESODA":
                return "TallGlass";
            case "CREAMSICLE":
                return "TallGlass";
            default:
                Debug.Log("cup type fail");
                return null;
        }
    }

    public void EmptyCup(){ //set animation trigger based on cup type
        string type = GetCupType();
        Debug.Log("empty cup? " + type);
        switch(type){
            case "TallCup":
                drinkAnim.SetTrigger("EmptyTallCup");
                break;
            case "ShortCup":
                drinkAnim.SetTrigger("EmptyShortCup");
                break;
            case "WineGlass":
                drinkAnim.SetTrigger("EmptyWineGlass");
                break;
            case "TallGlass":
                drinkAnim.SetTrigger("EmptyTallGlass");
                break;
        }

        shaking = false;
        discardPrompt.SetActive(false);
        shakePrompt.SetActive(false);
    }
}
