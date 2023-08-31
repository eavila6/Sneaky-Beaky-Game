using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable : MonoBehaviour {
    int collectableLayer;    // for a layer comparison
    // apparently this is more efficient than a tag comparison

    TutorialMovement player;

    void Start() {
        collectableLayer = LayerMask.NameToLayer("Collectable");

        player = GetComponent<TutorialMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.layer != collectableLayer) {
            return;
        }

        collision.gameObject.SetActive(false);

        player.gems++;

        player.score += 10;
    }
}
