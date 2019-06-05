using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    public float speed;

    MeshRenderer backgroundRenderer;
    private void Start()
    {
        backgroundRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        backgroundRenderer.material.mainTextureOffset = new Vector2(Time.time * speed, 0);
    }
}
