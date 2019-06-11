using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLastLevel : MonoBehaviourSingleton<SaveLastLevel>
{
    public string levelName;

    public override void Awake()
    {
        base.Awake();
    }

    public void saveLevelName(string name)
    {
        levelName = name;
    }

    public string loadLevelName()
    {
        return levelName;
    }
}
