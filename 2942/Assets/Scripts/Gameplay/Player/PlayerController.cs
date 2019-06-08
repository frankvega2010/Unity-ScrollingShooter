using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void onPlayerAction();
    public static onPlayerAction onPlayerDeath;

    public List<GameObject> laserGuns;
    public GameObject firestormX;
    public int energy;
    public int points;
    public int firestormXCharge;
    public float energyDrainRate;
    public float newPositionDifference;
    public int maxCannons;
    public int energyRecovery;
    public int receivedDamage;

    public float duplicateCount;
    private int newPositionDirection;
    private float constantNewPositionDifference;
    private SpriteRenderer playerRenderer;
    public List<LaserGun> playerLaserGuns;
    private LaserGun laserGunTemplate;
    private FirestormX playerFirestormX;
    private float energyDrainTimer;
    public int spawnCount;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject LaserGun in laserGuns)
        {
            playerLaserGuns.Add(LaserGun.GetComponent<LaserGun>());
            laserGunTemplate = LaserGun.GetComponent<LaserGun>();
        }
        newPositionDirection = 1;
        duplicateCount = 1;
        spawnCount = 0;
        constantNewPositionDifference = newPositionDifference;
        playerFirestormX = firestormX.GetComponent<FirestormX>();
        PlayerCollision.onEnemyHit += substractEnergy;
        PlayerCollision.onEnemyHit += RemoveUpgrades;
        PlayerCollision.onEnergyCollected = RecoverEnergy;
        PlayerCollision.onRocketCollected = addCannon;
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        energyDrainTimer += Time.deltaTime;

        if(Input.GetKey(KeyCode.F))
        {
            foreach (LaserGun LaserGun in playerLaserGuns)
            {
                LaserGun.Shoot();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) // DEBUG
        {
            if(playerLaserGuns.Count < maxCannons)
            {
                addCannon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(firestormXCharge >= 100)
            {
                firestormXCharge = 0;
                playerFirestormX.Shoot();
            }
        }

        if(energyDrainTimer >= energyDrainRate)
        {
            energyDrainTimer = 0;
            energy--;
        }

        if(energy <= 0)
        {
            playerDie();
        }
    }

    public void addPoints()
    {
        points = points + 10;
    }

    public void substractEnergy()
    {
        energy = energy - receivedDamage;
        ChangeColor(Color.red);
    }

    private void RemoveUpgrades()
    {
        if(playerLaserGuns.Count > 1)
        {
            LaserGun gun;
            int maxGuns = playerLaserGuns.Count - 1;
            for (int i = maxGuns; i > 0; i--)
            {
                gun = playerLaserGuns[i].GetComponent<LaserGun>();
                if (!gun.isMainGun)
                {
                    playerLaserGuns.Remove(gun);
                    Destroy(gun.gameObject);
                }
            }
            ResetRocketPositions();
        }
    }

    private void RecoverEnergy()
    {
        energy = energy + energyRecovery;
        ChangeColor(Color.green);
    }

    private void ChangeColor(Color color)
    {
        playerRenderer.material.color = color;
        Invoke("switchColorBack", 0.1f);
    }

    private void switchColorBack()
    {
        playerRenderer.material.color = Color.white;
    }

    private void playerDie()
    {
        if(onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    private void ResetRocketPositions()
    {
        duplicateCount = 1;
        spawnCount = 0;
        constantNewPositionDifference = newPositionDifference * duplicateCount;
    }

    private void addCannon()
    {
        if (playerLaserGuns.Count < maxCannons)
        {
            for (int i = 0; i < 2; i++)
            {
                LaserGun laserGunInstance = Instantiate(laserGunTemplate);
                laserGunInstance.transform.SetParent(gameObject.transform);
                laserGunInstance.transform.localPosition = Vector3.zero;
                laserGunInstance.GetComponent<LaserGun>().isMainGun = false;
                spawnCount++;
                newPositionDirection = newPositionDirection * -1;
                laserGunInstance.transform.localPosition = laserGunInstance.transform.localPosition + (new Vector3(constantNewPositionDifference, 0, 0) * newPositionDirection); // direction 1 or -1
                playerLaserGuns.Add(laserGunInstance);

                if (isSpawnCountPair())
                {
                    duplicateCount++;
                    constantNewPositionDifference = newPositionDifference * duplicateCount;
                    spawnCount = 0;
                }
            }
        }
        
    }

    private bool isSpawnCountPair()
    {
        if(spawnCount % 2 == 0 && spawnCount != 0)
        {
            return true;
        }
        return false;
    }
}
