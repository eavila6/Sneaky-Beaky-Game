using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable : MonoBehaviour {

    public int score = 10;
    public int gemWeight = 1;
    int playerLayer;    // for a layer comparison
    // apparently this is more efficient than a tag comparison

    void Start() {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.layer != playerLayer) {
            return;
        }

        GameManager.instance.score += score;
        GameManager.instance.gems += gemWeight;

        gameObject.SetActive(false); 
    }
}
