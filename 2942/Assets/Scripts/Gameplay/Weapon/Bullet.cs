using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed;
    public float maxLifespan;
    public GameObject target;

    private Animator animator;
    private float lifespan;
    private Rigidbody rig;
    private bool setTargetOnce;
    private Vector3 dir;
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

        if(lifespan > maxLifespan || animator.GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            Destroy(this.gameObject);
        }

        if (!animator.GetBool("hasHit"))
        {
            if(target != null)
            {
                if (!setTargetOnce)
                {
                    dir = transform.position - target.transform.position;
                    dir.Normalize();

                    Quaternion q01 = Quaternion.LookRotation(transform.position - target.transform.position, transform.forward);
                    q01.x = 0;
                    q01.y = 0;
                    transform.rotation = q01;
                    setTargetOnce = true;
                }

                transform.position = transform.position - dir * speed * Time.deltaTime; 
            }
            else
            {
                transform.position = transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
            }
            
        } 
    }

    private bool isPlayerBeingTarget()
    {
        if (target != null)
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
            case "Player":
                if (isPlayerBeingTarget())
                {
                    animator.SetBool("hasHit", true);
                }
                break;
            case "bounds":
                break;
            case "item":
                break;
            case "enemy":
                if (!isPlayerBeingTarget())
                {
                    animator.SetBool("hasHit", true);
                }
                break;
            default:
                animator.SetBool("hasHit", true);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bounds")
        {
            animator.SetBool("hasHit", true);
        }
    }
}
