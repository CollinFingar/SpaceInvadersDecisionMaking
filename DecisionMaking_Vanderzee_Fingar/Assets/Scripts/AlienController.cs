using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlienController : MonoBehaviour {

    GameObject[] aliens;
    public int aliensAlive = 55;
    bool goingRight = true;
    public bool ableToShoot = true;

    public List<GameObject> shooters;

    public GameObject alien;
    public GameObject levelManager;
    public GameObject player;
    public Transform shot;

    public float leftBound = -3f;
    public float rightBound = 3f;
    public float bottomBound;
    public float topBound;

    public float alienStep;     //Value for how much an alien moves by
    public float distanceBetweenAliens;
    public float rowStep;
    public float goodShotRadius;

    public Text text;
    public int score = 0;   //Total score in the game

	// Use this for initialization
	void Start () {
        text.text = "SCORE: " + score;

        //Add shooter aliens
        //Select the aliens we want to shoot
        shooters = new List<GameObject>();

        //Second row, skip two, then every other alien add
        shooters.Add(aliens[13]);
        shooters.Add(aliens[15]);
        shooters.Add(aliens[17]);
        shooters.Add(aliens[19]);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate ()
    {
        move();
        shoot();
    }
    //Should be called in a fixed value of time that increases due to amount of aliens
    void move()
    {
        //Go Right
        if (goingRight)
        {
            //Check for bound
            if (findEndAlien(false) >= rightBound)
            {
                //Move down a row
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x, aliens[i].transform.position.y - rowStep, aliens[i].transform.position.z);
                    }
                }
                goingRight = false;

                //if the group reached the bottom
                if(findEndAlien(true) <= bottomBound)
                {
                    Debug.Log("Game Over");
                }
            }

            else
            {
                //Move the group over
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x + alienStep, aliens[i].transform.position.y, aliens[i].transform.position.z);
                    }
                }
            }
        }
        
        //Go Left
        else
        {
            //Check for bound
            if (findEndAlien(false) <= leftBound)
            {
                //Move down a row
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x, aliens[i].transform.position.y - rowStep, aliens[i].transform.position.z);
                    }
                }
                goingRight = true;

                //if the group reached the bottom
                if (findEndAlien(true) <= bottomBound)
                {
                    Debug.Log("Game Over");
                }
            }

            else
            {
                //Move the group over
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x - alienStep, aliens[i].transform.position.y, aliens[i].transform.position.z);
                    }
                }
            }
        }
        
    }

    //Called every fixed update to shoot if we can
    void shoot()
    {
        shooters.TrimExcess();

        //If they aren't all dead
        if(shooters.Count > 0)
        {
            for(int i = 0; i < shooters.Count; i++)
            {
                if (shooters[i] != null)
                {
                    //Check if the player is in a radius of a good shot
                    float distance = (Mathf.Abs(shooters[i].transform.position.x - player.transform.position.x));

                    if (distance <= goodShotRadius && ableToShoot)
                    {
                        Instantiate(shot, shooters[i].transform.position, Quaternion.identity);
                        ableToShoot = false;
                    }
                }
            }
        }
    }

    //Used to find the bound of the group
    float findEndAlien(bool wantbottom)
    {
        float bound = Mathf.Infinity;

        //Want left or right
        if (!wantbottom)
        {
            //Want left bound
            if (!goingRight)
            {
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        if (aliens[i].transform.position.x < bound)
                        {
                            bound = aliens[i].transform.position.x;
                        }
                    }
                }
            }

            //Want right bound
            else
            {
                bound = -Mathf.Infinity;
                for (int i = 0; i < aliens.Length; i++)
                {
                    if (aliens[i] != null)
                    {
                        if (aliens[i].transform.position.x > bound)
                        {
                            bound = aliens[i].transform.position.x;
                        }
                    }
                }
            }
        }
        //Want bottom
        else
        {
            for (int i = 0; i < aliens.Length; i++)
            {
                if (aliens[i] != null)
                {
                    if (aliens[i].transform.position.y < bound)
                    {
                        bound = aliens[i].transform.position.y;
                    }
                }
            }
        }

        return bound;
    }

    //Called from shotcontroller. Kill alien that was hit.
    public void destroyAlien(GameObject alien)
    {
        Destroy(alien);
        aliensAlive--;
        score+=100;
        text.text = "SCORE: " + score;

        //No aliens left. You win
        if(aliensAlive <= 0)
        {
            Debug.Log("You Win!");
            score += 500;
            text.text = "SCORE: " + score;
            levelManager.GetComponent<LevelController>().buildLevel();
        }
    }

    //Called from Level Controller when a new level starts
    public void resetLevel()
    {
        aliensAlive = 55;
        aliens = new GameObject[aliensAlive];
        goingRight = true;

        //Instantiate new aliens at left most bound moving right
        float rowValue = topBound;
        float colValue = leftBound;
        for(int i = 0; i < aliensAlive; i++)
        {
            //New Row
            if(i%11 == 0 && i > 0)
            {
                //Go down one row
                rowValue -= distanceBetweenAliens;
                colValue = leftBound;
            }
            GameObject anAlien = Instantiate(alien, new Vector3(colValue, rowValue, 0f), Quaternion.identity) as GameObject;
            aliens[i] = anAlien;
            anAlien.transform.parent = GameObject.Find("Aliens").transform;

            //Increment col space
            colValue += distanceBetweenAliens;
        }
    }
}
