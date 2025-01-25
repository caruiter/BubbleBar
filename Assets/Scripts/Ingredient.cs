using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ingredient : MonoBehaviour
{

    private bool carrying;
    private Player playerCarrying;
    private Vector2 distance;
    private float tilGrab;

    // Start is called before the first frame update

    void Start()
    {
     carrying = false;   
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

    public void PickUp(Player p){ //player picks up the ingredient
        if(tilGrab!<=0){
            carrying = true;
            playerCarrying = p;
            distance = p.transform.position - this.transform.position;
            p.GrabItem(this);

            Physics2D.IgnoreLayerCollision(7, 9,true); 
        }
    }

    public void SetDown(){ //player sets down the ingredient
        carrying = false;
        playerCarrying.GetComponent<Player>().ReleaseItem();
        playerCarrying = null;
        tilGrab = 1;
        Physics2D.IgnoreLayerCollision(7, 9,false); 
    }
}
