﻿using System.Collections;
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
    public GameObject LaserGun;
    public float fireRateMin;
    public float fireRateMax;

    private LaserGun enemyLaserGun;
    private Animator animator;
    private SpriteRenderer enemyRenderer;
    private float fireRate;
    private float fireRateTimer;
    private bool doOnce;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyLaserGun = LaserGun.GetComponent<LaserGun>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case enemyStates.moving:
                resetFireRate();
                fireRateTimer += Time.deltaTime;
                move();
                shoot();
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

    private void resetFireRate()
    {
        if (!doOnce)
        {
            fireRate = Random.Range(fireRateMin, fireRateMax);
            doOnce = true;
        }
    }

    private void shoot()
    {
        if (fireRateTimer >= fireRate)
        {
            enemyLaserGun.Shoot();
            doOnce = false;
            fireRateTimer = 0;
        }
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

    private bool isEnemyBeingTarget(Bullet currentBullet)
    {
        if (currentBullet.target == null)
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
            case "bullet":
                Bullet currentBullet = collision.gameObject.GetComponent<Bullet>();
                if(isEnemyBeingTarget(currentBullet))
                {
                    hitEnemy();
                }
                break;
            case "homingbullet":
                    hitEnemy();
                break;
            case "Player":
                hitEnemy();
                break;
            default:
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "bounds":
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
        
    }
}
