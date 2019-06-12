using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayNumbers : MonoBehaviour
{
    public string textToDisplay;
    public float number;

    private Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = textToDisplay + ": " + number.ToString("f0");
    }
}
