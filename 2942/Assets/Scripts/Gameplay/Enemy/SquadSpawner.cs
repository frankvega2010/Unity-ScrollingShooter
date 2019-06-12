using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour
{
    public List<GameObject> squadList;
    public GameObject GameManagerGameObject;
    public GameObject newParent;

    private int randomIndex;
    private int lastIndex;
    private GameManager gameManager;
    // Start is called before the first frame update
    private void Awake()
    {
        lastIndex = -1;
        gameManager = GameManagerGameObject.GetComponent<GameManager>();
    }

    public void addSquadToLevelSquads()
    {
        for (int i = 0; i < gameManager.enemySquads.Length; i++)
        {
            randomIndex = Random.Range(0, squadList.Count);

            if(randomIndex == lastIndex)
            {
                i--;
            }
            else
            {
                GameObject newSquad = Instantiate(squadList[randomIndex]);
                newSquad.transform.SetParent(newParent.transform, false);
                newSquad.SetActive(true);
                newSquad.transform.GetChild(0).gameObject.name = "Squad " + i;
                gameManager.enemySquads[i] = newSquad.transform.GetChild(0).gameObject;
                lastIndex = randomIndex;
            }
        }
    }
}
