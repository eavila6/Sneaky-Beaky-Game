using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIManager : MonoBehaviour {
    
    public static UIManager instance; // current UIManager, should only be one

    [Header ("Texts")]
    public TextMeshProUGUI gameOverText;
    void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
            Debug.Log("instance could not be created in UIManager");
        }

        instance = this;

        // keep instance up across screenloading
        DontDestroyOnLoad(this);
    }

    
    public static void PlayerVictory(){
        if(instance == null){
            return;
        }
        Debug.Log("Enabling victory text");
        // enable game over text
        instance.gameOverText.enabled = true;
    }
}
