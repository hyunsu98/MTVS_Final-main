using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thanks : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<Image>().enabled = true;
        }
    }
}
