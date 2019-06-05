using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if(xAxis > 0)
        {
            playerAnimator.SetBool("movingRight",true);
            playerAnimator.SetBool("movingLeft", false);
        }
        else if (xAxis < 0)
        {
            playerAnimator.SetBool("movingRight", false);
            playerAnimator.SetBool("movingLeft", true);
        }
        else
        {
            playerAnimator.SetBool("movingRight", false);
            playerAnimator.SetBool("movingLeft", false);
        }
    }
}
