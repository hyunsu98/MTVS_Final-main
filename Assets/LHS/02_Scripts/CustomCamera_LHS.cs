using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ŀ������ ���� ī�޶�
public class CustomCamera_LHS : MonoBehaviour
{
    public Transform Target;
    private Transform tr;
    public float Zoom;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 TargetDist = tr.position - Target.position;
        TargetDist = Vector3.Normalize(TargetDist);

        tr.position = (TargetDist * Input.GetAxis("Mouse ScrollWheel") * Zoom);
    }
}