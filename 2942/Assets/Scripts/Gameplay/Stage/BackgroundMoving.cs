using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    public float speed;

    private float timeBackground;
    MeshRenderer backgroundRenderer;
    private void Start()
    {
        backgroundRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBackground += Time.deltaTime;
        backgroundRenderer.material.mainTextureOffset = new Vector2(timeBackground * speed, 0);
    }
}
