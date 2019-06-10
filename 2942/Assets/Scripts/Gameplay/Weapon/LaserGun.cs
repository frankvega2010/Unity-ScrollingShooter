using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public GameObject bulletInstance;
    public float fireRate;
    public bool isBot;
    public bool isMainGun;
    public GameObject target;
    public GameObject soundObject;
    public Color bulletColor;

    private bool isFiring;
    private float fireRateTimer;
    private AudioSource sound;

    private void Start()
    {
        sound = soundObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isFiring)
        {
            fireRateTimer += Time.deltaTime;
        } 

        if(fireRateTimer > fireRate)
        {
            isFiring = false;
            fireRateTimer = 0;
        }
    }

    public void Shoot()
    {
        if(!isFiring)
        {
            isFiring = true;
            GameObject bullet = Instantiate(bulletInstance, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            bullet.transform.position = GetComponent<Transform>().position;
            bullet.SetActive(true);

            if(isBot)
            {
                if(target != null)
                {
                    Bullet bulletComponent = bullet.GetComponent<Bullet>();
                    bulletComponent.target = target;
                }
            }

            bullet.GetComponent<SpriteRenderer>().material.color = bulletColor;
            sound.Play();
        }
    }
}
