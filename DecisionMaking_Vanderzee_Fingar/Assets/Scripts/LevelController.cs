using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    int currentLevel = 0;

    public GameObject alienController;
    public GameObject playerController;

    public float score = 0f;


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

        //Increment Level number
        currentLevel++;
       
        
    }

    //Should keep track of data before resetting. Happens when player dies
    public void resetGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
