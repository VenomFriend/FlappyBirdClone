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
        return isDead;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        isDead = false;
        isFlip = false;
        isOutOfScreen = false;
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
                isFlip = true;
            }
            if(transform.localPosition.y < -5)
            {
                isOutOfScreen = true;
            }
            
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, 0);
                rbPlayer.AddForce(new Vector2(rbPlayer.velocity.x, forceJump));
            }
            else
            {
                rbPlayer.gravityScale = 1;
            }

            if (rbPlayer.velocity.y < 0)
            {
                goingUp = false;
                goingDown = true;
            }
            else
            {
                goingUp = true;
                goingDown = false;
            }
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
        isDead = true;
    }

    private void FixedUpdate()
    {

    }
}
