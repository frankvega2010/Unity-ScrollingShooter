﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed;
    public float smoothSpeed;
    public float maxLifespan;
    public float maxTimeForward;
    public GameObject target;

    private Animator animator;
    private float lifespan;
    private float timeForward;
    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lifespan += Time.deltaTime;

        if (timeForward < maxTimeForward)
        {
            timeForward += Time.deltaTime;
            transform.position = transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
        }
        else
        {
            if (!animator.GetBool("hasHit"))
            {
                if(target != null)
                {
                    Vector3 dir;
                    dir = transform.position - target.transform.position;
                    dir.Normalize();
                    transform.position = transform.position - dir * speed * Time.deltaTime;

                    Quaternion q01 = Quaternion.LookRotation(transform.position - target.transform.position, transform.forward);
                    q01.x = 0;
                    q01.y = 0;
                    transform.rotation = q01;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }

        if (lifespan > maxLifespan || animator.GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            Destroy(this.gameObject);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                break;
            case "bounds":
                break;
            case "bulletBounds":
                break;
            case "item":
                break;
            default:
                if(collision.gameObject == target)
                {
                    animator.SetBool("hasHit", true);
                }
                break;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bounds" || collision.gameObject.tag == "bulletBounds")
        {
            animator.SetBool("hasHit", true);
        }
    }
}
