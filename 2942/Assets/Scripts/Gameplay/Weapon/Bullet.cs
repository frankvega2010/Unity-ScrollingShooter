using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed;
    public float maxLifespan;

    private Animator animator;
    private float lifespan;
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

        if(lifespan > maxLifespan || animator.GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            Destroy(this.gameObject);
        }

        if (!animator.GetBool("hasHit"))
        {
            transform.position = transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("hasHit", true);
    }
}
