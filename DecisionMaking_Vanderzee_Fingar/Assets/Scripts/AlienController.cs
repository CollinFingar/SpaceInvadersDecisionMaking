using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienController : MonoBehaviour {

    GameObject[] aliens;
    public int aliensAlive = 55;
    bool goingRight = true;

    public GameObject alien;
    public GameObject levelManager;

    public float leftBound = -3f;
    public float rightBound = 3f;
    public float bottomBound;
    public float topBound;

    public float alienStep;     //Value for how much an alien moves by
    public float distanceBetweenAliens;

    public int score = 0;   //Total score in the game

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate ()
    {
        move();
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
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x, aliens[i].transform.position.y - alienStep, aliens[i].transform.position.z);
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
                        aliens[i].transform.position = new Vector3(aliens[i].transform.position.x, aliens[i].transform.position.y - alienStep, aliens[i].transform.position.z);
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
        score++;

        //No aliens left. You win
        if(aliensAlive <= 0)
        {
            Debug.Log("You Win!");
            score += 20;
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
