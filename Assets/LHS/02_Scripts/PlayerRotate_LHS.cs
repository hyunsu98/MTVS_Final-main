using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate_LHS : MonoBehaviour
{

    Quaternion changeRot;

    [Header("���콺 ���� / ȸ�� �ӵ�")]
    public float sensitivity = 100f;

    [Header("ĳ���� ȸ�� �ӵ�")]
    public float rotSpeed = 100f;

    //���콺 ��ǲ
    private float rotY;

    void Start()
    {
        changeRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

            Quaternion rot = Quaternion.Euler(0, -rotY, 0);

            transform.rotation = rot * changeRot;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rotY = 0;
            changeRot = transform.rotation;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotSpeed);

            
            changeRot = transform.rotation;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, -transform.up, Time.deltaTime * rotSpeed);
            changeRot = transform.rotation;
        }
    }
}
