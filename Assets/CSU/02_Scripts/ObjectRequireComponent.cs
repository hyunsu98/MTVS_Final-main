using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Outline)), RequireComponent(typeof(ObjectMoveOnMouse)), RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
public class ObjectRequireComponent : MonoBehaviourPun
{
    PhotonTransformView photonTransformView;
    Outline outline;

    private void Awake()
    {
        photonTransformView = GetComponent<PhotonTransformView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
        photonTransformView.m_SynchronizeScale = true;

        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0f;
    }
}
