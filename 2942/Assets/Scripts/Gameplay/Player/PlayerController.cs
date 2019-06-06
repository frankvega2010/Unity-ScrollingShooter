using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject LaserGun;
    public GameObject FirestormX;
    public int FirestormXCharge;

    private LaserGun playerLaserGun;
    private FirestormX playerFirestormX;
    // Start is called before the first frame update
    void Start()
    {
        playerLaserGun = LaserGun.GetComponent<LaserGun>();
        playerFirestormX = FirestormX.GetComponent<FirestormX>();
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
}
