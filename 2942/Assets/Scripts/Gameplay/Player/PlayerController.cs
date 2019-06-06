using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject LaserGun;
    public GameObject FirestormX;
    public int energy;
    public int FirestormXCharge;

    private SpriteRenderer playerRenderer;
    private LaserGun playerLaserGun;
    private FirestormX playerFirestormX;
    // Start is called before the first frame update
    void Start()
    {
        playerLaserGun = LaserGun.GetComponent<LaserGun>();
        playerFirestormX = FirestormX.GetComponent<FirestormX>();
        PlayerCollision.onEnemyHit = substractEnergy;
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            playerLaserGun.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(FirestormXCharge >= 100)
            {
                FirestormXCharge = 0;
                playerFirestormX.Shoot();
            }
        }
    }

    public void substractEnergy()
    {
        energy = energy - 5;
        playerRenderer.material.color = Color.red;
        Invoke("switchColorBack", 0.1f);
    }

    private void switchColorBack()
    {
        playerRenderer.material.color = Color.white;
    }
}
