using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PublicObjectManager : MonoBehaviourPun
{
    public static PublicObjectManager instance;

    [Header("< Manager >")]
    public Transform objectManager;
    public Transform imageChangeManager;

    [Header("< My Room >")]
    public Transform myRoomUI;
    public Transform bottomCenter;
    public Transform buttonLike;

    [Header("< Create Object >")]
    public Transform createObjectParent;
    public Transform selectedObjectParent;
    public Transform createObjectUI;
    public Transform co_content;

    [Header("< Object Transform >")]
    public Transform objectTransformUI;

    [Header("< Image >")]
    public Transform imageChangeUI;
    public Transform imageZoomUI;

    [Header("< Control Icon >")]
    public Transform chatIcon;
    public RectTransform chatAIWindow;

    [Header("< Json >")]
    public Transform saveJsonButton;

    private void Awake()
    {
        if(instance == null)    instance = this;
    }
}
