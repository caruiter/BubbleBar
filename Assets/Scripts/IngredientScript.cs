using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class IngredientScript : MonoBehaviour
{

    private bool carrying;
    private PlayerScript playerCarrying;
    private Vector2 distance;
    private float tilGrab;
    private string theIngredient;
    [SerializeField] IngredientScriptableObject ingredientScriptable;

    // Start is called before the first frame update

    void Start()
    {
     carrying = false;   
     theIngredient = ingredientScriptable.GetIngredient();
    }

    // Update is called once per frame
    void Update()
    {
        if(carrying){
            this.transform.position = new Vector2(playerCarrying.transform.position.x - distance.x,playerCarrying.transform.position.y - distance.y);
        }

        if(tilGrab>0){
            tilGrab -= Time.deltaTime;
        }
    }

    public void PickUp(PlayerScript p){ //player picks up the ingredient
        if(tilGrab!<=0){

            if(carrying == true){ // if being stolen, remove vars from playerscript
                Physics2D.IgnoreLayerCollision(9+playerCarrying.GetPlayerID(), 8,false); 
                playerCarrying.ReleaseItem();
            }

            carrying = true;
            playerCarrying = p;
            distance = p.transform.position - this.transform.position;
            p.GrabItem(this);

            this.gameObject.layer = LayerMask.NameToLayer("MovingIngredient"+playerCarrying.GetPlayerID());
            Physics2D.IgnoreLayerCollision(9+playerCarrying.GetPlayerID(), 9,true); 
        }
    }

    public void SetDown(){ //player sets down the ingredient

        Physics2D.IgnoreLayerCollision(9+playerCarrying.GetPlayerID(), 8,false); 
        this.gameObject.layer = LayerMask.NameToLayer("Ingredient");

        carrying = false;
        playerCarrying.GetComponent<PlayerScript>().ReleaseItem();
        playerCarrying = null;
        tilGrab = 1;
        //Physics2D.IgnoreLayerCollision(7, 9,false); 
        //Physics2D.IgnoreLayerCollision(7, 8,false); 
    }

    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Cup") && carrying){
            //Physics2D.IgnoreLayerCollision(7, 8,true); 
            Physics2D.IgnoreLayerCollision(9+playerCarrying.GetPlayerID(), 8,true); 
            int id = playerCarrying.GetPlayerID();
            other.gameObject.GetComponent<CupScript>().AddIngredient(theIngredient, id);
            Debug.Log("clink");
        }
    }
}
