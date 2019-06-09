﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void onGameAction();
    public static onGameAction onRoundEnd;

    public GameObject player;
    public GameObject[] enemySquads;
    public GameObject distanceTextGameObject;
    public GameObject roundEndUI;
    public string nextSceneName;
    public float waitingTime;
    public int maxSquadsOnScreen;
    public float distance;

    private float waitingTimer;
    private int currentSquadsOnScreen;
    private PlayerController playerController;
    private DisplayNumbers distanceText;
    private RoundEnd roundEndMessage;
    private bool endRoundOnce;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemySquads.Length; i++)
        {
            enemySquads[i].GetComponent<Enemy>().deActivateWaypoint();
            for (int f= 0; f < enemySquads[i].GetComponent<Enemy>().squadMembers.Count; f++)
            {
                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().onEnemyDeath += deleteEnemyFromSquad;
                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().onEnemyDeath += addPointsToPlayer;
            }
            
            enemySquads[i].SetActive(false);
        }

        playerController = player.GetComponent<PlayerController>();
        distanceText = distanceTextGameObject.GetComponent<DisplayNumbers>();
        roundEndMessage = roundEndUI.GetComponent<RoundEnd>();
        PlayerController.onPlayerDeath += roundEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if(distance <= 0)
        {
            if(!endRoundOnce)
            {
                roundEnd();
                endRoundOnce = true;
            }
            waitingTimer += Time.deltaTime;
            checkNextScene();
        }
        else
        {
            distance -= Time.deltaTime;
            distanceText.number = distance;
        }

        for (int i = 0; i < enemySquads.Length; i++) //TEST, AVOID REPETEAING THE FOR EVERY FRAME.
        {
            if (enemySquads[i] != null && currentSquadsOnScreen < maxSquadsOnScreen)
            {
                enemySquads[i].SetActive(true);
                enemySquads[i].GetComponent<Enemy>().activateWaypoint();
                currentSquadsOnScreen++;
            }
        }
    }

    private void addPointsToPlayer(GameObject enemy)
    {
        playerController.addPoints();
    }

    private void deleteEnemyFromSquad(GameObject enemy)
    {
        GameObject parentOfEnemy = enemy.transform.parent.gameObject;

        for (int i = 0; i < enemySquads.Length; i++)
        {
            if(enemySquads[i] != null)
            {
                if (parentOfEnemy.name == enemySquads[i].name)
                {
                    parentOfEnemy.GetComponent<Enemy>().squadMembers.Remove(enemy);

                    if (parentOfEnemy.GetComponent<Enemy>().squadMembers.Count <= 0)
                    {
                        enemy.transform.SetParent(gameObject.transform);
                        Destroy(parentOfEnemy);
                        enemySquads[i] = null;
                        currentSquadsOnScreen--;
                    }
                }
            }
            
        }
    }

    private void roundEnd()
    {
        Debug.Log("Round ended");
        if(onRoundEnd != null)
        {
            onRoundEnd();
        }

        for (int i = 0; i < enemySquads.Length; i++)
        {
            if(enemySquads[i] != null)
            {
                if (enemySquads[i].activeSelf)
                {
                    for (int f = 0; f < enemySquads[i].GetComponent<Enemy>().squadMembers.Count; f++)
                    {
                        if (enemySquads[i].GetComponent<Enemy>().squadMembers.Count > 0)
                        {
                            if(enemySquads[i].GetComponent<Enemy>().squadMembers[f] != null)
                            {
                                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().killEnemy();
                            }
                        }
                    }
                }
            }
        }

        distanceTextGameObject.SetActive(false);

        if(distance <= 0)
        {
            roundEndMessage.display("You Passed the Level!", Color.green);
        }
        else
        {
            roundEndMessage.display("You died!", Color.red);
        }
    }

    private void checkNextScene()
    {
        if (waitingTimer >= waitingTime)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnDestroy()
    {
        PlayerController.onPlayerDeath -= roundEnd;
    }
}
