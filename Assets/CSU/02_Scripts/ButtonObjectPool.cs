using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObjectPool : MonoBehaviour
{
    Transform parent;
    Image img;
    Button btn;

    private void Start()
    {
        parent = PublicObjectManager.instance.co_content;
        img = GetComponent<Image>();
        btn = GetComponent<Button>();

        btn.onClick.AddListener(CloseUI);
        btn.onClick.AddListener(SetObjectPool);
    }

    void CloseUI()
    {
        PublicObjectManager.instance.createObjectUI.gameObject.SetActive(false);
    }

    void SetObjectPool()
    {
        ObjectManager objMgr = PublicObjectManager.instance.objectManager.GetComponent<ObjectManager>();
        int idx = objMgr.objPool.Count;

        objMgr.SetItemIndex(idx);
        objMgr.CreateObject();
    }
}
