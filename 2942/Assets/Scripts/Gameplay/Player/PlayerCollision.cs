using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public delegate void onAction();
    public static onAction onEnemyHit;

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
                    onEnemyHit();
                }
                break;
            case "bullet":
                Bullet currentBullet = collision.gameObject.GetComponent<Bullet>();
                if (isPlayerBeingTarget(currentBullet))
                {
                    if (onEnemyHit != null)
                    {
                        onEnemyHit();
                    }
                }
                break;
            default:
                break;
        }
    }
}
