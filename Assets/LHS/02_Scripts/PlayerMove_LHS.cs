using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;


public class PlayerMove_LHS : MonoBehaviourPun
{
    [Header("남/녀")]
    public GameObject female;
    public GameObject male;

    [Header("닉네임 UI")]
    public Text nickName;
    public Text nickName2;

    [Header("이동 속도")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed;

    [Header("점프 힘")]
    public float jumpPower = 5;

    #region 점프 관련 변수
    // 중력 / 수직속도
    float gravity = -20;
    float yVelocity = 0;

    public bool isJumping = false;
    #endregion

    [Header("둘러보기 회전 속도")]
    public float smoothness = 10f;
    [Header("둘러보기 가능 여부")]
    public bool toggleCameraRotation;
    [Header("달리기")]
    public bool run;

    Vector3 moveDirection;

    Animator anim;
    Camera cam;
    CharacterController cc;

    // [구현 예정] 텔레포트 이동
    private Vector3 destination;

    [SerializeField]
    public bool isMove;

    // [사용 안함] 복원 오브제 아웃라인
    Transform objectTree;

    //커스텀을 위한
    FemaleTPPrefabMaker_LHS femaleInfo;
    MaleTPPrefabMaker_LHS maleInfo;

    string jsonData;

    //public static PlayerMove_LHS instance;

    //private void Awake()
    //{
    //    //instance에 나 자신을 넣는다.
    //    instance = this;
    //}

   public bool genderCheck;

    void Start()
    {

        //닉네임 설정
        nickName.text = photonView.Owner.NickName;
        nickName2.text = photonView.Owner.NickName;

        //커스텀을 위한
        femaleInfo = GetComponentInChildren<FemaleTPPrefabMaker_LHS>();
        maleInfo = GetComponentInChildren<MaleTPPrefabMaker_LHS>();

        //커스텀 GET
        OnGetPost();

        //anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
        cc = GetComponentInChildren<CharacterController>();

        isMove = true;

        //photonView.RPC("LoadJson", RpcTarget.All, PhotonNetwork.NickName);
        //photonView.RPC("LoadJson", RpcTarget.All, gameObject.name);
        //photonView.RPC("OnGetPost", RpcTarget.All);
    }

    void Update()
    {
        //여자라면
        if (genderCheck == true)
        {
            anim = female.GetComponent<Animator>();

            //print("여자");
        }

        else if (genderCheck == false)
        {
            anim = male.GetComponent<Animator>();

            //print("남자");
        }

        //if (photonView.IsMine == false) return;
        if (photonView.IsMine == true)
        {
            if (Input.GetKeyDown(KeyCode.F4))
            {
                MouseUI_LHS.instance.isChat = false;
            }


            if (MouseUI_LHS.instance.isChat == false)
            {
                #region [카메라] 플레이어 중심으로 둘러보기
                //둘러보기 활성화
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    toggleCameraRotation = true;
                }
                //둘러보기 비활성화
                else
                {
                    toggleCameraRotation = false;
                }
                #endregion

                #region 달리기
                //달리기 활성화
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    run = true;
                }
                //달리기 비활성화
                else
                {
                    run = false;
                }
                #endregion

                //기본 움직임
                InputMovement();

                //감정 표현
                Expression();

                // 만약 플레이어 중심으로 둘러보기가 비활성화 되있다면
                if (toggleCameraRotation != true)
                {
                    // 카메라 앞방향 기준으로 변환
                    // Scale : 두개의 Vector3의 값을 곱해줌
                    Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
                    // Slerp : 구면 선형 보간 
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
                }

                
            }

        //만약 내것이 아니라면 함수를 나가겠다.

        //if (Input.GetKeyDown(KeyCode.F4))
        //{
        //    print("찍음");
        //    isMove = false;
        //}

        // 마우스 커서가 없다면 움직임 실행
        //if (PublicObjectManager.instance.chatIcon.gameObject.activeSelf == false) return;
        //if (PublicObjectManager.instance.chatAIWindow.localScale.magnitude >= 1f && isMove) return;

       
        }



        #region  [구현 예정] 텔레포트 이동
        ////클릭한 위치로 캐릭터 이동시키는 기능
        //if (Input.GetMouseButton(1))
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        //    {
        //        SetDestination(hit.point);
        //    }
        //}

        //Move();
        #endregion


        #region [사용 안함] 복원 오브제 아웃라인
        // 레이를 쏴서 Tree를 맞는다면 아웃라인을 그려준다.

        //RaycastHit hitInfo;

        //if (Physics.Raycast(transform.position, transform.forward, out hitInfo, MaxDistance, (1 << 8)))
        //{
        //    objectTree = GameObject.Find("purpleTree").GetComponent<Transform>();
        //    float dist = Vector3.Distance(transform.position, objectTree.position);

        //    Outline outline = hitInfo.transform.GetComponent<Outline>();
        //    if (outline != null)
        //    {
        //        GameObject go = outline.gameObject;

        //        // 거리 조건
        //        if (dist <= 3)
        //        {
        //            outline.OutlineWidth = 5f;
        //        }

        //        else
        //        {
        //            outline.OutlineWidth = 0f;
        //        }
        //    }
        //}
        #endregion
    }

    private void LateUpdate()
    {
        ////만약 내것이 아니라면 함수를 나가겠다.
        //if (photonView.IsMine == false) return;

        //// 만약 플레이어 중심으로 둘러보기가 비활성화 되있다면
        //if (toggleCameraRotation != true)
        //{
        //    // 카메라 앞방향 기준으로 변환
        //    // Scale : 두개의 Vector3의 값을 곱해줌
        //    Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
        //    // Slerp : 구면 선형 보간 
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        //}
    }

    void InputMovement()
    {
        //isMove = true;

        //만약에 뛴다면 달리는 속도 아니라면 기본 속도
        //run이라는 조건이 참이면 runSpeed 값 / 거짓이면 speed 값이 할당
        finalSpeed = (run) ? runSpeed : speed;

        #region [사용 안함] 기본 이동 방법
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        //Vector3 dir = new Vector3(h, 0, v);

        //// 메인 카메라 기준으로 방향을 변환한다
        //dir = Camera.main.transform.TransformDirection(dir);
        #endregion

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // 방향키를 눌렀을 때, 이동하는 방향을 바라보게 
        anim.transform.forward = Vector3.Slerp(anim.transform.forward, moveDirection, 20f * Time.deltaTime);

        // 만약 움직이지 않는다면 다시 카메라를 바라보게 한다.
        if (moveDirection.magnitude <= 0.1f)
        {
            //anim.transform.forward = Vector3.Slerp(anim.transform.forward, transform.Find("FollowCam").transform.forward, 10f * Time.deltaTime);

            anim.transform.forward = transform.Find("FollowCam").transform.forward;
        }

        else
        {
            CameraMovement_LHS.instance.cameraMove = true;
        }

        #region 점프
        yVelocity += gravity * Time.deltaTime;

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                yVelocity = 0;
                isJumping = false;
                anim.SetBool("isJump", false);
            }
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
            anim.SetBool("isJump", true);
        }

        //if (Input.GetKeyDown(KeyCode.J) && !isJumping)
        //{
        //    yVelocity = jumpPower;
        //    isJumping = true;
        //    anim.SetBool("isJump", true);
        //}

        moveDirection.y = yVelocity;
        #endregion

        cc.Move(moveDirection * finalSpeed * Time.deltaTime);

        #region Move Blend 애니메이션 
        // 0 : Idle / 0.5 : Walk / 1 : Run
        // 움직이는 방향 / y는 0으로 만들기
        Vector3 tempDir = moveDirection;
        tempDir.y = 0;

        // run이 참이라면 1 / 거짓이라면 0.5f
        // magnitude 거리 (크기)
        float percent = ((run) ? 1 : 0.5f) * tempDir.normalized.magnitude;

        percent = percent < 0.5f ? 0 : percent;

        //if (percent <= 1)
        //{
        //    percent = percent < 0.5f ? 0 : percent;
        //}

        anim.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
        #endregion
    }

    #region  [구현 예정] 텔레포트 이동
    //private void SetDestination(Vector3 dest)
    //{
    //    destination = dest;
    //    isMove = true;
    //}

    //private void Move()
    //{
    //    if (isMove)
    //    {
    //        var dir = destination - transform.position;

    //        //transform.position += dir.normalized * Time.deltaTime * 5f;
    //        //cc.Move(dir * finalSpeed * Time.deltaTime);
    //    }

    //    if (Vector3.Distance(transform.position, destination) <= 0.1f)
    //    {
    //        isMove = false;
    //    }
    //}
    #endregion

    // 감정표현
    void Expression()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("Clap");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("Wave");
        }
    }

    //File sJson
    [PunRPC]
    void LoadJson(string nick)
    {
        //jsonData = File.ReadAllText(Application.streamingAssetsPath + "/" + nick + ".txt");

        //방 주인의 이름의 파일을 찾는다!
        //결국 내꺼만 바꿔주는 것임..! -> 다른 사람들은 Player(clone)이기 때문에
        jsonData = File.ReadAllText(Application.streamingAssetsPath + "/" + photonView.Owner.NickName + ".txt");

        //ArrayJson 형태로 Json을 변환
        CustomInfoList customInfoList = JsonUtility.FromJson<CustomInfoList>(jsonData);

        //ArrayJson의 데이터를 가지고 오브젝트 생성 //리스트인 경우
        for (int i = 0; i < customInfoList.data.Count; i++)
        {
            CustomInfo info = customInfoList.data[i];

            LoadModel(info.nickName, info.gender, info.hairNum, info.jacketNum, info.chestNum, info.tieNum, info.legsNum, info.feetNum);
        }

        print("조회 완료");
    }

    // 커스텀 로드
    [PunRPC]
    public void OnGetPost()
    {
        string url = "/character/find/";
        url += photonView.Owner.NickName;
        //url += HttpManager.instance.username;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnGetPostComplete(DownloadHandler result)
    {
        print(result.text);
        //print("불러오기 성공");

        // 캐릭터 커스텀 담기
        HttpCharacterData characterData = new HttpCharacterData();
        characterData = JsonUtility.FromJson<HttpCharacterData>(result.text);

        LoadModel(characterData.results.nickName, characterData.results.gender, characterData.results.hairNum, characterData.results.jacketNum, characterData.results.chestNum, characterData.results.tieNum, characterData.results.legsNum, characterData.results.feetNum);
    }

    void OnGetFailed()
    {
        print("불러오기 실패!");
    }

    // 커스텀 로드!
    void LoadModel(string nickName, int gender, int hairNum, int jacketNum, int chestNum, int tieNum, int legsNum, int feetNum)
    {
        //여자라면
        if (gender == 1)
        {
            genderCheck = true;

            print("커스텀 여자 정보 확인");

            male.SetActive(false);
            female.SetActive(true);

            // -------------------------------HAIR-------------------------
            femaleInfo.GOhair[femaleInfo.hair].SetActive(false);
            femaleInfo.hair = hairNum;
            femaleInfo.GOhair[femaleInfo.hair].SetActive(true);
            //print($" HAIR (모자번호 7~9) = {index.hatNum}번 로드");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // 저장 값이 활성화 상태라면 
            if (jacketNum != 2)
            {
                //현재 값이 활성화
                if (femaleInfo.jacket != 2)
                {
                    // 꺼주고 켜준다
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(false);
                    femaleInfo.jacket = jacketNum;
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(true);
                }

                //현재 값 비활성화
                else
                {
                    femaleInfo.jacket = jacketNum;
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(true);

                }
            }

            // 저장 값이 비활성화 상태라면 
            else
            {
                //현재 값이 활성화
                if (femaleInfo.jacket != 2)
                {
                    // 꺼주고 켜준다
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(false);
                }

                //현재 값 비활성화
                else
                {
                    femaleInfo.jacket = jacketNum;
                }
            }
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            femaleInfo.GOchest[femaleInfo.chest].SetActive(false);
            femaleInfo.chest = chestNum;
            femaleInfo.GOchest[femaleInfo.chest].SetActive(true);
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // 저장 값이 활성화 상태라면 
            if (tieNum != 3)
            {
                //현재 값이 활성화
                if (femaleInfo.tie != 3)
                {
                    // 꺼주고 켜준다
                    femaleInfo.GOties[femaleInfo.tie].SetActive(false);
                    femaleInfo.tie = tieNum;
                    femaleInfo.GOties[femaleInfo.tie].SetActive(true);
                }

                //현재 값 비활성화
                else
                {
                    femaleInfo.tie = tieNum;
                    femaleInfo.GOties[femaleInfo.tie].SetActive(true);

                }
            }

            // 저장 값이 비활성화 상태라면 
            else
            {
                //현재 값이 활성화
                if (femaleInfo.tie != 3)
                {
                    // 꺼주고 켜준다
                    femaleInfo.GOties[femaleInfo.tie].SetActive(false);
                }

                //현재 값 비활성화
                else
                {
                    femaleInfo.tie = tieNum;
                }
            }
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            femaleInfo.GOlegs[femaleInfo.legs].SetActive(false);
            femaleInfo.legs = legsNum;
            femaleInfo.GOlegs[femaleInfo.legs].SetActive(true);
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            femaleInfo.GOfeet[femaleInfo.feet].SetActive(false);
            femaleInfo.feet = feetNum;
            femaleInfo.GOfeet[femaleInfo.feet].SetActive(true);
            //print($" FEET = {info.feet}번 로드");
            // ------------------------------------------------------------

            print($" LOAD : HAIR(HAT 7~9)= {femaleInfo.hair}, JACKET(0~2)= {femaleInfo.jacket}, CHEST(0~8)= {femaleInfo.chest}, TIE(0~3= {femaleInfo.tie}, LEGS(0~3)= {femaleInfo.legs}, FEET= {femaleInfo.feet}번 로드 ");
        }

        else if (gender == 2)
        {
            genderCheck = false;

            female.SetActive(false);
            male.SetActive(true);

            // -------------------------------HAIR-------------------------
            maleInfo.GOhair[maleInfo.hair].SetActive(false);
            maleInfo.hair = hairNum;
            maleInfo.GOhair[maleInfo.hair].SetActive(true);
            //print($" HAIR (모자번호 7~9) = {index.hatNum}번 로드");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // 저장 값이 활성화 상태라면 
            if (jacketNum != 2)
            {
                //현재 값이 활성화
                if (maleInfo.jacket != 2)
                {
                    // 꺼주고 켜준다
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(false);
                    maleInfo.jacket = jacketNum;
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(true);
                }

                //현재 값 비활성화
                else
                {
                    maleInfo.jacket = jacketNum;
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(true);

                }
            }

            // 저장 값이 비활성화 상태라면 
            else
            {
                //현재 값이 활성화
                if (maleInfo.jacket != 2)
                {
                    // 꺼주고 켜준다
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(false);
                }

                //현재 값 비활성화
                else
                {
                    maleInfo.jacket = jacketNum;
                }
            }
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            maleInfo.GOchest[maleInfo.chest].SetActive(false);
            maleInfo.chest = chestNum;
            maleInfo.GOchest[maleInfo.chest].SetActive(true);
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // 저장 값이 활성화 상태라면 
            if (tieNum != 3)
            {
                //현재 값이 활성화
                if (maleInfo.tie != 3)
                {
                    // 꺼주고 켜준다
                    maleInfo.GOties[maleInfo.tie].SetActive(false);
                    maleInfo.tie = tieNum;
                    maleInfo.GOties[maleInfo.tie].SetActive(true);
                }

                //현재 값 비활성화
                else
                {
                    maleInfo.tie = tieNum;
                    maleInfo.GOties[maleInfo.tie].SetActive(true);

                }
            }

            // 저장 값이 비활성화 상태라면 
            else
            {
                //현재 값이 활성화
                if (maleInfo.tie != 3)
                {
                    // 꺼주고 켜준다
                    maleInfo.GOties[maleInfo.tie].SetActive(false);
                }

                //현재 값 비활성화
                else
                {
                    maleInfo.tie = tieNum;
                }
            }
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            maleInfo.GOlegs[maleInfo.legs].SetActive(false);
            maleInfo.legs = legsNum;
            maleInfo.GOlegs[maleInfo.legs].SetActive(true);
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            maleInfo.GOfeet[maleInfo.feet].SetActive(false);
            maleInfo.feet = feetNum;
            maleInfo.GOfeet[maleInfo.feet].SetActive(true);
            //print($" FEET = {info.feet}번 로드");
            // ------------------------------------------------------------
        }
    }
}
