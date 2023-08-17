using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DefaultExe... is a property that makes this script run as "firstly" as possible
// I'm doing this to make controls snappy by processing player inputs first
// CREDIT: @Unity RobbiePlatformer tutorial
[DefaultExecutionOrder(-100)]

public class PlayerInput : MonoBehaviour {

    // NOTE: the Held vars are nice to distinguish between taps and holds
    // for more dynamic player movement, also are useful for relaying info to other scripts

    [HideInInspector, Header ("Hidden player attributes")]
    public float horizontal;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool crouchPressed;
    public bool crouchHeld;

    [Header ("Status flags")]
    private bool readyToClear;
    

    // Update is called once per frame
    void Update() {
        
        // first clear the last inputs if true
        ClearInput();

        //If the Game Manager says the game is over, exit
        // if (GameManager.IsGameOver()){
            // return;
        // }

        //Process keyboard, mouse, gamepad (etc) inputs
		ProcessInputs();

        //Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
    }

    void FixedUpdate() {
		// reset inputs in the slower FixedUpdate cycle
		readyToClear = true;
	}

    void ClearInput() {
        //If not ready to clear, exit
		if (!readyToClear) {
			return;
        }

		//Reset inputs
		horizontal = 0f;
		jumpPressed = false;
        jumpHeld = false;
		crouchPressed = false;
        crouchHeld = false;

		readyToClear = false;
    }

    void ProcessInputs() {
        /* The idea is that inputs will be collected in this script, then handed over to the movement
        script. In this way I can add more stuff in the future and pipe it into other scripts that
        aren't necessarily bound to movement, like looking up and down
        */

        //Accumulate horizontal axis input
		horizontal		+= Input.GetAxis("Horizontal");

        //Accumulate button inputs
		jumpPressed	= jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");
        Debug.Log("Jump pressed: " + jumpPressed);

        crouchPressed = crouchPressed || Input.GetButtonDown("Crouch");
        Debug.Log("Crouch pressed: " + crouchPressed);
        crouchHeld = crouchHeld || Input.GetButton("Crouch");
    }
}
