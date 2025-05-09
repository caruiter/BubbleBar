using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] GameObject thePauseMenu;
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        thePauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused && Input.GetKeyDown(KeyCode.Space)){ //player pauses the menu with spacebar
            paused = true;
            thePauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            for(int i = 0; i<=3; i++){ //look for button presses from each player with buttons 1 & 3
                string inputPrefix = "P"+(i+1);
                if(Input.GetButtonDown(inputPrefix + "Button" + (1)) && Input.GetButtonDown(inputPrefix + "Button" + (3))){
                    //Quit Game
                    Application.Quit();
                }
            }
            if(Input.GetKeyDown(KeyCode.Space)){ //unpause menu with spacebar
                paused = false;
                Time.timeScale = 1;
                thePauseMenu.SetActive(false);
            }
        }
    }
}
