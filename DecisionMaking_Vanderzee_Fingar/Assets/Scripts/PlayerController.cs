using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speed = .05f;
    public float minX = -3f;
    public float maxX = 3f;

    //public int lives = 3;

    private bool goingLeft = false;
    private bool goingRight = false;
    

    public Transform shot;
    public bool ableToShoot = true;

    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
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

    public void die()
    {
        transform.position = startPos;
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

    void moveLeft() {
        if (transform.position.x > minX) {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x - speed * 10, position.y);
        }
    }
    void moveRight() {
        if (transform.position.x < maxX) {
            Vector2 position = transform.position;
            transform.position = new Vector2(position.x + speed * 10, position.y);
        }
    }

    void shoot() {
        if (ableToShoot)
        {
            GameObject aShot = Instantiate(shot, transform.position, Quaternion.identity) as GameObject;
            ableToShoot = false;
        }
    }


}
