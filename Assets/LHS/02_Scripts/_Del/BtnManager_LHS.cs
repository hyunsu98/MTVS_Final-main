using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManager_LHS : MonoBehaviour
{
    //동적으로 버튼에 기능을 넣어보기-------------------
    public Button[] btn_Hat;
    public Button[] btn_Hair;
    public Button[] btn_Jacket;
    public Button[] btn_Chest;
    public Button[] btn_Tie;
    public Button[] btn_Legs;
    public Button[] btn_Feet;
    //------------------------------------------------

    public Button but_Save;
    public Button but_Load;

    FemaleTPPrefabMaker_LHS info;

    // 전역 변수로 선언
    // 사장님이 이런 결재서류 나중에 써야된다고 회의때 말함;;
    Index_Model index_model;

    // 결재서류 양식 만들기
    // Json형식을 사용할 틀 클래스는 Serializable을 넣어줘야함
    [System.Serializable]
    public class Index_Model
    {
        public int hairNum;
        public int jacketNum;
        public int chestNum;
        public int tieNum;
        public int legsNum;
        public int feetNum;
    }

    void Start()
    {
        // 시작할 때 생성은 해줘야 함
        // 결재서류 쓰라고 받음.. ㅠ
        index_model = new Index_Model();

        // 플레이어의 자식에 있는 스크립트를 들고오자
        info = GameObject.Find("Player").GetComponentInChildren<FemaleTPPrefabMaker_LHS>();

        btn_Hat[0].onClick.AddListener(info.Prevhat);
        btn_Hat[1].onClick.AddListener(info.Nexthat);

        btn_Hair[0].onClick.AddListener(info.Prevhair);
        btn_Hair[1].onClick.AddListener(info.Nexthair);

        btn_Jacket[0].onClick.AddListener(info.Prevjacket);
        btn_Jacket[1].onClick.AddListener(info.Nextjacket);

        btn_Chest[0].onClick.AddListener(info.Prevchest);
        btn_Chest[1].onClick.AddListener(info.Nextchest);

        btn_Tie[0].onClick.AddListener(info.Prevtie);
        btn_Tie[1].onClick.AddListener(info.Nexttie);

        btn_Legs[0].onClick.AddListener(info.Prevlegs);
        btn_Legs[1].onClick.AddListener(info.Nextlegs);

        btn_Feet[0].onClick.AddListener(info.Prevfeet);
        btn_Feet[1].onClick.AddListener(info.Nextfeet);

        //함수 실행
        but_Save.onClick.AddListener(() => SaveModel(index_model));
        but_Load.onClick.AddListener(() => LoadModel(index_model));
    }

    void Update()
    {
        //TEST
        // 1번을 눌렀을 때, 결재서류 안에 값 저장하기
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 그 값으로 실행
            SaveModel(index_model);
        }

        // 2번을 눌렀을 때, 저장한 모델로 불러오기
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadModel(index_model);
        }
    }

    // 결재서류의 양식에 맞게 값을 작성
    // 클래스 형태 저장
    void SaveModel(Index_Model index)
    {
        // -------------------------------HAIR-------------------------
        // 결제서류에 hair 값을 저장
        index.hairNum = info.hair;
        //print($" HAIR (HAT 7~9) = {info.hair} 번 저장");
        // ------------------------------------------------------------

        // -------------------------------JACKET-----------------------
        // 결제서류에 hair 값을 저장
        index.jacketNum = info.jacket;
        //print($" JACKET (0~2) = {info.jacket} 번 저장");
        // ------------------------------------------------------------

        // -------------------------------CHEST------------------------
        // 결제서류에 hair 값을 저장
        index.chestNum = info.chest;
        //print($" CHEST (0~8) = {info.chest} 번 저장");
        // -----------------------------------------------------------

        // -------------------------------TIE-------------------------
        // 결제서류에 hair 값을 저장
        index.tieNum = info.tie;
        //print($" TIE (0~3) = {info.tie} 번 저장");
        // ------------------------------------------------------------

        // -------------------------------LEGS-------------------------
        // 결제서류에 hair 값을 저장
        index.legsNum = info.legs;
        //print($" LEGS (0~3) = {info.legs} 번 저장");
        // ------------------------------------------------------------

        // -------------------------------FEET-------------------------
        // 결제서류에 hair 값을 저장
        index.feetNum = info.feet;
        //print($" FEET = {info.feet} 번 저장");
        // ------------------------------------------------------------

        print($" SAVE : HAIR(HAT 7~9)= {index.hairNum}, JACKET(0~2)= {index.jacketNum}, CHEST(0~8)= {index.chestNum}, TIE(0~3= {index.tieNum}, LEGS(0~3)= {index.legsNum}, FEET= {index.feetNum}번 저장 ");
    }

    // 결재서류 제출~~
    void LoadModel(Index_Model index)
    {

        // -------------------------------HAIR-------------------------
        info.GOhair[info.hair].SetActive(false);
        info.hair = index.hairNum;
        info.GOhair[info.hair].SetActive(true);
        //print($" HAIR (모자번호 7~9) = {index.hatNum}번 로드");
        // ------------------------------------------------------------

        // -------------------------------JACKET-----------------------
        // 저장 값이 활성화 상태라면 
        if (index.jacketNum != 2)
        {
            //현재 값이 활성화
            if (info.jacket != 2)
            {
                // 꺼주고 켜준다
                info.GOjackets[info.jacket].SetActive(false);
                info.jacket = index.jacketNum;
                info.GOjackets[info.jacket].SetActive(true);
            }

            //현재 값 비활성화
            else
            {
                info.jacket = index.jacketNum;
                info.GOjackets[info.jacket].SetActive(true);

            }
        }

        // 저장 값이 비활성화 상태라면 
        else
        {
            //현재 값이 활성화
            if (info.jacket != 2)
            {
                // 꺼주고 켜준다
                info.GOjackets[info.jacket].SetActive(false);
            }

            //현재 값 비활성화
            else
            {
                info.jacket = index.jacketNum;
            }
        }

        // ------------------------------------------------------------

        // -------------------------------CHEST------------------------
        info.GOchest[info.chest].SetActive(false);
        info.chest = index.chestNum;
        info.GOchest[info.chest].SetActive(true);
        // -----------------------------------------------------------

        // -------------------------------TIE-------------------------
        // 저장 값이 활성화 상태라면 
        if (index.tieNum != 3)
        {
            //현재 값이 활성화
            if (info.tie != 3)
            {
                // 꺼주고 켜준다
                info.GOties[info.tie].SetActive(false);
                info.tie = index.tieNum;
                info.GOties[info.tie].SetActive(true);
            }

            //현재 값 비활성화
            else
            {
                info.tie = index.tieNum;
                info.GOties[info.tie].SetActive(true);

            }
        }

        // 저장 값이 비활성화 상태라면 
        else
        {
            //현재 값이 활성화
            if (info.tie != 3)
            {
                // 꺼주고 켜준다
                info.GOties[info.tie].SetActive(false);
            }

            //현재 값 비활성화
            else
            {
                info.tie = index.tieNum;
            }
        }
        // ------------------------------------------------------------

        // -------------------------------LEGS-------------------------
        info.GOlegs[info.legs].SetActive(false);
        info.legs = index.legsNum;
        info.GOlegs[info.legs].SetActive(true);
        // ------------------------------------------------------------

        // -------------------------------FEET-------------------------
        info.GOfeet[info.feet].SetActive(false);
        info.feet = index.feetNum;
        info.GOfeet[info.feet].SetActive(true);
        //print($" FEET = {info.feet}번 로드");
        // ------------------------------------------------------------

        print($" LOAD : HAIR(HAT 7~9)= {info.hair}, JACKET(0~2)= {info.jacket}, CHEST(0~8)= {info.chest}, TIE(0~3= {info.tie}, LEGS(0~3)= {info.legs}, FEET= {info.feet}번 로드 ");
    }
}
