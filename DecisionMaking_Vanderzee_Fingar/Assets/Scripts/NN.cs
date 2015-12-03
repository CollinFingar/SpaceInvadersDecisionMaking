using UnityEngine;
using System.Collections;

public class NN : MonoBehaviour {

    public GameObject player;
    public GameObject alienManager;

    private PlayerController pc;
    private AlienController ac;

    public float enemyShotAboveMoveWeight = 10f;
    public float enemyShotAboveShootWeight = 3f;

    public float enemyShotFarMoveWeight = 3f;
    public float enemyShotFarShootWeight = 6f;

    public float enemyAboveMoveWeight = 3f;
    public float enemyAboveShootWeight = 8f;

    public float enemyNotAboveMoveWeight = 8f;
    public float enemyNotAboveShootWeight = 1f;

    public float closeToEdgeMoveWeight = 5f;

    private bool enemyShotAbove = false;
    private bool enemyShotFar = false;
    private bool enemyAbove = false;
    private bool enemyNotAbove = false;
    private bool closeToEdge = false;

    private bool closeToRight = false;
    private bool nearestEnemyRight = false;

    private float moveLeft = 0f;
    private float moveRight = 0f;
    private float shoot = 0f;

    public float tooCloseToEdge = .5f;
    public float enemyShotClose = .5f;
    public float enemyClose = .5f;
    
    private float pScore = 0;
    private int previousTimesMoved = 0;
    private int previousTimesShot = 0;
    private int timesMoved = 10;
    private int timesShot = 10;

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
        enemyShotFar = false;
        enemyAbove = false;
        enemyNotAbove = false;
        closeToEdge = false;

        closeToRight = false;
        nearestEnemyRight = false;


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
            float closestShot = Mathf.Infinity;
            for (int i = 0; i < shots.Length; i++) {
                float distance = Mathf.Abs(shots[i].gameObject.transform.position.x - pc.transform.position.x);
                if (distance < enemyShotClose) {
                    enemyShotAbove = true;
                } else {
                    enemyShotFar = true;
                }
                if (distance < closestShot) {
                    closestShot = distance;
                }
            }
        }

        //===Enemy Alien===
        AlienController[] aliens = FindObjectsOfType<AlienController>();
        //If there is at least one shot
        if (aliens.Length != 0)
        {
            float closestAlien = Mathf.Infinity;
            for (int i = 0; i < aliens.Length; i++)
            {
                float distance = Mathf.Abs(aliens[i].gameObject.transform.position.x - pc.transform.position.x);
                if (distance < enemyClose){
                    enemyAbove = true;
                }
                else{
                    enemyNotAbove = true;
                }
                if (distance < closestAlien){
                    closestAlien = distance;
                    if(aliens[i].gameObject.transform.position.x > transform.position.x) {
                        nearestEnemyRight = true;
                    }
                }
            }
        }
    }

    void addWeights() {

        if (enemyShotAbove) {
            moveLeft += enemyShotAboveMoveWeight;
            moveRight += enemyShotAboveMoveWeight;
            shoot += enemyShotAboveShootWeight;
        } else if (enemyShotFar) {
            //moveLeft += enemyShotFarMoveWeight;
            //moveRight += enemyShotFarMoveWeight;
            //shoot += enemyShotFarShootWeight;
        }

        if (enemyAbove) {
            shoot += enemyAboveShootWeight;
            if (nearestEnemyRight){
                moveRight += enemyAboveMoveWeight;
            }
            else {
                moveLeft += enemyAboveMoveWeight;
            }
        } else if (enemyNotAbove) {
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
            timesShot++;
            pc.shoot();
        }         //We should move left
        else if (largest == moveLeft)
        {
            timesMoved++;
            pc.moveLeft();
        }

        //We should move right
        else
        {
            timesMoved++;
            pc.moveRight();
        }
    }
    
    public void modifyWeights(bool wasShot) {
        if (wasShot) {
            enemyShotAboveMoveWeight++;
        }
        else if (ac.score > pScore){
            if (timesMoved >= previousTimesMoved) {
                increaseMoveWeight();
            } else {
                decreaseMoveWeight();
            }
            if (timesShot > previousTimesShot) {
                increaseShootWeight();
            } else {
                decreaseShootWeight();
            }
        } else if (ac.score == pScore) {
            float rand = Random.Range(1, 10);
            if (rand > 5) {
                increaseMoveWeight();
            } else {
                increaseShootWeight();
            }
        } else {
            if (timesMoved >= previousTimesMoved) {
                decreaseMoveWeight();
            } else {
                increaseMoveWeight();
            }
            if (timesShot >= previousTimesShot) {
                decreaseShootWeight();
            } else {
                increaseShootWeight();
            }
        }
        previousTimesShot = timesShot;
        timesShot = 0;
        previousTimesMoved = timesMoved;
        timesMoved = 0;
        pScore = ac.score;
    }

    void increaseMoveWeight() {
        enemyShotAboveMoveWeight += 1;
        enemyAboveMoveWeight += 1;
        closeToEdgeMoveWeight += 1;
        enemyShotFarMoveWeight += 1;
        enemyNotAboveMoveWeight += 1;
    }
    void increaseShootWeight() {
        enemyAboveShootWeight += 1;
        enemyNotAboveShootWeight += 1;
        enemyShotAboveShootWeight += 1;
        enemyShotFarShootWeight += 1;
    }
    void decreaseMoveWeight() {
        enemyShotAboveMoveWeight -= 1;
        enemyAboveMoveWeight -= 1;
        closeToEdgeMoveWeight -= 1;
        enemyShotFarMoveWeight -= 1;
        enemyNotAboveMoveWeight -= 1;
    }
    void decreaseShootWeight() {
        enemyAboveShootWeight -= 1;
        enemyNotAboveShootWeight -= 1;
        enemyShotAboveShootWeight -= 1;
        enemyShotFarShootWeight -= 1;
    }
}
