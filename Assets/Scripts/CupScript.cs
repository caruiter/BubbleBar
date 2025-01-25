using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    public List<String> Contents;

    // Start is called before the first frame update
    void Start()
    {
        Contents = new List<String>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddIngredient(string ingr){ //called by IngredientScript class
        if(Contents.Count!>=3){ //check that no more than 3 drinks have been added
            if(Contents.Contains(ingr) ==false){ //check that ingredient isn't already in there
                Contents.Add(ingr);
                Debug.Log("cup added " + ingr);
             }
        }
    }
}
