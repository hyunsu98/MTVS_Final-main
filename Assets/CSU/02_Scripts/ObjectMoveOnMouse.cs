using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveOnMouse : MonoBehaviour
{
    ObjectTransformControl objTransformControl;

    void Start()
    {
        objTransformControl = PublicObjectManager.instance.selectedObjectParent.GetComponent<ObjectTransformControl>();
    }

    void Update()
    {
        // 선택된 오브젝트가 없으면 return
        if (PublicObjectManager.instance.selectedObjectParent.childCount <= 0) return;

        // 선택된 오브젝트가 액자라면
        if (objTransformControl.MyGetChild().gameObject.layer == LayerMask.NameToLayer("Frame")) MoveFrame();

        // 선택된 오브젝트가 액자가 아니라면 (기타 오브젝트)
        else MoveEtcObject();

    }

    // 액자 오브젝트를 움직이는 함수
    void MoveFrame()
    {
        // 왼쪽 컨트롤 키를 눌렀을 때
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // 선택된 오브젝트가 아니면 return
            if (transform.parent != PublicObjectManager.instance.selectedObjectParent) return;

            // Ray 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // ray 가 닿은 곳이 땅이라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // 새로운 위치 설정 및 UI 변경
                    // -> UI 변경 시 자동으로 위치가 변경됨
                    Vector3 pos = hitInfo.point;
                    pos.y = 0f;
                    objTransformControl.SetPositionUI(pos);
                }
                // ray 가 닿은 곳이 벽이라면
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    // 위치 설정
                    Vector3 pos = hitInfo.point;
                    objTransformControl.SetPositionUI(pos);
                    // 각도 설정
                    transform.forward = hitInfo.normal;
                    float yAngle = transform.localEulerAngles.y;
                    yAngle = yAngle > 180f ? yAngle - 360f : yAngle;
                    objTransformControl.SetRotationUI(yAngle);
                }
            }
        }
    }

    // 액자가 아닌 오브젝트를 움직이는 함수
    void MoveEtcObject()
    {
        // 왼쪽 컨트롤 키를 눌렀을 때
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // 선택된 오브젝트가 아니면 return
            if (transform.parent != PublicObjectManager.instance.selectedObjectParent) return;

            // Ray 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground")) return;

                // 새로운 위치 설정 및 UI 변경
                // -> UI 변경 시 자동으로 위치가 변경됨
                Vector3 pos = hitInfo.point;
                pos.y = transform.position.y;
                objTransformControl.SetPositionUI(pos);
            }
        }
    }
}
