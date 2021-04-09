using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    private Rigidbody2D rbGround;
    private float groundVelocity;
    // Start is called before the first frame update
    void Start()
    {
        //rbGround = GetComponent<Rigidbody2D>();
        //groundVelocity = -2.0f;
    }

    // Update is called once per frame
    void Update()
    {/*
        rbGround.velocity = new Vector2(groundVelocity, 0);
        if(rbGround.transform.position.x <= -0.71f)
        {
            rbGround.transform.position = new Vector3(1.04f, rbGround.transform.position.y, 0);
        }*/
    }
}
