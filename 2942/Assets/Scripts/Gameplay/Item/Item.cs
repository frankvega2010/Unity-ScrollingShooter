using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
    }
}
