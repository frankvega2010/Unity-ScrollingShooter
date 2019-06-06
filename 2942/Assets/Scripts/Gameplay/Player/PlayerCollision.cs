using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public delegate void onAction();

    public static onAction onEnemyHit;

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
            default:
                break;
        }
    }
}
