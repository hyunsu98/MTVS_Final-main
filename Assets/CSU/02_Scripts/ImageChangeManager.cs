using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangeManager : MonoBehaviour
{
    [Header("< 이미지 리스트 >")]
    public List<Sprite> spritesList = new List<Sprite>();

    [Header("< 이미지 정보 >")]
    public Transform imageInfo;

    public void SetImageChange(int idx)
    {
        Image img = imageInfo.GetComponentInChildren<Image>();
        img.sprite = spritesList[idx];
        img.color = Color.white;
    }

    public int FindImageIndex(Image img)
    {
        int idx = -1;

        for (int i = 0; i < spritesList.Count; ++i)
        {
            if (spritesList[i] == img.sprite)
            {
                idx = i;
                return idx;
            }
        }

        return idx;
    }
}
