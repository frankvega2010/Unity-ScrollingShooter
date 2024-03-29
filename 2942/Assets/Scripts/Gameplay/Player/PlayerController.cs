﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public delegate void onPlayerAction();
    public static onPlayerAction onPlayerDeath;

    [Header("Game Objects")]
    public GameObject playerStatusGameObject;
    public List<GameObject> laserGuns;
    public List<LaserGun> playerLaserGuns;
    public GameObject firestormX;
    public GameObject playerModifiersGameObject;
    public GameObject upgradesIcons;
    public GameObject pointsUI;
    public GameObject deathSoundObject;

    [Header("Bars")]
    public GameObject EnergyBar;
    public GameObject FirestormXBar;

    private float duplicateCount;
    private int newPositionDirection;
    private float constantNewPositionDifference;

    private SpriteRenderer playerRenderer;
    
    private LaserGun laserGunTemplate;
    private FirestormX playerFirestormX;
    private AudioSource deathSound;
    private float energyDrainTimer;
    private float firestormXChargeTimer;
    private int spawnCount;

    private UIStatusBar energyStatusBar;
    private UIStatusBar firestormXStatusBar;

    private PlayerModifiers playerModifiers;
    private PlayerStatus playerStatus;
    private UIDisplayUpgrades upgradesDisplay;
    private UIDisplayNumbers pointsDisplay;
    private bool dieOnce;
    // Start is called before the first frame update
    void Start()
    {
        playerModifiers = playerModifiersGameObject.GetComponent<PlayerModifiers>();

        foreach (GameObject LaserGun in laserGuns)
        {
            playerLaserGuns.Add(LaserGun.GetComponent<LaserGun>());
            laserGunTemplate = LaserGun.GetComponent<LaserGun>();
        }

        playerFirestormX = firestormX.GetComponent<FirestormX>();
        playerRenderer = GetComponent<SpriteRenderer>();
        energyStatusBar = EnergyBar.GetComponent<UIStatusBar>();
        firestormXStatusBar = FirestormXBar.GetComponent<UIStatusBar>();
        playerStatus = playerStatusGameObject.GetComponent<PlayerStatus>();
        upgradesDisplay = upgradesIcons.GetComponent<UIDisplayUpgrades>();
        deathSound = deathSoundObject.GetComponent<AudioSource>();

        newPositionDirection = 1;
        duplicateCount = 1;
        spawnCount = 0;
        constantNewPositionDifference = playerModifiers.newPositionDifference;
        pointsDisplay = pointsUI.GetComponent<UIDisplayNumbers>();

        PlayerCollision.onEnemyHit += substractEnergy;
        PlayerCollision.onEnemyHit += RemoveUpgrades;
        PlayerCollision.onEnergyCollected = RecoverEnergy;
        PlayerCollision.onRocketCollected = addCannon;
        GameManager.onRoundEnd += disableUI;
        GameManager.onRoundEnd += disableCollision;
        GameManager.onRoundEnd += disableGuns;

    }

    // Update is called once per frame
    void Update()
    {
        energyDrainTimer += Time.deltaTime;
        firestormXChargeTimer += Time.deltaTime;
        energyStatusBar.value = playerStatus.energy;
        firestormXStatusBar.value = playerStatus.firestormXCharge;

        if (Input.GetKey(KeyCode.F))
        {
            foreach (LaserGun LaserGun in playerLaserGuns)
            {
                LaserGun.Shoot();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) // DEBUG
        {
            if(playerLaserGuns.Count < playerModifiers.maxCannons)
            {
                addCannon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(playerStatus.firestormXCharge >= 100)
            {
                playerStatus.firestormXCharge = 0;
                playerFirestormX.Shoot();
            }
        }

        if(energyDrainTimer >= playerModifiers.energyDrainRate)
        {
            energyDrainTimer = 0;
            playerStatus.energy--;
        }

        if(firestormXChargeTimer >= playerModifiers.firestormXChargeRate)
        {
            playerStatus.firestormXCharge++;
            if(playerStatus.firestormXCharge >= 100)
            {
                playerStatus.firestormXCharge = 100;
            }
            firestormXChargeTimer = 0;
        }

        if(playerStatus.energy <= 0)
        {
            playerDie();
        }
    }

    public void addPoints()
    {
        playerStatus.points = playerStatus.points + 10;
        pointsDisplay.number = playerStatus.points;
    }

    public void substractEnergy()
    {
        playerStatus.energy = playerStatus.energy - playerModifiers.receivedDamage;
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
            upgradesDisplay.resetUpgradesIcons();
        }
    }

    private void RecoverEnergy()
    {
        playerStatus.energy = playerStatus.energy + playerModifiers.energyRecovery;
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
        if(!dieOnce)
        {
            deathSound.Play();
            disableGuns();
            dieOnce = true;
        }
        
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    private void ResetRocketPositions()
    {
        duplicateCount = 1;
        spawnCount = 0;
        constantNewPositionDifference = playerModifiers.newPositionDifference * duplicateCount;
    }

    private void addCannon()
    {
        if (playerLaserGuns.Count < playerModifiers.maxCannons)
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
                    constantNewPositionDifference = playerModifiers.newPositionDifference * duplicateCount;
                    spawnCount = 0;
                }
            }

            upgradesDisplay.addUpgradeIcon();
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

    private void disableUI()
    {
        EnergyBar.SetActive(false);
        FirestormXBar.SetActive(false);
    }

    private void disableGuns()
    {
        playerLaserGuns.Clear();
        Destroy(playerFirestormX);
    }

    private void disableCollision()
    {
        Destroy(GetComponent<PlayerCollision>());
    }

    private void OnDestroy()
    {
        PlayerCollision.onEnemyHit -= substractEnergy;
        PlayerCollision.onEnemyHit -= RemoveUpgrades;
        GameManager.onRoundEnd -= disableUI;
        GameManager.onRoundEnd -= disableCollision;
        GameManager.onRoundEnd -= disableGuns;
    }
}
