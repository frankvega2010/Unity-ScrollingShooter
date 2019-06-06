using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum enemyStates
    {
        moving,
        dead,
        maxStates
    }


    public int lives;
    public Vector3 speed;
    public enemyStates currentState;

    private Animator animator;
    private SpriteRenderer enemyRenderer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case enemyStates.moving:
                move();
                break;
            case enemyStates.dead:
                die();
                break;
            default:
                break;
        }
    }

    private void hitEnemy()
    {
        if (lives > 0)
        {
            lives--;
            enemyRenderer.material.color = Color.red;
            Invoke("switchColorBack", 0.1f);
            if(lives <= 0)
            {
                currentState = enemyStates.dead;
            }
        }
    }

    private void move()
    {
        transform.position = transform.position + speed * Time.deltaTime;
    }

    private void die()
    {
        animator.SetBool("isDead", true);
        Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length * 4);
    }

    private void switchColorBack()
    {
        enemyRenderer.material.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "bullet":
                hitEnemy();
                break;
            case "bounds":
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
}
