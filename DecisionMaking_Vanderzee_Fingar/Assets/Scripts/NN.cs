using UnityEngine;
using System.Collections;

public class NN : MonoBehaviour {

    public GameObject player;
    public GameObject alienManager;

    private PlayerController pc;
    private AlienController ac;

    public float enemyShotAboveMoveWeight = 10f;
    public float enemyShotAboveShootWeight = 3f;

    public float enemyAboveMoveWeight = 3f;
    public float enemyAboveShootWeight = 8f;

    public float enemyNotAboveMoveWeight = 8f;
    public float enemyNotAboveShootWeight = 1f;

    public float closeToEdgeMoveWeight = 5f;

    public bool enemyShotAbove = false;
    //public bool enemyShotFar = false;
    public bool enemyAbove = false;
    //public bool enemyNotAbove = false;
    public bool closeToEdge = false;

    private bool closeToRight = false;
    public bool nearestEnemyRight = false;
    public bool nearestShotRight = false;

    private float moveLeft = 0f;
    private float moveRight = 0f;
    private float shoot = 0f;

    public float tooCloseToEdge = .5f;
    public float enemyShotClose = .5f;
    public float enemyClose = .5f;
    
    private float pScore = 0;
    private int previousTimesMoved = 0;
    private int previousTimesShot = 0;
    // private int timesMoved = 10;
    // private int timesShot = 10;
    private int timesMovedShotAbove = 0;
    private int timesMovedEnemyAbove = 0;
    private int timesMovedEnemyNotAbove = 0;
    private int timesShotEnemyShotAbove = 0;
    private int timesShotEnemyAbove = 0;
    private int timesShotEnemyNotAbove = 0;

    private int prevTimesMovedShotAbove = 0;
    private int prevTimesMovedEnemyAbove = 0;
    private int prevTimesMovedEnemyNotAbove = 0;
    private int prevTimesShotEnemyShotAbove = 0;
    private int prevTimesShotEnemyAbove = 0;
    private int prevTimesShotEnemyNotAbove = 0;

    private int maxWeight = 15;

    // Use this for initialization
    void Start () {
        pc = player.GetComponent<PlayerController>();
        ac = alienManager.GetComponent<AlienController>();
	}
	
	// Update is called once per frame
	void Update () {

        //Set bools according to conditions
        setBools();

        //Add up the weights of each condition
        addWeights();

        //Find and do the most strategic thing
        compareWeights();
	}


    void setBools() {
        enemyShotAbove = false;
        //enemyShotFar = false;
        enemyAbove = false;
        //enemyNotAbove = false;
        closeToEdge = false;

        closeToRight = false;
        nearestEnemyRight = false;
        nearestShotRight = false;

        //===Close to Edge===
        if (pc.gameObject.transform.position.x < pc.minX + tooCloseToEdge) {
            closeToEdge = true;
            closeToRight = false;
        } else if(pc.gameObject.transform.position.x > pc.maxX - tooCloseToEdge) {
            closeToEdge = true;
            closeToRight = true;
        } else {
            closeToEdge = false;
            closeToRight = false;
        }
        //===Enemy Bullet===
        ShotControllerAlien[] shots = FindObjectsOfType<ShotControllerAlien>();
            //If there is at least one shot
        if (shots.Length != 0) {
            //float closestShot = Mathf.Infinity;
            for (int i = 0; i < shots.Length; i++) {
                float distance = Mathf.Abs(shots[i].gameObject.transform.position.x - pc.gameObject.transform.position.x);
                if (distance < enemyShotClose) {
                    enemyShotAbove = true;
                    if (shots[i].gameObject.transform.position.x > pc.gameObject.transform.position.x) {
                        nearestShotRight = true;
                    }
                } 
            }
        }

        //===Enemy Alien===
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("alien");
        //If there is at least one shot
        if (aliens.Length != 0)
        {
            float closestAlien = Mathf.Infinity;
            for (int i = 0; i < aliens.Length; i++)
            {
                AlienController aCont = aliens[i].GetComponent<AlienController>();
                float distance = Mathf.Abs(aliens[i].transform.position.x - pc.gameObject.transform.position.x);
                if (distance < enemyClose){
                    enemyAbove = true;
                }
                if (distance < closestAlien){
                    closestAlien = distance;
                    if(aliens[i].transform.position.x > pc.gameObject.transform.position.x) {
                        nearestEnemyRight = true;
                    }
                    else {
                        nearestEnemyRight = false;
                    }
                }
            }
        }
    }

    void addWeights() {

        if (enemyShotAbove) {
            if (nearestShotRight) {
                moveLeft += enemyShotAboveMoveWeight;
            } else {
                moveRight += enemyShotAboveMoveWeight;
            }
            shoot += enemyShotAboveShootWeight;
        } 

        if (enemyAbove) {
            shoot += enemyAboveShootWeight;
            if (nearestEnemyRight){
                moveRight += enemyAboveMoveWeight;
            }
            else {
                moveLeft += enemyAboveMoveWeight;
            }
        } else if (!enemyAbove) {
            shoot += enemyNotAboveShootWeight;
            if (nearestEnemyRight) {
                moveRight += enemyNotAboveMoveWeight;
            } else {
                moveLeft += enemyNotAboveMoveWeight;
            }
        }

        if (closeToEdge) {
            if (closeToRight) {
                moveLeft += closeToEdgeMoveWeight;
            } else {
                moveRight += closeToEdgeMoveWeight;
            }
        }
    }

    void compareWeights()
    {
        //Compare the three weights for largest.
        float largest = Mathf.Max(shoot, moveLeft, moveRight);

        //We should shoot
        if(largest == shoot)
        {
            //Find the most contributing factor to the shot
            float hightestWeight = Mathf.Max(enemyAboveShootWeight, enemyNotAboveShootWeight, enemyShotAboveShootWeight);
            if(hightestWeight == enemyAboveShootWeight)
            {
                timesShotEnemyAbove++;
            }

            else if (hightestWeight == enemyNotAboveShootWeight)
            {
                timesShotEnemyNotAbove++;
            }

            else
            {
                timesShotEnemyShotAbove++;
            }

            pc.shoot();
        }         
        
        //We should move left
        else if (largest == moveLeft)
        {

            //Find most contributing factor to the move
            float hightestWeight = Mathf.Max(enemyAboveMoveWeight, enemyNotAboveMoveWeight, enemyShotAboveMoveWeight);

            //Close to edge
            if (closeToEdge)
            {
                if (closeToRight)
                {
                    //timesMoved++;
                    pc.moveLeft();
                }

                else
                {
                    pc.shoot();
                }
            }

            //Not on edge. Move normally
            else
            {
                if(hightestWeight == enemyAboveMoveWeight)
                {
                    timesMovedEnemyAbove++;
                }

                else if (hightestWeight == enemyNotAboveMoveWeight)
                {
                    timesMovedEnemyNotAbove++;
                }

                else
                {
                    timesMovedShotAbove++;
                }

                pc.moveLeft();
            }
            
        }

        //We should move right
        else
        {
            //Find most contributing factor to the move
            float hightestWeight = Mathf.Max(enemyAboveMoveWeight, enemyNotAboveMoveWeight, enemyShotAboveMoveWeight);

            //Close to edge
            if (closeToEdge)
            {
                if (closeToRight)
                {
                    pc.shoot();
                }
                else
                {
                    //timesMoved++;
                    pc.moveRight();
                }
            }

            //Not close to edge. move normally
            else
            {
                if (hightestWeight == enemyAboveMoveWeight)
                {
                    timesMovedEnemyAbove++;
                }

                else if (hightestWeight == enemyNotAboveMoveWeight)
                {
                    timesMovedEnemyNotAbove++;
                }

                else
                {
                    timesMovedShotAbove++;
                }
                pc.moveRight();
            }
        }

        moveLeft = 0;
        moveRight = 0;
        shoot = 0;
    }
    
    public void modifyWeights(bool wasShot) {
       if (wasShot) {
            //enemyShotAboveMoveWeight++;
        }
        /*if(timesShot == 0) {
            enemyAboveShootWeight += 1;
        }*/

        //If our score is greater this time
        if (ac.score > pScore)
        {

            //Move weights check
            if (timesMovedEnemyAbove >= prevTimesMovedEnemyAbove && timesMovedEnemyAbove < maxWeight)
            {
                enemyAboveMoveWeight++;
            }
            else
            {
                enemyAboveMoveWeight--;
            }

            if (timesMovedEnemyNotAbove >= prevTimesMovedEnemyNotAbove && timesMovedEnemyNotAbove < maxWeight)
            {
                enemyNotAboveMoveWeight++;
            }
            else
            {
                enemyNotAboveMoveWeight--;
            }

            if (timesMovedShotAbove >= prevTimesMovedShotAbove && timesMovedShotAbove < maxWeight)
            {
                enemyShotAboveMoveWeight++;
            }
            else
            {
                enemyShotAboveMoveWeight--;
            }

            //Shoot weights check
            if (timesShotEnemyAbove >= prevTimesShotEnemyAbove && timesShotEnemyAbove < maxWeight)
            {
                enemyAboveShootWeight++;
            }
            else
            {
                enemyAboveShootWeight--;
            }

            if (timesShotEnemyNotAbove >= prevTimesShotEnemyNotAbove && timesShotEnemyNotAbove < maxWeight)
            {
                enemyNotAboveShootWeight++;
            }
            else
            {
                enemyNotAboveShootWeight--;
            }

            if (timesShotEnemyShotAbove >= prevTimesShotEnemyShotAbove && timesShotEnemyShotAbove< maxWeight)
            {
                enemyShotAboveShootWeight++;
            }
            else
            {
                enemyShotAboveShootWeight--;
            }
        }


        //Our score is equal to last one 
        else if (ac.score == pScore)
        {
            float rand = Random.Range(1, 10);
            if (rand > 5)
            {
                if (rand <= 7 && enemyAboveMoveWeight < maxWeight)
                {
                    enemyAboveMoveWeight++;
                }
                else if (rand == 8 && enemyNotAboveMoveWeight < maxWeight)
                {
                    enemyNotAboveMoveWeight++;
                }
                else if (enemyShotAboveMoveWeight < maxWeight)
                {
                    enemyShotAboveMoveWeight++;
                }
            }
            else
            {
                if ( rand <= 3 && enemyAboveShootWeight < maxWeight)
                {
                    enemyAboveShootWeight++;
                }
                else if (rand == 4 && enemyNotAboveShootWeight < maxWeight)
                {
                    enemyNotAboveShootWeight++;
                }
                if (rand == 5 && enemyShotAboveShootWeight < maxWeight)
                {
                    enemyShotAboveShootWeight++;
                }
            }
        }

        //Score is less
        else
        {
            //Move weights check
            if (timesMovedEnemyAbove >= prevTimesMovedEnemyAbove)
            {
                enemyAboveMoveWeight--;
            }
            else if(enemyAboveMoveWeight < maxWeight)
            {
                enemyAboveMoveWeight++;
            }

            if (timesMovedEnemyNotAbove >= prevTimesMovedEnemyNotAbove)
            {
                enemyNotAboveMoveWeight--;
            }
            else if(enemyNotAboveMoveWeight < maxWeight)
            {
                enemyNotAboveMoveWeight++;
            }

            if (timesMovedShotAbove >= prevTimesMovedShotAbove)
            {
                enemyShotAboveMoveWeight--;
            }
            else if(enemyShotAboveMoveWeight < maxWeight)
            {
                enemyShotAboveMoveWeight++;
            }

            //Shoot weights check
            if (timesShotEnemyAbove >= prevTimesShotEnemyAbove)
            {
                enemyAboveShootWeight--;
            }
            else if(enemyAboveShootWeight < maxWeight)
            {
                enemyAboveShootWeight++;
            }

            if (timesShotEnemyNotAbove >= prevTimesShotEnemyNotAbove)
            {
                enemyNotAboveShootWeight--;
            }
            else if(enemyNotAboveShootWeight < maxWeight)
            {
                enemyNotAboveShootWeight++;
            }

            if (timesShotEnemyShotAbove >= prevTimesShotEnemyShotAbove)
            {
                enemyShotAboveShootWeight--;
            }
            else if(enemyShotAboveShootWeight < maxWeight)
            {
                enemyShotAboveShootWeight++;
            }
        }
        //Reset variables
        prevTimesMovedEnemyAbove = timesMovedEnemyAbove;
        prevTimesMovedEnemyNotAbove = timesMovedEnemyNotAbove;
        prevTimesMovedShotAbove = timesMovedShotAbove;
        prevTimesShotEnemyAbove = timesShotEnemyAbove;
        prevTimesShotEnemyNotAbove = timesShotEnemyNotAbove;
        prevTimesShotEnemyShotAbove = timesShotEnemyShotAbove;

        timesMovedEnemyAbove = 0;
        timesMovedEnemyNotAbove = 0;
        timesMovedShotAbove = 0;
        timesShotEnemyAbove = 0;
        timesShotEnemyNotAbove = 0;
        timesShotEnemyShotAbove = 0;

        pScore = ac.score;
    }

    
}
