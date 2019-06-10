using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void onEnemyAction(GameObject Enemy);
    public onEnemyAction onEnemyDeath;
    public onEnemyAction onEnemyDestroyed;

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
    public bool canShoot;
    public GameObject waypoint;
    public GameObject ExplosionObject;
    public GameObject hurtObject;
    public List<GameObject> lootPool;

    [Header("Squad")]
    public bool isParent;
    public List<GameObject> squadMembers;

    private LaserGun enemyLaserGun;
    private Animator animator;
    private SpriteRenderer enemyRenderer;
    private AudioSource explosionSound;
    private AudioSource hurtSound;
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
            explosionSound = ExplosionObject.GetComponent<AudioSource>();
            hurtSound = hurtObject.GetComponent<AudioSource>();
        }
    }

    public void deActivateWaypoint()
    {
        if (isParent)
        {
            waypoint.SetActive(false);
        }
    }

    public void activateWaypoint()
    {
        if(isParent)
        {
            waypoint.SetActive(true);
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

    private void hitEnemy(int damage)
    {
        if (lives > 0)
        {
            lives = lives - damage;
            enemyRenderer.material.color = Color.red;
            Invoke("switchColorBack", 0.1f);
            if (lives <= 0)
            {
                currentState = enemyStates.dead;
            }
        }
    }

    public void killEnemy()
    {
        animator.SetBool("isDead", true);
        Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length * 4);
        //explosionSound.Play();
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
            if(canShoot)
            {
                enemyLaserGun.Shoot();
            }
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

            if(onEnemyDeath != null)
            {
                Debug.Log("ENTERING");
                onEnemyDeath(this.gameObject);
            }

            if(!isParent)
            {
                animator.SetBool("isDead", true);
                Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length * 7);
            }

            explosionSound.Play();
        }
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
                    hurtSound.Play();
                    hitEnemy();
                }
                break;
            case "homingbullet":
                if (collision.gameObject.GetComponent<HomingMissile>().target == gameObject)
                {
                    hurtSound.Play();
                    hitEnemy(999);
                }
                break;
            case "Player":
                hurtSound.Play();
                hitEnemy();
                break;
            case "item":
                break;
            case "bulletBounds":
                canShoot = true;
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
                if(onEnemyDestroyed != null)
                {
                    onEnemyDestroyed(this.gameObject);
                }
                Destroy(this.gameObject);
                break;
            case "bulletBounds":
                canShoot = false;
                break;
            default:
                break;
        }
        
    }
}
