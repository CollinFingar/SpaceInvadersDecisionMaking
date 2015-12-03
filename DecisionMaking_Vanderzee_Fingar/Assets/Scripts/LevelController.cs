using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    int currentLevel = 0;

    public GameObject alienController;
    public GameObject playerController;

    public float score = 0f;


    // Use this for initialization
    void Start()
    {
        alienController.GetComponent<AlienController>().resetLevel();
    }

    // Update is called once per frame
    void Update()
    {

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
        //Application.LoadLevel(Application.loadedLevel);
        GameObject theAliens = GameObject.Find("Aliens");

        //Delete current aliens
        for (int i = 0; i < theAliens.transform.childCount; i++)
        {
            Destroy(theAliens.transform.GetChild(i));
        }

        //Rebuild aliens
        alienController.GetComponent<AlienController>().resetLevel();

        //Rebuild walls
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].GetComponent<Wall>().reset();
        }

        //Reset Score and Level Number
        score = 0;
        currentLevel = 0;

        //Reset Player
        playerController.transform.position = playerController.GetComponent<PlayerController>().startPos;
        playerController.GetComponent<PlayerController>().ableToShoot = true;
    }
}