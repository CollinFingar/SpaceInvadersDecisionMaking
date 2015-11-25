using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    int currentLevel = 0;

    public GameObject alienController;
    public GameObject playerController;

	// Use this for initialization
	void Start () {
        alienController.GetComponent<AlienController>().resetLevel();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void buildLevel()
    {
        //Reset and build aliens
        alienController.GetComponent<AlienController>().resetLevel();

        //Increment Level number and add a life
        currentLevel++;
        playerController.GetComponent<PlayerController>().lives++;
        
    }
}
