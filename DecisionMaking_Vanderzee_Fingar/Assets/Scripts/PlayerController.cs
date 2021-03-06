﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = .05f;
    public float minX = -3f;
    public float maxX = 3f;


    private bool goingLeft = false;
    private bool goingRight = false;
    

    public Transform shot;
    public bool ableToShoot = true;

    public Vector3 startPos;

    public Text text;
    public int generation = 0;

	// Use this for initialization
	void Start () {
        startPos = transform.position;

        text.text = "Generation " + generation;
	}
	
	// Update is called once per frame
    /*
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            goingLeft = true;
            goingRight = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            goingLeft = false;
            goingRight = true;
        }
        else {
            goingLeft = false;
            goingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            shoot();
        }

        move();

	}*/



    void FixedUpdate() {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveRight();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            shoot();
        }

    }

    //Not currently being used
    public void die()
    {
        transform.position = startPos;
        ableToShoot = true;
        /*
        //Gameover
        if(lives == 0)
        {
            Debug.Log("GameOver, out of lives");

        }

        //Reset Player
        else
        {
            transform.position = startPos;
        //}*/

        GameObject.Find("Alien Manager").GetComponent<AlienController>().resetLevel();
    }

    //Used for testing
    void move() {
        if (goingLeft && transform.position.x > minX)
        {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x - speed, position.y);
        }
        else if (goingRight && transform.position.x < maxX) {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x + speed, position.y);
        }
    }

    public void moveLeft() {
        if (transform.position.x > minX) {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x - speed * 10, position.y);
        }
    }
    public void moveRight() {
        if (transform.position.x < maxX) {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x + speed * 10, position.y);
        }
    }

    public void shoot() {
        if (ableToShoot)
        {
            Instantiate(shot, transform.position, Quaternion.identity);
            ableToShoot = false;
        }
    }


}
