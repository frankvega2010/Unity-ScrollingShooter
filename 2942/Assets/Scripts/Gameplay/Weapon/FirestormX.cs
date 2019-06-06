using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestormX : MonoBehaviour
{
    public GameObject missileInstance;
    //public float fireRate;

    //private bool isFiring;
    //private float fireRateTimer;
    //private void Update()
    //{
    //    if (isFiring)
    //    {
    //        fireRateTimer += Time.deltaTime;
    //    }

    //    //if (fireRateTimer > fireRate)
    //    //{
    //    //    isFiring = false;
    //    //    fireRateTimer = 0;
    //    //}
    //}

    public void Shoot()
    {
            GameObject bullet = Instantiate(missileInstance, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            bullet.transform.position = GetComponent<Transform>().position;
            bullet.SetActive(true);
    }
}
