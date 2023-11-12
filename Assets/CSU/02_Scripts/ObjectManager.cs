using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Networking;
using System.IO;

public class ObjectManager : MonoBehaviourPun
{
    // ��ġ�� Object List ����
    [Header("< �ٹ̱� ������ >")]
    public List<ObjectInfo> objPool = new List<ObjectInfo>();
    public List<GameObject> objPoolUI = new List<GameObject>();
    // ��ġ ������Ʈ ��� �ε���
    public int itemIndex;

    // ������Ʈ �θ�
    Transform createObjParent;   // ������ ������Ʈ�� �θ�
    Transform selectedObjParent; // ���õ� ������Ʈ�� �θ�

    [Header("< Ray �ִ� �Ÿ� >")]
    public float maxDistance = 15f;

    [Header("< Create Button Text >")]
    public List<Button> createButtons;
    public List<Text> createButtonTexts;

    // UI
    GameObject objTransformUI;
    GameObject bottomCenter;
    GameObject normal_BC;
    GameObject selected_BC;
    Button btnDone;

    [Header("< ��ġ�� ������Ʈ >")]
    // ��ġ�� ObjectList
    public List<CreatedInfo> createdObj = new List<CreatedInfo>();
    // ���õ� ������Ʈ index
    int selectedIndex;

    ObjectTransformControl obj_TC;

    // Ray �� ���� ������Ʈ
    [Header("< Hit Object >")]
    [SerializeField]
    List<GameObject> hitObjects;

    private void Awake()
    {
        createObjParent = PublicObjectManager.instance.createObjectParent;
        selectedObjParent = PublicObjectManager.instance.selectedObjectParent;
        objTransformUI = PublicObjectManager.instance.objectTransformUI.gameObject;
        bottomCenter = PublicObjectManager.instance.myRoomUI.GetChild(2).gameObject;

        normal_BC = bottomCenter.transform.GetChild(0).gameObject;
        selected_BC = bottomCenter.transform.GetChild(1).gameObject;

        btnDone = objTransformUI.transform.Find("Btn_Done").GetComponent<Button>();
        btnDone.onClick.AddListener(SetChangeTransform);
        obj_TC = selectedObjParent.GetComponent<ObjectTransformControl>();

        for (int i = 0; i < createButtons.Count; ++i)
        {
            Text t = createButtons[i].GetComponentInChildren<Text>();
            createButtonTexts.Add(t);
            createButtons[i].onClick.AddListener(() => ChangeUIList(t.text));
        }
    }

    private void Start()
    {
        OnGet();
    }

    private void Update()
    {
        if (RoomManager.instance.room.memberNickname != HttpManager.instance.nickname) return;

        SelectObject();

        if (Input.GetKeyDown(KeyCode.T))
        {
            OnGet();
        }
    }

    public void SetItemIndex(int idx)
    {
        itemIndex = idx;
    }

    #region Object ����(����, ����, ����)
    // �ε�
    public void LoadObject(int idx, Vector3 pos, float angle, float scale)
    {
        GameObject obj = Instantiate(objPool[idx].objPrefab, pos, Quaternion.Euler(0, angle, 0), createObjParent);
        //GameObject obj = PhotonNetwork.Instantiate(objPool[idx].objPrefab.name, pos, Quaternion.Euler(0, angle, 0));
        obj.transform.localScale = new Vector3(scale, scale, scale);

        CreatedInfo info = new CreatedInfo();
        info.go = obj;
        info.idx = idx;

        createdObj.Add(info);
    }

    // ����
    public void CreateObject()
    {
        LoadObject(itemIndex, Vector3.zero, 0f, 1f);
    }

    // ����
    public void RemoveObject()
    {
        photonView.RPC("RpcRemoveObject", RpcTarget.All);
    }

    [Photon.Pun.PunRPC]
    public void RpcRemoveObject()
    {
        Destroy(createdObj[selectedIndex].go);
        createdObj.RemoveAt(selectedIndex);
        objTransformUI.SetActive(false);
        ClearHitList();
        Destroy(selectedObjParent.GetChild(0).gameObject);
    }

    // ������Ʈ ����
    public void SelectObject()
    {
        // ���콺 ��Ŭ���� �ϸ� ������ ������Ʈ�� �����ϰ� �ʹ�.
        // 1. ���콺 ��Ŭ���� �ϸ�
        if (Input.GetMouseButtonDown(1))
        {
            // �̹� ���õ� ������Ʈ�� ������ ��Ŭ�� �����ֱ�
            if (selectedObjParent.childCount != 0) return;
            // 2. ���̸� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 3. ������ ������Ʈ�� ������ ������Ʈ���� Ȯ��
                // -> �θ� üũ
                if (hitInfo.transform.parent == createObjParent)
                {
                    Outline outline = hitInfo.transform.GetComponent<Outline>();
                    if (outline != null)
                    {
                        GameObject go = outline.gameObject;
                        ClearHitList();
                        outline.OutlineWidth = 10f;
                        hitObjects.Add(go);

                        for (int i = 0; i < createdObj.Count; ++i)
                        {
                            if (createdObj[i].go == go)
                            {
                                selectedIndex = i;
                                break;
                            }
                        }

                        // 4. ������ ������Ʈ�� �θ� ����
                        hitInfo.transform.parent = selectedObjParent;

                        // 5. ������ ������Ʈ ����
                        obj_TC.MySetChild();
                    }

                    // 6. UI �ѱ�
                    objTransformUI.SetActive(true);
                    normal_BC.SetActive(false);
                    selected_BC.SetActive(true);

                    // ������ ������Ʈ�� Frame �̸�
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Frame"))
                    {
                        // Image Change UI ���ֱ�
                        PublicObjectManager.instance.imageChangeUI.gameObject.SetActive(true);
                        PublicObjectManager.instance.imageChangeManager.GetComponent<ImageChangeManager>().imageInfo = hitInfo.transform;
                    }
                }
            }
            else
            {
                ClearHitList();
                objTransformUI.SetActive(false);
                normal_BC.SetActive(true);
                selected_BC.SetActive(false);
            }
        }
    }
    #endregion

    #region Json ����
    public void SaveJson()
    {
        if (RoomManager.instance.room.memberNickname != HttpManager.instance.nickname) return;

        // map.createObjects �� �ִ� ������ json ���� ��ȯ
        // map.createObjects �� ������ŭ SaveJsonInfo ���� ����
        SaveJsonInfo info;

        // ArrayJson �����
        ArrayJson arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        for (int i = 0; i < createdObj.Count; ++i)
        {
            CreatedInfo ci = createdObj[i];
            Transform tf = ci.go.transform;

            info = new SaveJsonInfo();
            info.idx = ci.idx;
            info.position = tf.position;
            info.angle = tf.eulerAngles.y;
            info.scaleValue = tf.localScale.x;
            // ArrayJson �ϳ� ���� datas �� �ϳ��� �߰�
            arrayJson.datas.Add(info);
        }

        // arrayJson �� Json ���� ��ȯ
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        Debug.Log(jsonData);

        // ��Ʈ��ũ�� ������
        OnPost(jsonData);
    }
    #endregion

    #region ��Ʈ��ũ (Post, Get)
    public void OnPost(string s)
    {
        string url = "/object/";
        url += RoomManager.instance.room.id;

        // roomid �ʿ�
        //url += HttpManager.instance.username;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    public void OnGet()
    {
        ClearFloor();

        string url = "/object/";
        url += RoomManager.instance.room.id;

        //url += HttpManager.instance.nickname;
        //print($"ĳ���� �ε� �̸� + {HttpManager.instance.username}");

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    #region Post, Get ���� or ���� Log
    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("������Ʈ ���� ������ ����");
    }

    void OnPostFailed()
    {
        print("������Ʈ ���� ������ ����!");
    }

    void OnGetComplete(DownloadHandler result)
    {
        print("������Ʈ �ҷ����� ����");

        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(result.text);

        for (int i = 0; i < arrayJson.datas.Count; ++i)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.angle, info.scaleValue);
        }
    }

    void OnGetFailed()
    {
        print("������Ʈ �ҷ����� ����");
    }
    #endregion

    #endregion

    #region ��Ÿ �Լ�
    // ������ hit �� object ���� outline �� ���ִ� ��
    public void ClearHitList()
    {
        foreach (GameObject go in hitObjects)
        {
            go.GetComponent<Outline>().OutlineWidth = 0;
        }
        hitObjects.Clear();
    }

    // Done ��ư ������ �� ����Ǵ� �Լ�
    void SetChangeTransform()
    {
        // UI ����
        ClearHitList();
        objTransformUI.SetActive(false);
        normal_BC.SetActive(true);
        selected_BC.SetActive(false);
        PublicObjectManager.instance.imageChangeUI.gameObject.SetActive(false);

        // �θ� ����
        GameObject go = selectedObjParent.transform.GetChild(0).gameObject;
        go.transform.parent = createObjParent;
    }

    // ������ ������Ʈ Ŭ����
    void ClearFloor()
    {
        // ���̾��Ű â�� ���� index �� �޾ƿ�
        int idx = createObjParent.GetSiblingIndex();

        // ������ ������Ʈ �θ� �ı�
        Destroy(createObjParent.gameObject);

        // ���ο� ������Ʈ �θ� ����
        GameObject empty = new GameObject();
        empty.name = "CreateObjects";
        empty.transform.SetSiblingIndex(idx);
        createObjParent = empty.transform;
        PublicObjectManager.instance.createObjectParent = empty.transform;

        createdObj.Clear();

        obj_TC.objParent = createObjParent;
    }

    // UI index �� ���� ���� ����
    public void ChangeSelected(int idx)
    {
        for (int i = 0; i < createButtonTexts.Count; ++i)
        {
            if (i == idx) continue;
            createButtonTexts[i].color = Color.white;
            createButtonTexts[i].GetComponent<UnityEngine.UI.Outline>().enabled = true;
        }

        createButtonTexts[idx].color = Color.black;
        createButtonTexts[idx].GetComponent<UnityEngine.UI.Outline>().enabled = false;
    }

    public void ChangeUIList(string s)
    {
        if (s == "All")
        {
            foreach (GameObject go in objPoolUI)
            {
                go.SetActive(true);
            }
        }
        else if (s == "Couch & Chair")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "Couch" || objPool[i].objType == "Chair")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
        else if (s == "Table & Desk")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "Table" || objPool[i].objType == "Desk")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
        else if (s == "Flowers & Plants")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "Flower" || objPool[i].objType == "Plants")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
        else if (s == "Cabinet & Shelves")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "Cabinet" || objPool[i].objType == "Shelves")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
        else if (s == "Frames")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "Frame")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
        else if (s == "ETC")
        {
            for (int i = 0; i < objPool.Count; ++i)
            {
                if (objPool[i].objType == "ETC")
                {
                    objPoolUI[i].SetActive(true);
                }
                else
                {
                    objPoolUI[i].SetActive(false);
                }
            }
        }
    }
    #endregion
}
