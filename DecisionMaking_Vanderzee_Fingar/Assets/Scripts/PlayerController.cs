using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speed = .05f;
    public float minX = -3f;
    public float maxX = 3f;

    private bool goingLeft = false;
    private bool goingRight = false;

    public Transform shot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
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

    void shoot() {
        Instantiate(shot, transform.position, Quaternion.identity);
    }


}
