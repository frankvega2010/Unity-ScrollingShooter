using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void onGameAction();
    public static onGameAction onRoundEnd;

    public GameObject player;
    public GameObject[] enemySquads;
    public GameObject distanceTextGameObject;
    public GameObject roundEndUI;
    public GameObject startRoundCanvas;
    public GameObject endRoundCanvas;

    public float roundStartCountdown;
    public string nextSceneName;
    public float waitingTime;
    public int maxSquadsOnScreen;
    public float nextSquadDelay;
    public float distance;

    private float waitingTimer;
    private float roundStartTimer;
    public int currentSquadsOnScreen;
    private PlayerController playerController;
    private DisplayNumbers distanceText;

    private Animator startRoundAnimator;
    private Animator endRoundAnimator;
    private RoundEnd roundEndMessage;
    private SaveLastLevel saveLevelName;
    private bool endRoundOnce;
    private bool startRoundOnce;
    private bool playerDied;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemySquads.Length; i++)
        {
            enemySquads[i].GetComponent<Enemy>().deActivateWaypoint();
            for (int f= 0; f < enemySquads[i].GetComponent<Enemy>().squadMembers.Count; f++)
            {
                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().onEnemyDeath += deleteEnemyFromSquad;
                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().onEnemyDestroyed += deleteEnemyFromSquad;
                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().onEnemyDeath += addPointsToPlayer;
            }
            
            enemySquads[i].SetActive(false);
        }

        playerController = player.GetComponent<PlayerController>();
        distanceText = distanceTextGameObject.GetComponent<DisplayNumbers>();
        roundEndMessage = roundEndUI.GetComponent<RoundEnd>();
        startRoundAnimator = startRoundCanvas.GetComponent<Animator>();
        endRoundAnimator = endRoundCanvas.GetComponent<Animator>();
        saveLevelName = SaveLastLevel.Get();


        PlayerController.onPlayerDeath += roundEnd;
        PlayerController.onPlayerDeath += playerIsDead;
        currentSquadsOnScreen = 2;

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

    // Update is called once per frame
    void Update()
    {
        if(distance <= 0)
        {
            if(!endRoundOnce)
            {
                endRoundOnce = true;
                roundEnd();
            }
            waitingTimer += Time.deltaTime;
            checkNextScene();
        }
        else
        {
            distance -= Time.deltaTime;
            distanceText.number = distance;
            if (!startRoundOnce)
            {
                roundStartTimer += Time.deltaTime;
            } 

            if(roundStartTimer >= roundStartCountdown)
            {
                if(!startRoundOnce)
                {
                    currentSquadsOnScreen = 0;
                    startRoundAnimator.SetBool("canSwitch", true);
                    spawnNextSquad();
                    startRoundOnce = true;
                }
                
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
                    enemy.transform.SetParent(gameObject.transform);

                    if (parentOfEnemy.GetComponent<Enemy>().squadMembers.Count <= 0)
                    {
                        Destroy(parentOfEnemy);
                        enemySquads[i] = null;
                        Invoke("substractCurrentSquadsOnScreen", nextSquadDelay);
                    }
                }
            }
            
        }  
    }

    private void substractCurrentSquadsOnScreen()
    {
        currentSquadsOnScreen--;
        spawnNextSquad();
    }

    private void spawnNextSquad()
    {
        for (int i = 0; i < enemySquads.Length; i++) //TEST, AVOID REPETEAING THE FOR EVERY FRAME.
        {
            if (enemySquads[i] != null && !enemySquads[i].activeSelf && currentSquadsOnScreen < maxSquadsOnScreen)
            {
                enemySquads[i].SetActive(true);
                enemySquads[i].GetComponent<Enemy>().activateWaypoint();
                currentSquadsOnScreen++;
            }
        }
    }

    private void playerIsDead()
    {
        playerDied = true;
    }

    private void roundEnd()
    {
        Debug.Log("Round ended");
        endRoundOnce = true;
        distance = 0;
        endRoundAnimator.SetBool("canSwitch", true);
        if (onRoundEnd != null)
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
                                enemySquads[i].GetComponent<Enemy>().squadMembers[f].transform.SetParent(gameObject.transform);
                                enemySquads[i].GetComponent<Enemy>().squadMembers[f].GetComponent<Enemy>().killEnemy();
                            }
                        }
                    }
                }
            }
        }

        //distanceTextGameObject.SetActive(false);

        if(!playerDied)
        {
            roundEndMessage.display("You Passed the Level!", Color.green);
        }
        else
        {
            roundEndMessage.display("You died!", Color.red);
        }

        saveLevelName.saveLevelName(SceneManager.GetActiveScene().name);
    }

    private void checkNextScene()
    {
        if (waitingTimer >= waitingTime)
        {
            if (!playerDied)
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void OnDestroy()
    {
        PlayerController.onPlayerDeath -= roundEnd;
        PlayerController.onPlayerDeath -= playerIsDead;
    }
}
