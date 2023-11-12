using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class asdfe : MonoBehaviour
{
    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(RoomManager.instance.ShowFileBrowser);
    }
}
