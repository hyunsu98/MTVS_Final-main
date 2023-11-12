using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// 설치 했을때의 정보
[System.Serializable]
public class CustomInfoList
{
    //선택된 오브젝트의 idx
    public List<CustomInfo> data;
}

// 결재서류 양식 만들기
[System.Serializable]
public class CustomInfo
{
    // public int userId;
    public string nickName;
    public int gender;
    public int hairNum;
    public int jacketNum;
    public int chestNum;
    public int tieNum;
    public int legsNum;
    public int feetNum;
}

public class BtnManager_LHS1 : MonoBehaviour
{
    [Header("남/녀")]
    public GameObject female;
    public GameObject male;

    public GameObject customFemale;
    public GameObject customMale;

    bool isFemale = true;
    bool isMale = true;

    [Header("커스텀 버튼")]
    //동적으로 버튼에 기능을 넣어보기-------------------
    public Button[] btn_Hat;
    public Button[] btn_Hair;
    public Button[] btn_Jacket;
    public Button[] btn_Chest;
    public Button[] btn_Tie;
    public Button[] btn_Legs;
    public Button[] btn_Feet;

    public Button[] btn_gender;

    public Button[] btn_HatM;
    public Button[] btn_HairM;
    public Button[] btn_JacketM;
    public Button[] btn_ChestM;
    public Button[] btn_TieM;
    public Button[] btn_LegsM;
    public Button[] btn_FeetM;
    //------------------------------------------------

    [Header("저장 / 로드 버튼")]
    public Button but_Save;
    public Button but_Load;

    FemaleTPPrefabMaker_LHS femaleInfo;
    MaleTPPrefabMaker_LHS maleInfo;

    public GameObject player;

    [Header("Json 데이터")]
    //전체 보낼 데이터 생성
    //만들어진 오브젝트들 담아놓을 리스트
    public List<CustomInfo> customList = new List<CustomInfo>();

    //json 데이터
    string jsonData;

    void Start()
    {

        // 플레이어의 자식에 있는 스크립트를 들고오자
        femaleInfo = GameObject.Find("Player").GetComponentInChildren<FemaleTPPrefabMaker_LHS>();
        maleInfo = GameObject.Find("Player").GetComponentInChildren<MaleTPPrefabMaker_LHS>();

        //male.SetActive(false);
        //customMale.SetActive(false);
        //isMale = false;


        //femaleInfo.GetComponentInChildren<FemaleTPPrefabMaker_LHS>();
        //maleInfo.GetComponentInChildren<MaleTPPrefabMaker_LHS>();


        btn_gender[0].onClick.AddListener(() => Female());
        btn_gender[1].onClick.AddListener(() => Male());


        #region 여자 커스텀 버튼 선택 시 -> 함수 실행

        btn_Hat[0].onClick.AddListener(femaleInfo.Prevhat);
        btn_Hat[1].onClick.AddListener(femaleInfo.Nexthat);

        btn_Hair[0].onClick.AddListener(femaleInfo.Prevhair);
        btn_Hair[1].onClick.AddListener(femaleInfo.Nexthair);

        btn_Jacket[0].onClick.AddListener(femaleInfo.Prevjacket);
        btn_Jacket[1].onClick.AddListener(femaleInfo.Nextjacket);

        btn_Chest[0].onClick.AddListener(femaleInfo.Prevchest);
        btn_Chest[1].onClick.AddListener(femaleInfo.Nextchest);

        btn_Tie[0].onClick.AddListener(femaleInfo.Prevtie);
        btn_Tie[1].onClick.AddListener(femaleInfo.Nexttie);

        btn_Legs[0].onClick.AddListener(femaleInfo.Prevlegs);
        btn_Legs[1].onClick.AddListener(femaleInfo.Nextlegs);

        btn_Feet[0].onClick.AddListener(femaleInfo.Prevfeet);
        btn_Feet[1].onClick.AddListener(femaleInfo.Nextfeet);

        #endregion

        #region 남자 커스텀 버튼 선택 시 -> 함수 실행

        btn_HatM[0].onClick.AddListener(maleInfo.Prevhat);
        btn_HatM[1].onClick.AddListener(maleInfo.Nexthat);

        btn_HairM[0].onClick.AddListener(maleInfo.Prevhair);
        btn_HairM[1].onClick.AddListener(maleInfo.Nexthair);

        btn_JacketM[0].onClick.AddListener(maleInfo.Prevjacket);
        btn_JacketM[1].onClick.AddListener(maleInfo.Nextjacket);

        btn_ChestM[0].onClick.AddListener(maleInfo.Prevchest);
        btn_ChestM[1].onClick.AddListener(maleInfo.Nextchest);

        btn_TieM[0].onClick.AddListener(maleInfo.Prevtie);
        btn_TieM[1].onClick.AddListener(maleInfo.Nexttie);

        btn_LegsM[0].onClick.AddListener(maleInfo.Prevlegs);
        btn_LegsM[1].onClick.AddListener(maleInfo.Nextlegs);

        btn_FeetM[0].onClick.AddListener(maleInfo.Prevfeet);
        btn_FeetM[1].onClick.AddListener(maleInfo.Nextfeet);

        #endregion


        #region 저장/로드 버튼 선택 시 -> 함수 실행
        but_Save.onClick.AddListener(() => SaveModel());
        #endregion

        //커스텀 로드
        OnGetPost();
    }

    void Update()
    {

    }

    void Female()
    {
        male.SetActive(false);
        customMale.SetActive(false);
        isMale = false;

        female.SetActive(true);
        customFemale.SetActive(true);
        isFemale = true;
    }

    void Male()
    {
        female.SetActive(false);
        customFemale.SetActive(false);
        isFemale = false;

        male.SetActive(true);
        customMale.SetActive(true);
        isMale = true;
    }

    // 커스텀 저장
    void SaveModel()
    {
        CustomInfo info = new CustomInfo();

        info.nickName = PhotonNetwork.NickName;

        // 젠더
        if(isFemale == true)
        {
            print("여자정보저장한다");

            info.gender = 1;

            // -------------------------------HAIR-------------------------
            // 결제서류에 hair 값을 저장
            info.hairNum = femaleInfo.hair;
            //print($" HAIR (HAT 7~9) = {info.hair} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // 결제서류에 hair 값을 저장
            info.jacketNum = femaleInfo.jacket;
            //print($" JACKET (0~2) = {info.jacket} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            // 결제서류에 hair 값을 저장
            info.chestNum = femaleInfo.chest;
            //print($" CHEST (0~8) = {info.chest} 번 저장");
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // 결제서류에 hair 값을 저장
            info.tieNum = femaleInfo.tie;
            //print($" TIE (0~3) = {info.tie} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            // 결제서류에 hair 값을 저장
            info.legsNum = femaleInfo.legs;
            //print($" LEGS (0~3) = {info.legs} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            // 결제서류에 hair 값을 저장
            info.feetNum = femaleInfo.feet;
            //print($" FEET = {info.feet} 번 저장");
            // ------------------------------------------------------------

            //print($" SAVE : HAIR(HAT 7~9)= {info.hairNum}, JACKET(0~2)= {info.jacketNum}, CHEST(0~8)= {info.chestNum}, TIE(0~3= {info.tieNum}, LEGS(0~3)= {info.legsNum}, FEET= {info.feetNum}번 저장 ");

            jsonData = JsonUtility.ToJson(info, true);

            print($"여자정보저장 + {jsonData}");

            //API Post 방식 -> 바디 Http통신으로 보내기
            OnPost(jsonData);

            //jsonData를 파일로 저장 //경로 , 데이터
            //File.WriteAllText(Application.dataPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
            //File.WriteAllText(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
        }

        else if(isMale == true)
        {
            print("남자정보저장한다");

            info.gender = 2;

            // -------------------------------HAIR-------------------------
            // 결제서류에 hair 값을 저장
            info.hairNum = maleInfo.hair;
            //print($" HAIR (HAT 7~9) = {info.hair} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // 결제서류에 hair 값을 저장
            info.jacketNum = maleInfo.jacket;
            //print($" JACKET (0~2) = {info.jacket} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            // 결제서류에 hair 값을 저장
            info.chestNum = maleInfo.chest;
            //print($" CHEST (0~8) = {info.chest} 번 저장");
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // 결제서류에 hair 값을 저장
            info.tieNum = maleInfo.tie;
            //print($" TIE (0~3) = {info.tie} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            // 결제서류에 hair 값을 저장
            info.legsNum = maleInfo.legs;
            //print($" LEGS (0~3) = {info.legs} 번 저장");
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            // 결제서류에 hair 값을 저장
            info.feetNum = maleInfo.feet;
            //print($" FEET = {info.feet} 번 저장");
            // ------------------------------------------------------------

            //print($" SAVE : HAIR(HAT 7~9)= {info.hairNum}, JACKET(0~2)= {info.jacketNum}, CHEST(0~8)= {info.chestNum}, TIE(0~3= {info.tieNum}, LEGS(0~3)= {info.legsNum}, FEET= {info.feetNum}번 저장 ");

            jsonData = JsonUtility.ToJson(info, true);

            print($"남자정보저장 + {jsonData}");

            //API Post 방식 -> 바디 Http통신으로 보내기
            OnPost(jsonData);

            //jsonData를 파일로 저장 //경로 , 데이터
            //File.WriteAllText(Application.dataPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
            //File.WriteAllText(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
        }


    }

    //커스텀 저장 
    public void OnPost(string s)
    {
        string url = "/character/";
        url += HttpManager.instance.username;
        print($"캐릭터 저장 이름 + {HttpManager.instance.username}");

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester);
    }

    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("캐릭터 저장 성공");
    }

    void OnPostFailed()
    {
        print("캐릭터 저장 실패");

        // 저장 실패 시 수정으로 요청! PUT
        OnPut(jsonData);
    }

    // 커스텀 수정
    public void OnPut(string s)
    {
        print(s);
        string url = "/character/";
        url += HttpManager.instance.username;
        print($"캐릭터 저장 이름 + {HttpManager.instance.username}");

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.PUT, url, true);
        requester.body = s;
        requester.isChat = true;

        requester.onComplete = OnPutComplete;
        requester.onFailed = OnPutFailed;

        //HttpManager에게 요청
        HttpManager.instance.SendRequest(requester);
    }

    void OnPutComplete(DownloadHandler result)
    {
        print(result.text);
        print("캐릭터 수정 성공");
    }

    void OnPutFailed()
    {
        print("캐릭터 수정 실패");
    }

    // 커스텀 로드
    public void OnGetPost()
    {
        string url = "/character/find/";
        url += HttpManager.instance.nickname;
        //url += PhotonNetwork.NickName;
        print($"캐릭터 로드 이름 + {HttpManager.instance.username}");

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
        print("불러오기 성공");

        HttpCharacterData characterData = new HttpCharacterData();
        characterData = JsonUtility.FromJson<HttpCharacterData>(result.text);

        LoadModel(characterData.results.nickName, characterData.results.gender, characterData.results.hairNum, characterData.results.jacketNum, characterData.results.chestNum, characterData.results.tieNum, characterData.results.legsNum, characterData.results.feetNum);
    }

    void OnGetFailed()
    {
        print("불러오기 실패!");
    }

    void LoadModel(string nickName, int gender, int hairNum, int jacketNum, int chestNum, int tieNum, int legsNum, int feetNum)
    {
        //여자라면
        if(gender == 1)
        {
            male.SetActive(false);
            customMale.SetActive(false);
            isMale = false;

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

        else if(gender == 2)
        {
            female.SetActive(false);
            customFemale.SetActive(false);
            isFemale = false;

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
