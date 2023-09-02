using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // singleton structure
    public static GameManager instance;  // this current GameManager, should only be one

    public int score = 0;   // total score amassed
    public int gems = 0;    // total number gems acquired

    bool isGameOver;    // ending the game
    void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
            Debug.Log("instance could not be created");
        }

        instance = this;

        // keep instance up across screenloading
        DontDestroyOnLoad(this);
    }

    void Update() {
        if(isGameOver){
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
    }
}
