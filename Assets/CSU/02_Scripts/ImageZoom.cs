using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    ImageChangeManager img_CM;
    // ZoomUI �̹���
    Image img;
    // ���� ������ �̹���
    Image parentImg;
    // Ȯ�� ������ ��ư
    Button btn;

    private void Start()
    {
        img_CM = PublicObjectManager.instance.imageChangeManager.GetComponent<ImageChangeManager>();
        
        img = PublicObjectManager.instance.imageZoomUI.GetChild(0).GetComponent<Image>();
        parentImg = GetComponentInChildren<Image>();

        btn = transform.GetComponentInChildren<Button>();
        btn.onClick.AddListener(ShowZoomUI);
    }

    void ShowZoomUI()
    {
        // UI ���ֱ�
        PublicObjectManager.instance.imageZoomUI.gameObject.SetActive(true);

        // �̹��� �ٲ��ֱ�
        // -> index �� ã�Ƽ� sprite �� ����
        int idx = img_CM.FindImageIndex(parentImg);
        if(idx != -1)
        {
            img.sprite = img_CM.spritesList[idx];
        }
    }
}
