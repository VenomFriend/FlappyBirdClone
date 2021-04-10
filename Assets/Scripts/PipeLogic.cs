using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLogic : MonoBehaviour
{

    private Rigidbody2D rbPipe;
    public float xVelocity = -2.0f;
    GameObject objPlayer;
    GameObject objGameLogic;
    GameLogic gameLogicScript;
    bool gaveScore;

    // Start is called before the first frame update
    void Start()
    {
        rbPipe = GetComponent<Rigidbody2D>();
        rbPipe.velocity = new Vector2(xVelocity, 0);
        objPlayer = GameObject.Find("Player");
        objGameLogic = GameObject.Find("GameLogic");
        gameLogicScript = objGameLogic.transform.GetComponent<GameLogic>();
        gaveScore = false;
    }

    // Update is called once per frame
    void Update()
    {
        // This will run when the player dies, so everything stops moving
        if (gameLogicScript.getStopEverything())
        {
            rbPipe.velocity = new Vector2(0, 0);
        }
        // I could just check if it's smaller than 0 and it would do the exact same thing
        // But basically it checks if the player passed the pipe(or technicallly if the pipe passed the player, since the player doesn't move in the X axys, only the pipes
        // and if he did, add some points to his score
        if( (this.gameObject.transform.position.x < objPlayer.transform.position.x) && !gaveScore)
        {
            gaveScore = true;
            gameLogicScript.addScore();

        }

        // If it's outside of the screen, delete the object 
        // there's probably a smarter and better way to do this, instead of creating lots of pipes and destroying them
        // like teleporting the pipes and having only like 3 of each pipe in the game or something, and teleporting them at random times
        // but screw it, this is easier and I don't have much time left
        if (this.gameObject.transform.position.x <= -4)
        {
            Destroy(this.gameObject);
        }
    }
}
