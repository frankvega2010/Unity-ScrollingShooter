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

    public enum items
    {
        rocket,
        energy,
        maxItems
    }

    
    public int lives;
    public Vector3 speed;
    public enemyStates currentState;
    public GameObject LaserGun;
    public float fireRateMin;
    public float fireRateMax;
    public List<GameObject> lootPool;

    private items itemDrop;
    private LaserGun enemyLaserGun;
    private Animator animator;
    private SpriteRenderer enemyRenderer;
    private float fireRate;
    private float fireRateTimer;
    private bool resetFireRateOnce;
    private bool dropItemOnce;
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

    private void die()
    {
        if(!dropItemOnce)
        {
            dropRandomItem();
            dropItemOnce = true;
        }
        animator.SetBool("isDead", true);
        Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length * 4);
    }

    private void dropRandomItem()
    {
        //itemDrop = (items)Random.Range(0, 2);
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

        //switch (itemDrop)
        //{
        //    case items.rocket:
        //        dropItem("Rocket");
        //        break;
        //    case items.energy:
        //        dropItem("Energy");
        //        break;
        //    default:
        //        break;
        //}
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
