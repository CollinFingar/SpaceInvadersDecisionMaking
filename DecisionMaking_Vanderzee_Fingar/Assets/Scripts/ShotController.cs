using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

    public float speed = .05f;
    public float maxHeight = 5f;
    public float minHeight = -5f;

    public GameObject alienManager;
    public GameObject playerManager;

    public bool fromAlien = true;

	// Use this for initialization
	void Start () {
        alienManager = GameObject.Find("Alien Manager");
        playerManager = GameObject.Find("player");

        
	}
	
	// Update is called once per frame
	void Update () {

        //Shot came from player
        if (!fromAlien)
        {
            if (transform.position.y > maxHeight)
            {
                playerManager.GetComponent<PlayerController>().ableToShoot = true;
                Destroy(gameObject);
            }


            Vector2 position = transform.position;
            transform.position = new Vector2(position.x, position.y + speed);
        }

        //From alien
        else
        {
            if (transform.position.y < minHeight)
            {
                alienManager.GetComponent<AlienController>().ableToShoot = true;
                Destroy(gameObject);
            }

            Vector2 position = transform.position;
            transform.position = new Vector2(position.x, position.y - speed);
        }
	}

    void OnTriggerEnter2D(Collider2D other) {

        //Shot from player
        if (!fromAlien)
        {
            //Kill alien
            if (other.gameObject.tag == "alien")
            {
                playerManager.GetComponent<PlayerController>().ableToShoot = true;
                alienManager.GetComponent<AlienController>().destroyAlien(other.gameObject);
                Destroy(gameObject);

            }
            //Break wall
            else if (other.gameObject.tag == "wall")
            {
                playerManager.GetComponent<PlayerController>().ableToShoot = true;
                other.gameObject.GetComponent<Wall>().lowerHealth();
                Destroy(gameObject);

            }
        }

        //Shot from alien
        else
        {

            //Kill Player
            if (other.gameObject.tag == "Player")
            {
                alienManager.GetComponent<AlienController>().ableToShoot = true;
                playerManager.GetComponent<PlayerController>().die();
                Destroy(gameObject);
            }


            //Break wall
            else if (other.gameObject.tag == "wall")
            {
                playerManager.GetComponent<PlayerController>().ableToShoot = true;
                other.gameObject.GetComponent<Wall>().lowerHealth();
                Destroy(gameObject);

            }
        }
    }
}
