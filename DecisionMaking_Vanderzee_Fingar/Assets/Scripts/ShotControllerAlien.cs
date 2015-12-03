using UnityEngine;
using System.Collections;

public class ShotControllerAlien : MonoBehaviour {

    public float speed = .05f;
    public float minHeight = -5f;

    public GameObject alienManager;
    public GameObject playerManager;

    // Use this for initialization
    void Start () {
        alienManager = GameObject.Find("Alien Manager");
        playerManager = GameObject.Find("player");
    }
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y < minHeight)
        {
            alienManager.GetComponent<AlienController>().ableToShoot = true;
            Destroy(gameObject);
        }

        Vector2 position = transform.position;
        transform.position = new Vector2(position.x, position.y - speed);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Kill Player
        if (other.gameObject.tag == "Player")
        {
            alienManager.GetComponent<AlienController>().ableToShoot = true;
            GameObject.Find("Level Manager").GetComponent<LevelController>().resetGame(true);
            Destroy(gameObject);
        }


        //Break wall
        else if (other.gameObject.tag == "wall")
        {
            alienManager.GetComponent<AlienController>().ableToShoot = true;
            other.gameObject.GetComponent<Wall>().lowerHealth();
            Destroy(gameObject);

        }
    }
}
