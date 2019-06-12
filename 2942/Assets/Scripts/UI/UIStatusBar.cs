using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBar : MonoBehaviour
{
    public int value;

    private Scrollbar scrollbar;
    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        scrollbar.size = value * 0.01f;
    }
}
