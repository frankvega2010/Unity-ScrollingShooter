using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrades : MonoBehaviour
{
    public float distanceBetweenIcons;
    public List<GameObject> upgradesIcons;
    public GameObject sprite;

    private int upgradeLevel;
    public void addUpgradeIcon()
    {
        GameObject spriteInstance = Instantiate(sprite);
        spriteInstance.transform.SetParent(gameObject.transform.parent);
        spriteInstance.GetComponent<RectTransform>().localPosition = Vector3.zero;
        spriteInstance.SetActive(true);
        upgradesIcons.Add(spriteInstance);
        upgradeLevel++;
        display();
    }

    public void resetUpgradesIcons()
    {
        for (int i = 0; i < upgradesIcons.Count; i++)
        {
            Destroy(upgradesIcons[i]);
        }
        upgradesIcons.Clear();
        upgradeLevel = 0;
        display();
    }

    private void display()
    {
        for (int i = 0; i < upgradeLevel; i++)
        {
            upgradesIcons[i].transform.localPosition = sprite.transform.localPosition + new Vector3((distanceBetweenIcons * i), 0,0);
        }
    }
}
