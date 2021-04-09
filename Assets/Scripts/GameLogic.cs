using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameLogic : MonoBehaviour
{

    public float randomTimePipeDown = 0.0f;
    public float timeCountPipeDown = 0.0f;    
    public float randomTimePipeUp = 0.0f;
    public float timeCountPipeUp = 0.0f;
    public GameObject pipeDown;
    public GameObject pipeUp;
    public float randomSize1;
    public float randomSize2;

    public int playerScore;
    GameObject txtScore;
    Text displayScore;

    GameObject objPlayer;
    GameObject objProcessLayer;

    PostProcessVolume volume;
    ColorGrading colorGradingLayer;


    private bool stopEverything;

    public bool getStopEverything()
    {
        return stopEverything;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        txtScore = GameObject.Find("Text");
        objPlayer = GameObject.Find("Player");
        objProcessLayer = GameObject.Find("ProcessLayer");
        displayScore = txtScore.GetComponent<Text>();
        displayScore.text = playerScore.ToString();
        stopEverything = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objPlayer.GetComponent<PlayerBehaviour>().getDead())
        {
            stopEverything = true;
            volume = objProcessLayer.GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings<ColorGrading>(out colorGradingLayer);
            colorGradingLayer.saturation.value = -100.0f;
        }
        if (stopEverything)
        {
            displayScore.text = playerScore.ToString() + "\nPress R to Restart!";
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
                //Application.LoadLevel(0);
            }
        }
        else
        {
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

            displayScore.text = playerScore.ToString(); // Always update this score


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
        playerScore++;
    }
}
