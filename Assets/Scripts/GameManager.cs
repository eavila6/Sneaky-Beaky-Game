using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // singleton structure
    public static GameManager instance;  // this current GameManager, should only be one

    [Header("Scoring")]
    public int score = 0;   // total score amassed
    public int gems = 0;    // total number gems acquired

    bool isGameOver;    // ending the game
    void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
            Debug.Log("instance could not be created in Game Manager");
        }

        instance = this;

        // keep instance up across screenloading
        DontDestroyOnLoad(this);
    }

    void Update(){
        // Debug.Log("isGameOver is " + isGameOver);
        if(isGameOver){
            // stop all movement
            Time.timeScale = 0f;
            return;
        }
    }

    public static bool IsGameOver(){
        if(instance == null){
            return false;
        }
        return instance.isGameOver;
    }

    public static void PlayerWin(){
        if(instance == null){
            return;
        }
        instance.isGameOver = true;
        UIManager.PlayerVictory();

        Debug.Log("You Win!");
    }

    public void IncrementScore(int scoreVal, int itemWeight){
        score += scoreVal;
        gems += itemWeight;
        if(score >= 50 || gems >= 5){
            instance.isGameOver = true;
            Debug.Log("Game is over because score is " + score + " and gems is "+ gems);
        }
    }
}
