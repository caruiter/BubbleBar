using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class GameTimerScript : MonoBehaviour
{

    public float ct;
    private int sec;
    [SerializeField] float roundLength;
    [SerializeField] LeaderboardManagerMod LB;
    [SerializeField] TextMeshProUGUI TimerText;
    private bool boardup;
    // Start is called before the first frame update

    void Awake(){
        Time.timeScale = 1;
        boardup = false;
    }
    void Start()
    {
        ct = 0;
        sec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boardup){
            if(ct>=1){
                sec++;
                ct = 0;
                if((roundLength-sec)<10){
                     TimerText.text = "0:0" + (roundLength-sec);
                } else{
                    TimerText.text = "0:" + (roundLength-sec);
                }

            }else{
                ct+=Time.deltaTime;
            }

            if(sec >= roundLength){
                //Time.timeScale = 0;
                boardup = true;
                LB.EnterHighScores();
                Debug.Log("scores??");
            } 
        }

        
    }
}
