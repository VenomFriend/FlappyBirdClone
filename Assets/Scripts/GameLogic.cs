using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameLogic : MonoBehaviour
{

    public float randomTimePipeDown = 0.0f; // How long to wait before I spawn another pipe
    public float timeCountPipeDown = 0.0f;    // The counter of time, when this is higher than the random time, it'll spawn the pipe
    public float randomTimePipeUp = 0.0f;
    public float timeCountPipeUp = 0.0f;
    public GameObject pipeDown; // The pipe that is down(d'uh)
    public GameObject pipeUp; // The pipe that is up
    public float randomSize1; // Each pipe can be of different sizes on the Y axis
    public float randomSize2;

    public float playerScore; // You gain 1 point each time you pass through the pipes, yaay
    GameObject txtScore; // I need to get the text object to display the text
    Text displayScore; // and the text

    GameObject objPlayer; // Gonna need to use a reference to the player here to get some stuff
    GameObject objProcessLayer; // need this to do the grayscale screen magic

    PostProcessVolume volume; // need this to do the saturation thing to make the screen gray
    ColorGrading colorGradingLayer; // so many things I need to get

    GameObject txtCanvas; // need the canvas so I can add another text on it, but this one will be the YOU DIED one
    Text txtYouDied; 


    //Get the audio for when you die
    AudioSource audioGame;

    public AudioClip deathSound; // the YOU DIED sound
    public AudioClip gameMusic; // the game music

    private bool stopEverything; // stop the movement of everything when the player dies

    public bool getStopEverything() // use this when another script needs to know if they need to stop
    {
        return stopEverything;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0; // score starts at 0, d'uh
        txtScore = GameObject.Find("Text"); // Gonna need the reference to the text object so I can update the score
        objPlayer = GameObject.Find("Player"); // gonna need a reference to the player so I can know if he collided and died
        objProcessLayer = GameObject.Find("ProcessLayer"); // need this to make the grayscale thing
        displayScore = txtScore.GetComponent<Text>(); // get the score text
        displayScore.text = "Score: " + playerScore.ToString(); // update it to be the player score
        stopEverything = false; // player is still alive, so everything should be moving
        txtCanvas = GameObject.Find("Canvas"); // need the canvas to be able to put the YOU DIED text on it
        txtYouDied = Resources.Load<Text>("YouDied"); // load the YOU DIED text

        audioGame = GetComponent<AudioSource>(); // gonna need to use this to play the sounds
        audioGame.clip = gameMusic; // start the game with the music playing
        audioGame.loop = true; // put the music on loop
        audioGame.Play(); // start the music
    }


    // Update is called once per frame
    void Update()
    {
        // If you are dead, make the screen black and white, stop the music, play the dark souls sound effect and wait for the player to press R to restart
        if (objPlayer.GetComponent<PlayerBehaviour>().getDead() && !stopEverything) // only need to do this once, so I check if everything is already stopped with the bool stopEverything
        {
            stopEverything = true;
            volume = objProcessLayer.GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings<ColorGrading>(out colorGradingLayer);
            
            //Make everything grayscale
            colorGradingLayer.saturation.value = -100.0f;

            // Try to make a black square to put the text on

            GameObject blackRectangleObject = new GameObject();
            blackRectangleObject.transform.SetParent(txtCanvas.transform);
            Image imageBlackRectangle = blackRectangleObject.AddComponent<Image>();
            imageBlackRectangle.color = Color.black;

            blackRectangleObject.transform.position = new Vector3(0, 0, 0);
            imageBlackRectangle.transform.localScale = new Vector3(12, 2, 0);



            //Show the "YOU DIED" text on screen
            Text textObj = Instantiate(txtYouDied, new Vector2(-10.0f, -435.0f), Quaternion.identity);
            textObj.transform.SetParent(txtCanvas.transform, false);


            // Play the sound
            audioGame.clip = deathSound;
            audioGame.loop = false;
            audioGame.Play(0);
            //displayScore.text = textObj.text;
        }
        if (stopEverything) // If you died, display the message on how to restart and restart the game if you actually press the button
        {
            displayScore.text = "Score: " + playerScore.ToString() + "\n\n\n\n\n\nPress R to Restart!";
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Restart the level if the player press R, but only if he is dead
                SceneManager.LoadScene(0);
                //Application.LoadLevel(0);
            }
        }
        // if the player is still alive, spawn the pipes outside of the screen in a random amount of time and destroy them when they get off the screen to the left
        else
        {
            // Before I had two timers, one for each pipe, but since I now spawn them always at the same X position, there's no need for 2 timers, but I'm too lazy to change the names
            timeCountPipeDown += Time.deltaTime;
            //timeCountPipeUp += Time.deltaTime;
            if (timeCountPipeDown >= randomTimePipeDown)
            {
                timeCountPipeDown = 0;
                randomTimePipeDown = (Random.value * 2.0f) + 1; // 1 a 3 segundos
                GameObject newPipeDown = Instantiate(pipeDown, new Vector2(4.0f, -2.946701f), Quaternion.identity);
                GameObject newPipeUp = Instantiate(pipeUp, new Vector2(4.0f, 5.0f), Quaternion.identity);


                /*
                 * Let's see... PipeDown height is 121*3 = 363 while PipeUp height is 135*3 = 405, so they both together would occupy 405+363 of the screen, or 768 pixels
                 * The screen has 1920 pixels, but there is the ground. The ground is 56 in height, which I scale 3.9 times, so 218.4pixels, or 219 pixels. 
                 * So there's 1920 - (768 + 219) pixels left on the screen, or 933 pixels left on the Y position...
                 * 
                 * Nevermind, camera works in mysterious ways, gonna need to find a new way to calculate this shit
                 */

                randomSize1 = Random.Range(0.5f, 4.0f);
                float minVal = Mathf.Abs(4.5f - randomSize1);
                // The transform of the pipes should be at least 4.5 when added together, so they are not that much far apart
                randomSize2 = Random.Range(minVal, 5.0f - randomSize1);
                //randomSize1 = 1;
                //randomSize2 = 1;
                newPipeDown.transform.localScale = new Vector3(newPipeDown.transform.localScale.x, randomSize2);
                newPipeUp.transform.localScale = new Vector3(newPipeUp.transform.localScale.x, randomSize1);
            }

            displayScore.text = "Score: " + playerScore.ToString(); // Always update this score


            /*
            if (timeCountPipeUp >= randomTimePipeUp)
            {
                timeCountPipeUp = 0;
                randomTimePipeUp = (Random.value * 2.0f) + 1; // 1 a 3 segundos
                Instantiate(pipeUp, new Vector2(4.0f, 3.0f), Quaternion.identity);
            }
            */
        }
    }

    public void addScore()
    {
        playerScore+=0.5f; // THis is gonna be called twice, since I'm too lazy to find a way to only call it once while using the same script for both of the pipes, so the lazy way to solve it is like this
    }
}
