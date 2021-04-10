using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

    public float forceJump;

    public bool goingUp;
    public bool goingDown;
    private bool isDead;
    public bool isFlip;
    public bool isOutOfScreen;


    public bool getDead()
    {
        return isDead; // return if the player is dead, in case it isn't obvious enough
    }

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        isDead = false; // the player isn't dead when the game starts, who would have guessed
        isFlip = false; // the sprite of the player is gonna get flipped when he dies
        isOutOfScreen = false; // the plan was to delete the player obj when he got out of screen, but I guess I kinda forgot but never deleted this, heh
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            rbPlayer.gravityScale = 1;

            // Flip the sprite when he dies
            if (!isFlip)
            {
                Vector3 flipPosition = transform.localScale;
                flipPosition.y *= -1;
                transform.localScale = flipPosition;
                isFlip = true; // I wouldn't want the sprite to be flipping all the time, so I only execute this once by using this bool
            }
            if(transform.localPosition.y < -5)
            {
                isOutOfScreen = true; // again, I'm checking if he is out of screen but not actually doing anything with this
            }
            
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                // First I make it so there's no gravity force on him anymore
                rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, 0);
                // that way, when I add force to the jump it won't have to fight against gravity and the jump will be consistent
                rbPlayer.AddForce(new Vector2(rbPlayer.velocity.x, forceJump));
            }
            else
            {
                // if you ain't jumping you are falling
                rbPlayer.gravityScale = 1;
            }

            if (rbPlayer.velocity.y < 0)
            {
                //if you are falling, change the bools so the animation changes
                goingUp = false;
                goingDown = true;
            }
            else
            {
                goingUp = true;
                goingDown = false;
            }
            // change the bools so the animation changes if he is going up or down
            animPlayer.SetBool("GoingUp", goingUp);
            animPlayer.SetBool("GoingDown", goingDown);
        }

    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isDead = true;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if he ever collides with anything, he dies
        isDead = true;
    }

    private void FixedUpdate()
    {

    }
}
