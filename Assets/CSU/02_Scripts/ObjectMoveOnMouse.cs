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
        // ���õ� ������Ʈ�� ������ return
        if (PublicObjectManager.instance.selectedObjectParent.childCount <= 0) return;

        // ���õ� ������Ʈ�� ���ڶ��
        if (objTransformControl.MyGetChild().gameObject.layer == LayerMask.NameToLayer("Frame")) MoveFrame();

        // ���õ� ������Ʈ�� ���ڰ� �ƴ϶�� (��Ÿ ������Ʈ)
        else MoveEtcObject();

    }

    // ���� ������Ʈ�� �����̴� �Լ�
    void MoveFrame()
    {
        // ���� ��Ʈ�� Ű�� ������ ��
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // ���õ� ������Ʈ�� �ƴϸ� return
            if (transform.parent != PublicObjectManager.instance.selectedObjectParent) return;

            // Ray �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // ray �� ���� ���� ���̶��
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // ���ο� ��ġ ���� �� UI ����
                    // -> UI ���� �� �ڵ����� ��ġ�� �����
                    Vector3 pos = hitInfo.point;
                    pos.y = 0f;
                    objTransformControl.SetPositionUI(pos);
                }
                // ray �� ���� ���� ���̶��
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    // ��ġ ����
                    Vector3 pos = hitInfo.point;
                    objTransformControl.SetPositionUI(pos);
                    // ���� ����
                    transform.forward = hitInfo.normal;
                    float yAngle = transform.localEulerAngles.y;
                    yAngle = yAngle > 180f ? yAngle - 360f : yAngle;
                    objTransformControl.SetRotationUI(yAngle);
                }
            }
        }
    }

    // ���ڰ� �ƴ� ������Ʈ�� �����̴� �Լ�
    void MoveEtcObject()
    {
        // ���� ��Ʈ�� Ű�� ������ ��
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // ���õ� ������Ʈ�� �ƴϸ� return
            if (transform.parent != PublicObjectManager.instance.selectedObjectParent) return;

            // Ray �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground")) return;

                // ���ο� ��ġ ���� �� UI ����
                // -> UI ���� �� �ڵ����� ��ġ�� �����
                Vector3 pos = hitInfo.point;
                pos.y = transform.position.y;
                objTransformControl.SetPositionUI(pos);
            }
        }
    }
}
