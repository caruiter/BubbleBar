using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerScript : MonoBehaviour
{

    public float ct;
    [SerializeField] float roundLength;
    // Start is called before the first frame update

    void Awake(){
        Time.timeScale = 1;
    }
    void Start()
    {
        ct = roundLength;
    }

    // Update is called once per frame
    void Update()
    {
        ct-=Time.deltaTime;
        if(ct <=0){
            Time.timeScale = 0;
        }
        
    }
}
