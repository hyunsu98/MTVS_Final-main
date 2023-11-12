using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Networking;
using System.IO;

public class ObjectManager : MonoBehaviourPun
{
    // 설치할 Object List 정보
    [Header("< 꾸미기 아이템 >")]
    public List<ObjectInfo> objPool = new List<ObjectInfo>();
    public List<GameObject> objPoolUI = new List<GameObject>();
    // 설치 오브젝트 목록 인덱스
    public int itemIndex;

    // 오브젝트 부모
    Transform createObjParent;   // 생성된 오브젝트의 부모
    Transform selectedObjParent; // 선택된 오브젝트의 부모

    [Header("< Ray 최대 거리 >")]
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

    [Header("< 설치된 오브젝트 >")]
    // 설치된 ObjectList
    public List<CreatedInfo> createdObj = new List<CreatedInfo>();
    // 선택된 오브젝트 index
    int selectedIndex;

    ObjectTransformControl obj_TC;

    // Ray 에 닿은 오브젝트
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

    #region Object 관련(생성, 삭제, 선택)
    // 로드
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

    // 생성
    public void CreateObject()
    {
        LoadObject(itemIndex, Vector3.zero, 0f, 1f);
    }

    // 삭제
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

    // 오브젝트 선택
    public void SelectObject()
    {
        // 마우스 우클릭을 하면 선택한 오브젝트로 지정하고 싶다.
        // 1. 마우스 우클릭을 하면
        if (Input.GetMouseButtonDown(1))
        {
            // 이미 선택된 오브젝트가 있으면 우클릭 막아주기
            if (selectedObjParent.childCount != 0) return;
            // 2. 레이를 쏴서
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 3. 선택한 오브젝트가 생성된 오브젝트인지 확인
                // -> 부모를 체크
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

                        // 4. 선택한 오브젝트의 부모를 변경
                        hitInfo.transform.parent = selectedObjParent;

                        // 5. 선택한 오브젝트 세팅
                        obj_TC.MySetChild();
                    }

                    // 6. UI 켜기
                    objTransformUI.SetActive(true);
                    normal_BC.SetActive(false);
                    selected_BC.SetActive(true);

                    // 선택한 오브젝트가 Frame 이면
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Frame"))
                    {
                        // Image Change UI 켜주기
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

    #region Json 저장
    public void SaveJson()
    {
        if (RoomManager.instance.room.memberNickname != HttpManager.instance.nickname) return;

        // map.createObjects 에 있는 정보를 json 으로 변환
        // map.createObjects 의 개수만큼 SaveJsonInfo 만들어서 세팅
        SaveJsonInfo info;

        // ArrayJson 만든다
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
            // ArrayJson 하나 만들어서 datas 에 하나씩 추가
            arrayJson.datas.Add(info);
        }

        // arrayJson 을 Json 으로 변환
        string jsonData = JsonUtility.ToJson(arrayJson, true);
        Debug.Log(jsonData);

        // 네트워크로 보내기
        OnPost(jsonData);
    }
    #endregion

    #region 네트워크 (Post, Get)
    public void OnPost(string s)
    {
        string url = "/object/";
        url += RoomManager.instance.room.id;

        // roomid 필요
        //url += HttpManager.instance.username;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        //print($"캐릭터 로드 이름 + {HttpManager.instance.username}");

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    #region Post, Get 실패 or 성공 Log
    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("오브젝트 정보 보내기 성공");
    }

    void OnPostFailed()
    {
        print("오브젝트 정보 보내기 실패!");
    }

    void OnGetComplete(DownloadHandler result)
    {
        print("오브젝트 불러오기 성공");

        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(result.text);

        for (int i = 0; i < arrayJson.datas.Count; ++i)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            LoadObject(info.idx, info.position, info.angle, info.scaleValue);
        }
    }

    void OnGetFailed()
    {
        print("오브젝트 불러오기 실패");
    }
    #endregion

    #endregion

    #region 기타 함수
    // 기존에 hit 한 object 들의 outline 을 꺼주는 것
    public void ClearHitList()
    {
        foreach (GameObject go in hitObjects)
        {
            go.GetComponent<Outline>().OutlineWidth = 0;
        }
        hitObjects.Clear();
    }

    // Done 버튼 눌렀을 때 실행되는 함수
    void SetChangeTransform()
    {
        // UI 변경
        ClearHitList();
        objTransformUI.SetActive(false);
        normal_BC.SetActive(true);
        selected_BC.SetActive(false);
        PublicObjectManager.instance.imageChangeUI.gameObject.SetActive(false);

        // 부모 변경
        GameObject go = selectedObjParent.transform.GetChild(0).gameObject;
        go.transform.parent = createObjParent;
    }

    // 생성된 오브젝트 클리어
    void ClearFloor()
    {
        // 하이어라키 창의 순서 index 를 받아옴
        int idx = createObjParent.GetSiblingIndex();

        // 생성된 오브젝트 부모 파괴
        Destroy(createObjParent.gameObject);

        // 새로운 오브젝트 부모 설정
        GameObject empty = new GameObject();
        empty.name = "CreateObjects";
        empty.transform.SetSiblingIndex(idx);
        createObjParent = empty.transform;
        PublicObjectManager.instance.createObjectParent = empty.transform;

        createdObj.Clear();

        obj_TC.objParent = createObjParent;
    }

    // UI index 를 통한 색상 변경
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
