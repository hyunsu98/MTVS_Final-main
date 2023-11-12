using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    ImageChangeManager img_CM;
    // ZoomUI 이미지
    Image img;
    // 현재 사진의 이미지
    Image parentImg;
    // 확대 아이콘 버튼
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
        // UI 켜주기
        PublicObjectManager.instance.imageZoomUI.gameObject.SetActive(true);

        // 이미지 바꿔주기
        // -> index 를 찾아서 sprite 에 적용
        int idx = img_CM.FindImageIndex(parentImg);
        if(idx != -1)
        {
            img.sprite = img_CM.spritesList[idx];
        }
    }
}
