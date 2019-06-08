using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemySquads;
    public int maxSquadsOnScreen;

    private int currentSquadsOnScreen;
    private PlayerController playerController;
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
    }

    // Update is called once per frame
    void Update()
    {
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
                        Destroy(parentOfEnemy);
                        enemySquads[i] = null;
                        currentSquadsOnScreen--;
                    }
                }
            }
            
        }
    }
}
