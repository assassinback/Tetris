using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    public LevelInfo currentLevelInfo;
    private void Awake()
    {
        _instance= this;
    }
}
