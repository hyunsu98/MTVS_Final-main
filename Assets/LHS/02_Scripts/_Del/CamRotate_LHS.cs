using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate_LHS : MonoBehaviour
{
    [Header("회전속도")]
    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;

    void Start()
    {
        // 시작할 때 eulerangles 의 값을 mx, my 에 할당
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
