using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject LaserGun;

    private LaserGun playerLaserGun;
    // Start is called before the first frame update
    void Start()
    {
        playerLaserGun = LaserGun.GetComponent<LaserGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            Debug.Log("DOING SOMETHING");
            playerLaserGun.Shoot();
        }
    }
}
