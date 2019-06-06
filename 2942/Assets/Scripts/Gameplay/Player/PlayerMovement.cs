using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x,0.02f,0.98f);
        pos.y = Mathf.Clamp(pos.y, 0.08f, 0.92f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        //Debug.Log(pos);
        

        if(xAxis > 0)
        {
            playerAnimator.SetBool("movingRight",true);
            playerAnimator.SetBool("movingLeft", false);
            transform.position = transform.position + new Vector3(speed, 0,0) * Time.deltaTime;
        }
        else if (xAxis < 0)
        {
            playerAnimator.SetBool("movingRight", false);
            playerAnimator.SetBool("movingLeft", true);
            transform.position = transform.position + new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
        else
        {
            playerAnimator.SetBool("movingRight", false);
            playerAnimator.SetBool("movingLeft", false);
        }

        if(yAxis > 0)
        {
            transform.position = transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
        }
        else if (yAxis < 0)
        {
            transform.position = transform.position + new Vector3(0, -speed, 0) * Time.deltaTime;
        }

        
    }
}
