using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using Game;
using TMPro;
using UnityEngine;

public class StarterController : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;
    [SerializeField] private List<GameObject> cups;

    [SerializeField] private GameObject GM;

    [SerializeField] private float waitTime;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private List<GameObject> buttonPrompts;
    [SerializeField] private GameObject instructionsPrompt;
    private float ct;
    private int sec;
    private bool started;
    private List<PlayerScript> activatedPlayers;


    // Start is called before the first frame update
    void Start()
    {
        activatedPlayers = new List<PlayerScript>();

     foreach(GameObject p in players){
        p.SetActive(false);
     }   
     foreach(GameObject c in cups){
        c.SetActive(false);
     }
     GM.SetActive(false);
     started = false;
     ct = 0;
     sec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!started){
            for(int i = 0; i<=3; i++){ //look for button prompts to spawn players + cups
                string inputPrefix = "P"+(i+1);
                if(Input.GetButtonDown(inputPrefix + "Button" + (1)) || Input.GetButtonDown(inputPrefix + "Button" + (2)) || Input.GetButtonDown(inputPrefix + "Button" + (3))){
                    Debug.Log("spawn "+ inputPrefix);
                    players[i].SetActive(true);
                    players[i].GetComponent<PlayerScript>().Immobilize(false);
                    activatedPlayers.Add(players[i].GetComponent<PlayerScript>());
                    cups[i].SetActive(true);
                    buttonPrompts[i].SetActive(false);
                    sec = 0;
                    ct = 0;

                    if(activatedPlayers.Count>=4){ //if all players activated just start
                        sec = 15;
                    }
                }
            }

            ct+=Time.deltaTime;
            if(ct>=1){
                sec++;
                ct = 0;
            }
            if(sec>=waitTime){ //display time until game starts
                started = true;
                timerText.text = "0:00";
                StartGame();
            } else{
                int display = (int)(waitTime-sec);
                if(display<10){
                    timerText.text = "0:0"+display;
                }else{
                    timerText.text = "0:"+display;
                }
            }
        }


    }

    private void StartGame(){ // the game starts
        GM.SetActive(true); //timer and manager activate

        foreach(GameObject g in buttonPrompts){ //put the guides away
            g.SetActive(false);
            instructionsPrompt.SetActive(false);
        }

        foreach(PlayerScript p in activatedPlayers){//let the players move
            p.Immobilize(true);
        }
        
    }
}
