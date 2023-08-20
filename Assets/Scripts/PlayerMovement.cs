using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public bool drawDebugRaycasts = true;	//Should the environment checks be visualized

    [Header ("Movement Properties")]
    public float speed = 10f;   // player speed
    public float crouchSpeedDivisor = 3f;   // crouching speed penalty
    public float jumpHoldDuration = 0.1f;
    public float jumpForce = 5f;
    public float jumpHoldForce = 2f;

    [Header ("Status Flags")]
    // Forces that physically affect the player
    public bool grounded = false;   // I'll need this for later
    // public bool falling;    // ditto ^^
    // public bool caught;     // When a guard spots you
    // public bool hiding;     // Is the player hiding?
    public bool isJumping;
	public bool isCrouching;
    public bool isHeadBlocked;

    [Header ("Environment Checks")]
    // Limitations of the player's body
    public float footOffset = 0.4f;     // X offset of feet for raycasts
    public float headClearance = 0.5f;  // distance needed for head not to get hit
    public float grabDist = 0.4f;       // how far the player can reach
    public float groundDist = 0.2f;     // fair game for being on the ground
    
    
    [Header ("Layer Mask")] public LayerMask groundLayer;            // layer of the ground

    PlayerInput input;  // another script called playerInput will make this work
    BoxCollider2D bodyCollider;
    Rigidbody2D rb;

    float jumpTime;
    float originalXScale;       // will need this for turning
    int direction = 1;          // Direction player faces

    Vector2 colliderStandSize;				//Size of the standing collider
	Vector2 colliderStandOffset;			//Offset of the standing collider
	Vector2 colliderCrouchSize;				//Size of the crouching collider
	Vector2 colliderCrouchOffset;			//Offset of the crouching collider

    [SerializeField]
    private GameObject leftRayOffset;
    [SerializeField]
    private GameObject rightRayOffset;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    void Update() 
    {
        // check if player is on the ground
        groundCheck();

        // for now only ground movement is possible
        // might give player air movement if it seems like
        // a good idea
        GroundMovement();
        MidAirMovement();
    }

    // this is PhysicsCheck in the tutorial
    void groundCheck() 
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
		grounded = false;
		isHeadBlocked = false;
        RaycastHit2D leftCheck;
        RaycastHit2D rightCheck;

        /*
		//Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);*/

		leftCheck = Physics2D.Raycast(leftRayOffset.transform.position, Vector2.down, groundDist, groundLayer);
        rightCheck = Physics2D.Raycast(rightRayOffset.transform.position, Vector2.down, groundDist, groundLayer);
        // Debug.Log(LayerMask.GetMask("Ground"));
        // Debug.Log("Left:  " + leftCheck.collider);
        // Debug.Log("Right: " + rightCheck.collider);

		//If either ray hit the ground, the player is on the ground
		if (leftCheck.collider != null || rightCheck.collider != null)
        {
            grounded = true;
        }
        /*
        //If either ray hit the ground, the player is on the ground
		if (leftCheck || rightCheck) {
			isOnGround = true;
        }*/


		//Cast the ray to check above the player's head
		RaycastHit2D headCheck = Physics2D.Raycast(new Vector2(0f, bodyCollider.size.y), Vector2.up, headClearance);

        //If that ray hits, the player's head is blocked
        if (headCheck)
        {
            isHeadBlocked = true;
        }
    }

    void GroundMovement() {

        // Crouching handler
        // If holding crouch button but not crouching, crouch
        // DEBUG: put this behind !isCrou... :  && !isJumping
		if (input.crouchHeld && !isCrouching && !isJumping) {
			Crouch();
        } 
        // if not holding crouch but currently crouching, stand up
        else if (!input.crouchHeld && isCrouching) {
			StandUp();
        }
		// if crouching and no longer on the ground, stand up
		else if (!grounded && isCrouching) {
			StandUp();
        }

        //Calculate desired velocity based on inputs
		float xVelocity = speed * input.horizontal;

        //If sign of velocity and direction don't match, flip character
		if (xVelocity * direction < 0f) {
			flipDirection();
        }

        //If player is crouching, reduce velocity
		if (isCrouching) {
			xVelocity /= crouchSpeedDivisor;
        }

        //Apply the desired velocity 
		rb.velocity = new Vector2(xVelocity, rb.velocity.y);

    }

    void MidAirMovement(){
        
        if (input.jumpPressed && !isJumping && grounded){
			//...The player is no longer on the groud and is jumping...
			grounded = false;
			isJumping = true;

			//...record the time the player will stop being able to boost their jump...
			jumpTime = Time.time + jumpHoldDuration;

			//...add the jump force to the rigidbody...
			rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

			//...and tell the Audio Manager to play the jump audio
			// AudioManager.PlayJumpAudio();
		}

        else if (isJumping) {
			//...and the jump button is held, apply an incremental force to the rigidbody...
			if (input.jumpHeld)
				rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

			//...and if jump time is past, set isJumping to false
			if (jumpTime <= Time.time)
				isJumping = false;
		}
    }

    void flipDirection() {
        // flip the player's direction
        direction *= -1;

        // get the old scale
        Vector3 scale = transform.localScale;
        // turn it around
        scale.x = originalXScale * direction;
        // apply the new, turned around scale
        transform.localScale = scale;
    }

    void Crouch() {
        Debug.Log("Crouching input detected");

		//The player is crouching
		isCrouching = true;

		//Apply the crouching collider size and offset
		bodyCollider.size = colliderCrouchSize;
		bodyCollider.offset = colliderCrouchOffset;
	}

    void StandUp() {
		//If the player's head is blocked, they can't stand so exit
		if (isHeadBlocked) {
            Debug.Log("Can't stand up!");
			return;
        }

		//The player isn't crouching
		isCrouching = false;
	
		//Apply the standing collider size and offset
		bodyCollider.size = colliderStandSize;
		bodyCollider.offset = colliderStandOffset;
	}


    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality

    // these two methods are from the tutorial I've been following for player movement
    // I'm testing them out to see if they're any good
    /*
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Physics2D.Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycast and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }
    */
}
