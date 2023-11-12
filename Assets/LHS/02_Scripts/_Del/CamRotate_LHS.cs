using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate_LHS : MonoBehaviour
{
    [Header("ȸ���ӵ�")]
    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;

    void Start()
    {
        // ������ �� eulerangles �� ���� mx, my �� �Ҵ�
        mx = transform.eulerAngles.y;
        my = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
