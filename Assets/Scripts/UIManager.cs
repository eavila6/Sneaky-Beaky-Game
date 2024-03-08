using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIManager : MonoBehaviour {
    
    public static UIManager instance; // current UIManager, should only be one

    [Header ("Texts")]
    public TextMeshProUGUI guideText;
    public TextMeshProUGUI objText;
    public TextMeshProUGUI gameOverText;
    void Awake(){
        if(instance != null && instance != this){
            Destroy(this);
            Debug.Log("instance could not be created in UIManager");
        }

        instance = this;

        // enable guiding textboxes
        instance.guideText.enabled= true;
        instance.objText.enabled = true;

        // keep instance up across screenloading
        DontDestroyOnLoad(this);
    }

    
    public static void PlayerVictory(){
        if(instance == null){
            return;
        }
        Debug.Log("Enabling victory text");

        // disable the guides
        instance.guideText.enabled = false;
        instance.objText.enabled = false;
        // enable game over text
        instance.gameOverText.enabled = true;
    }
}
