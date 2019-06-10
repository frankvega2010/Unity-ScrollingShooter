using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public delegate void onAction();
    public static onAction onEnemyHit;
    public static onAction onEnergyCollected;
    public static onAction onRocketCollected;

    public GameObject soundObject;
    public GameObject soundItemObject;

    private AudioSource sound;
    private AudioSource soundItem;
    private void Start()
    {
        sound = soundObject.GetComponent<AudioSource>();
        soundItem = soundItemObject.GetComponent<AudioSource>();
    }

    private bool isPlayerBeingTarget(Bullet currentBullet)
    {
        if (currentBullet.target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                if(onEnemyHit != null)
                {
                    sound.Play();
                    onEnemyHit();
                }
                break;
            case "bullet":
                Bullet currentBullet = collision.gameObject.GetComponent<Bullet>();
                if (isPlayerBeingTarget(currentBullet))
                {
                    sound.Play();
                    if (onEnemyHit != null)
                    {
                        onEnemyHit();
                    }
                }
                break;
            case "item":
                
                switch (collision.gameObject.name)
                {
                    case "Energy":
                        if (onEnergyCollected != null)
                        {
                            soundItem.Play();
                            onEnergyCollected();
                            Destroy(collision.gameObject);
                        }
                        break;
                    case "Rocket":
                        if (onRocketCollected != null)
                        {
                            soundItem.Play();
                            onRocketCollected();
                            Destroy(collision.gameObject);
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
