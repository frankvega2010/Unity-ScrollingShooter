﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void onEnemyAction(GameObject Enemy);
    public static onEnemyAction onEnemyDeath;

    public enum enemyStates
    {
        moving,
        dead,
        maxStates
    }
    
    public int lives;
    public float speed;
    public enemyStates currentState;
    public GameObject LaserGun;
    public float fireRateMin;
    public float fireRateMax;
    public bool hasWaypoints;
    public List<GameObject> lootPool;

    [Header("Squad")]
    public bool isParent;
    public List<GameObject> squadMembers;

    private LaserGun enemyLaserGun;
    private Animator animator;
    private SpriteRenderer enemyRenderer;
    private float fireRate;
    private float fireRateTimer;
    private bool resetFireRateOnce;
    private bool dropItemOnce;
    private void Start()
    {
        if(!isParent)
        {
            animator = GetComponent<Animator>();
            enemyRenderer = GetComponent<SpriteRenderer>();
            enemyLaserGun = LaserGun.GetComponent<LaserGun>();
        }
        else
        {
            onEnemyDeath = deleteEnemyFromSquad;
        }
    }

    private void Update()
    {
        if(!isParent)
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
        if (!hasWaypoints)
        {
            transform.position = transform.position + new Vector3(0, speed,0) * Time.deltaTime;
        }
    }

    private void resetFireRate()
    {
        if (!resetFireRateOnce)
        {
            fireRate = Random.Range(fireRateMin, fireRateMax);
            resetFireRateOnce = true;
        }
    }

    private void shoot()
    {
        if (fireRateTimer >= fireRate)
        {
            enemyLaserGun.Shoot();
            resetFireRateOnce = false;
            fireRateTimer = 0;
        }
    }

    private void deleteEnemyFromSquad(GameObject enemy)
    {
        if(isParent)
        {
            squadMembers.Remove(enemy);
            if(squadMembers.Count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void die()
    {
        if(!dropItemOnce)
        {
            dropRandomItem();
            dropItemOnce = true;

            if(onEnemyDeath != null)
            {
                onEnemyDeath(this.gameObject);
            }
        }
        animator.SetBool("isDead", true);
        Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length * 4);
    }

    private void dropRandomItem()
    {
        int randomChance = Random.Range(0, 11);

        switch (randomChance)
        {
            case 0:
                dropItem("Energy");
                break;
            case 1:
                dropItem("Energy");
                break;
            case 2:
                dropItem("Energy");
                break;
            case 3:
                dropItem("Energy");
                break;
            case 4:
                dropItem("Energy");
                break;
            case 5:
                dropItem("Energy");
                break;
            case 6:
                dropItem("Energy");
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                dropItem("Rocket");
                break;
            case 10:
                dropItem("Rocket");
                break;
            default:
                break;
        }
    }

    private void dropItem(string name)
    {
        foreach (GameObject currentLoot in lootPool)
        {
            if(currentLoot.name == name)
            {
                GameObject lootToDrop = Instantiate(currentLoot);
                lootToDrop.SetActive(true);
                lootToDrop.name = name;
                lootToDrop.transform.position = transform.position;
            }
        }
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
            case "item":
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
