using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestormX : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject missileInstance;
    public GameObject soundObject;

    public List<GameObject> enemyShips;
    private GameManager gameManager;
    private AudioSource sound;
    private void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        sound = soundObject.GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (enemyShips.Count > 0)
        {
            enemyShips.Clear();
        }

        for (int i = 0; i < gameManager.enemySquads.Length; i++)
        {
            if (gameManager.enemySquads[i] != null && gameManager.enemySquads[i].activeSelf)
            {
                for (int f = 0; f < gameManager.enemySquads[i].GetComponent<Enemy>().squadMembers.Count; f++)
                {
                    enemyShips.Add(gameManager.enemySquads[i].GetComponent<Enemy>().squadMembers[f]);
                    //if (gameManager.enemySquads[i].GetComponent<Enemy>().squadMembers[f].activeSelf)
                    //{
                        
                    //}
                }
            }
        }

        foreach (GameObject enemy in enemyShips)
        {
            GameObject bullet = Instantiate(missileInstance, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            bullet.transform.position = GetComponent<Transform>().position;
            bullet.GetComponent<HomingMissile>().target = enemy;
            bullet.SetActive(true);
            sound.Play();
        }
    }
}
