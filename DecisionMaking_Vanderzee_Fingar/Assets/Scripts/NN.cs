using UnityEngine;
using System.Collections;

public class NN : MonoBehaviour {

    public GameObject player;
    public GameObject alienManager;

    private PlayerController pc;
    private AlienController ac;

    public float enemyShotAboveMoveWeight = 10f;
    public float enemyShotAboveShootWeight = 10f;

    public float enemyShotFarMoveWeight = 10f;
    public float enemyShotFarShootWeight = 10f;

    public float enemyAboveMoveWeight = 10f;
    public float enemyAboveShootWeight = 10f;

    public float enemyNotAboveMoveWeight = 10f;
    public float enemyNotAboveShootWeight = 10f;

    public float closeToEdgeMoveWeight = 10f;

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

    // Use this for initialization
    void Start () {
        pc = player.GetComponent<PlayerController>();
        ac = alienManager.GetComponent<AlienController>();
	}
	
	// Update is called once per frame
	void Update () {
        setBools();
        addWeights();
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
            moveLeft += enemyShotFarMoveWeight;
            moveRight += enemyShotFarMoveWeight;
            shoot += enemyShotFarShootWeight;
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




}
