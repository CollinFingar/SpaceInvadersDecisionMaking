using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

    public float speed = .05f;
    public float maxHeight = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > maxHeight) {
            Destroy(gameObject);
        }

        Vector2 position = transform.position;
        transform.position = new Vector2(position.x, position.y + speed);
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "alien")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "wall") {
            Wall script = other.gameObject.GetComponent<Wall>();
            script.lowerHealth();
            Destroy(gameObject);
        }
        
    }
}
