using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class GameTimerScript : MonoBehaviour
{

    public float ct;
    [SerializeField] float roundLength;
    [SerializeField] LeaderboardManagerMod LB;
    private bool boardup;
    // Start is called before the first frame update

    void Awake(){
        Time.timeScale = 1;
        boardup = false;
    }
    void Start()
    {
        ct = roundLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boardup){
        
            if(ct <=0){
                 //Time.timeScale = 0;
                 boardup = true;
                LB.EnterHighScores();
                Debug.Log("scores??");
            } else{
                ct-=Time.deltaTime;
            }
        }

        
    }
}
