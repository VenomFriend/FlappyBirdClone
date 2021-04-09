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
        if( (this.gameObject.transform.position.x < objPlayer.transform.position.x) && !gaveScore)
        {
            gaveScore = true;
            gameLogicScript.addScore();

        }

        if (this.gameObject.transform.position.x <= -4)
        {
            Destroy(this.gameObject);
        }
    }
}
