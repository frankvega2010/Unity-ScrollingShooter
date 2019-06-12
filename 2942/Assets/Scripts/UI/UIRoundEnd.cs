using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundEnd : MonoBehaviour
{
    public GameObject blackscreen;
    public GameObject roundMessage;
    //public Color textColor;

    private Text roundText;
    private Image blackscreenPanel;
    // Start is called before the first frame update
    void Start()
    {
        blackscreen.SetActive(false);
        roundMessage.SetActive(false);

        roundText = roundMessage.GetComponent<Text>();
    }

    public void display(string message, Color color)
    {
        blackscreen.SetActive(true);
        roundMessage.SetActive(true);

        roundText.text = message;
        roundText.color = color;
    }
}
