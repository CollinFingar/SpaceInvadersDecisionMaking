using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public int health = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void lowerHealth() {
        if (health == 3)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .66f);
            health--;
        }
        else if (health == 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .33f);
            health--;
        }
        else {
            //Destroy(gameObject);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void reset()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        health = 3;
    }

}
