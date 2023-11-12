using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard_LHS : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        //나의 앞방향을 카메라 앞방향으로 셋팅하자
        transform.forward = Camera.main.transform.forward;
    }
}
