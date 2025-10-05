using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private List<TextMeshProUGUI> scoreTexts;
    [SerializeField] private List<TextMeshProUGUI> playerRankings;
    private bool showingRanks;
    private float rankTimer;

    void Awake(){
        Time.timeScale = 1;
        boardup = false;
        showingRanks = false;
        scoreScreen.SetActive(false);
    }
    void Start()
    {
        ct = 0;
        sec = 0;
        rankTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boardup){
            if(ct>=1){
                sec++;
                ct = 0;
                float tempTimer = roundLength - sec;
                int min = (int)tempTimer / 60;
                int tempSec = (int)tempTimer % 60;

                if(tempSec<10){
                    TimerText.text = min + ":0"+tempSec;
                } else{
                    TimerText.text = min+ ":" +tempSec;
                }
                /*
                if((roundLength-sec)<10){
                    TimerText.text = "0:0" + (roundLength -sec);
                } else{
                    TimerText.text = "0:" + (roundLength-sec);
                }*/
                

            }else{
                ct+=Time.deltaTime;
            }

            if(sec >= roundLength){
                //Time.timeScale = 0;
                boardup = true;
                //LB.EnterHighScores();
                ShowRanks();
                showingRanks = true;
                Debug.Log("scores??");
                GetComponent<PauseScreenManager>().enabled = false; //disable pausing
            } 
        } else{
            if(showingRanks){
                rankTimer+=Time.deltaTime;
                if(rankTimer >=10){
                    for(int a = 0; a<4; a++){ //look for input from each player
                        string inputPrefix = "P"+(a+1);
                        if(Input.GetButtonDown(inputPrefix + "Button" + (1)) || Input.GetButtonDown(inputPrefix + "Button" + (2)) || Input.GetButtonDown(inputPrefix + "Button" + (3))){
                            ShowLeaderBoard();
                            showingRanks = false;
                        }
                    } 
                }
            }
            
        }

        
    }

    public void ShowLeaderBoard(){ //input top scores, calld after showRanks
        LB.EnterHighScores();
        scoreScreen.SetActive(false);
    }

    private void ShowRanks(){ //show player ranks after end of game
        GameManagerMod GM = GameObject.Find("GameManager").GetComponent<GameManagerMod>();
        scoreScreen.SetActive(true);

        foreach(CupScript cup in GM.cups){
            cup.EndGame();
        }

        List<int> sorting = new List<int>(); //put scores in list and sort
        for(int s = 0; s < 2;s++){ //edited for 2 Player
           sorting.Add(GM.players[s].Score);
        }
        sorting.Sort();

        for(int i =0; i<2; i++){ // edited for 2 player
            scoreTexts[i].text = GM.players[i].Score.ToString(); //display player's score

            if(sorting[1]==GM.players[i].Score){//determine and show rank //edited for 2 Player
                playerRankings[i].text = "1st";
            } else {
                playerRankings[i].text = "2nd";
            } /*else if(sorting[1]==GM.players[i].Score){
                playerRankings[i].text = "3rd";
            } else{
                playerRankings[i].text = "4th";
            }*/
        }
    }
}
