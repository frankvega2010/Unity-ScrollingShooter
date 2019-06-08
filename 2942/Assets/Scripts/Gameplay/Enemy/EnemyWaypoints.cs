using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoints : MonoBehaviour
{
    public GameObject targetAffected;
    public List<GameObject> waypoints;
    public int[] waypointsToWait;
    public float waitingTime;
    public float reachTimeRotation;

    private int waypointIndex = 0;
    private Enemy enemy;
    private float waitingTimer;
    private float reachTimerRotation;
    private bool canSwitch;
    private bool startWaitingTimer;
    // Start is called before the first frame update
    void Start()
    {
        enemy = targetAffected.GetComponent<Enemy>();
        targetAffected.transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.hasWaypoints && targetAffected != null)
        {
            if(startWaitingTimer)
            {
                waitingTimer += Time.deltaTime;

                if(waitingTimer >= waitingTime)
                {
                    canSwitch = true;
                    waitingTimer = 0;
                    startWaitingTimer = false;
                }
            }
            
            if (!startWaitingTimer)
            {
                reachTimerRotation += Time.deltaTime * reachTimeRotation;

                Quaternion q01 = Quaternion.LookRotation(targetAffected.transform.position - waypoints[waypointIndex].transform.position, transform.forward * -1);
                q01.x = 0;
                q01.y = 0;
                targetAffected.transform.rotation = Quaternion.Slerp(targetAffected.transform.rotation, q01, reachTimerRotation);

                targetAffected.transform.position = Vector2.MoveTowards(targetAffected.transform.position,
                                                                    waypoints[waypointIndex].transform.position,
                                                                    enemy.speed * Time.deltaTime);
            }

            if(new Vector2(targetAffected.transform.position.x, targetAffected.transform.position.y) == new Vector2(waypoints[waypointIndex].transform.position.x, waypoints[waypointIndex].transform.position.y))
            {
                if(!canSwitch)
                {
                    for (int i = 0; i < waypointsToWait.Length; i++)
                    {
                        if (waypointIndex == waypointsToWait[i])
                        {
                            startWaitingTimer = true;
                        }
                    }

                    canSwitch = true;
                }
                
                
                if(canSwitch)
                {
                    waypointIndex++;
                    reachTimerRotation = 0;
                    canSwitch = false;
                }
                
            }

            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
        else
        {
            if(targetAffected == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
