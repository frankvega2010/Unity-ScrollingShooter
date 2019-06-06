using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed;
    public float maxLifespan;
    public GameObject target;

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

        if (!animator.GetBool("hasHit"))
        {
            Vector3 dir;
            dir = transform.position - target.transform.position;
            dir.Normalize();
            transform.position = transform.position - dir * speed * Time.deltaTime;

            Quaternion q01 = Quaternion.LookRotation(transform.position - target.transform.position, transform.forward);
            q01.x = 0;
            q01.y = 0;
            transform.rotation = q01;
            Debug.Log(transform.rotation);
            Debug.Log(transform.position);
        }

        if (lifespan > maxLifespan || animator.GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("hasHit", true);
    }
}
