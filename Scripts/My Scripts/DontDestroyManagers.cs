using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManagers : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
