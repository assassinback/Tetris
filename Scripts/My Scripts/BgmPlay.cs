using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmPlay : MonoBehaviour
{
    public int toggle;
    void Start()
    {
         toggle = PlayerPrefs.GetInt("toggle");
        if (toggle == 1)
        {
            GetComponent<AudioSource>().volume = 1; ;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
      
    }
}
