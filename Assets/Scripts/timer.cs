using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    Image timeBar;
    public float maxTime =  1f; // set the maximum amount  of time possible
    float timeLeft;
    // public GameObject timeUpTxt

    // Start is called before the first frame update
    void Start() {
        // timeUpTxt.SetActive(false);
        timeBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void LateUpdate() {
        // using the late update to give the player the maximum amount of time to
        // do whatever
        if(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeBar.fillAmount = timeLeft / maxTime;
        } else {
            // timesUpTxt.SetActive (true);
            GameManager.instance.isGameOver = true;
            
            // the time left should return a ping to the game manager
            // to raise the gameover flag and stop the game
            // timeBar.timeScale = 0;
        }
    }

    public void incrementTimer(int timeVal){
        // this is the funct that'll allow for the clock to go up
        // whenever the player collects a gem
        Debug.Log("timeLeft is " + timeLeft);
        Debug.Log("timeVal is " + timeVal);
        timeLeft += (float)timeVal;
        Debug.Log("timeLeft is " + timeLeft);
    }
}
