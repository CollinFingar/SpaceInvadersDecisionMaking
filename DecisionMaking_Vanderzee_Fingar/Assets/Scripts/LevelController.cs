using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    int currentLevel = 0;

    public GameObject alienController;
    public GameObject playerController;
    public GameObject neuralObject;

    private NN nn;

    public float score = 0f;


    // Use this for initialization
    void Start()
    {
        nn = neuralObject.GetComponent<NN>();
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
    public void resetGame(bool wasShot)
    {
        nn.modifyWeights(wasShot);
        alienController.GetComponent<AlienController>().score = 0;
        //Application.LoadLevel(Application.loadedLevel);
        GameObject theAliens = GameObject.Find("Aliens");

        //Delete current aliens
        for (int i = 0; i < theAliens.transform.childCount; i++)
        {
            Destroy(theAliens.transform.GetChild(i).gameObject);
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

        //Destory all shots
        GameObject[] shots = GameObject.FindGameObjectsWithTag("shot");
        for(int i = 0; i < shots.Length; i++)
        {
            Destroy(shots[i].gameObject);
        }
    }
}
