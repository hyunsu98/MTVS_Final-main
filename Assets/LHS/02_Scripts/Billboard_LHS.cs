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
        //���� �չ����� ī�޶� �չ������� ��������
        transform.forward = Camera.main.transform.forward;
    }
}
