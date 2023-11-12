using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManager_LHS : MonoBehaviour
{
    //�������� ��ư�� ����� �־��-------------------
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

    // ���� ������ ����
    // ������� �̷� ���缭�� ���߿� ��ߵȴٰ� ȸ�Ƕ� ����;;
    Index_Model index_model;

    // ���缭�� ��� �����
    // Json������ ����� Ʋ Ŭ������ Serializable�� �־������
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
        // ������ �� ������ ����� ��
        // ���缭�� ����� ����.. ��
        index_model = new Index_Model();

        // �÷��̾��� �ڽĿ� �ִ� ��ũ��Ʈ�� ������
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

        //�Լ� ����
        but_Save.onClick.AddListener(() => SaveModel(index_model));
        but_Load.onClick.AddListener(() => LoadModel(index_model));
    }

    void Update()
    {
        //TEST
        // 1���� ������ ��, ���缭�� �ȿ� �� �����ϱ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // �� ������ ����
            SaveModel(index_model);
        }

        // 2���� ������ ��, ������ �𵨷� �ҷ�����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadModel(index_model);
        }
    }

    // ���缭���� ��Ŀ� �°� ���� �ۼ�
    // Ŭ���� ���� ����
    void SaveModel(Index_Model index)
    {
        // -------------------------------HAIR-------------------------
        // ���������� hair ���� ����
        index.hairNum = info.hair;
        //print($" HAIR (HAT 7~9) = {info.hair} �� ����");
        // ------------------------------------------------------------

        // -------------------------------JACKET-----------------------
        // ���������� hair ���� ����
        index.jacketNum = info.jacket;
        //print($" JACKET (0~2) = {info.jacket} �� ����");
        // ------------------------------------------------------------

        // -------------------------------CHEST------------------------
        // ���������� hair ���� ����
        index.chestNum = info.chest;
        //print($" CHEST (0~8) = {info.chest} �� ����");
        // -----------------------------------------------------------

        // -------------------------------TIE-------------------------
        // ���������� hair ���� ����
        index.tieNum = info.tie;
        //print($" TIE (0~3) = {info.tie} �� ����");
        // ------------------------------------------------------------

        // -------------------------------LEGS-------------------------
        // ���������� hair ���� ����
        index.legsNum = info.legs;
        //print($" LEGS (0~3) = {info.legs} �� ����");
        // ------------------------------------------------------------

        // -------------------------------FEET-------------------------
        // ���������� hair ���� ����
        index.feetNum = info.feet;
        //print($" FEET = {info.feet} �� ����");
        // ------------------------------------------------------------

        print($" SAVE : HAIR(HAT 7~9)= {index.hairNum}, JACKET(0~2)= {index.jacketNum}, CHEST(0~8)= {index.chestNum}, TIE(0~3= {index.tieNum}, LEGS(0~3)= {index.legsNum}, FEET= {index.feetNum}�� ���� ");
    }

    // ���缭�� ����~~
    void LoadModel(Index_Model index)
    {

        // -------------------------------HAIR-------------------------
        info.GOhair[info.hair].SetActive(false);
        info.hair = index.hairNum;
        info.GOhair[info.hair].SetActive(true);
        //print($" HAIR (���ڹ�ȣ 7~9) = {index.hatNum}�� �ε�");
        // ------------------------------------------------------------

        // -------------------------------JACKET-----------------------
        // ���� ���� Ȱ��ȭ ���¶�� 
        if (index.jacketNum != 2)
        {
            //���� ���� Ȱ��ȭ
            if (info.jacket != 2)
            {
                // ���ְ� ���ش�
                info.GOjackets[info.jacket].SetActive(false);
                info.jacket = index.jacketNum;
                info.GOjackets[info.jacket].SetActive(true);
            }

            //���� �� ��Ȱ��ȭ
            else
            {
                info.jacket = index.jacketNum;
                info.GOjackets[info.jacket].SetActive(true);

            }
        }

        // ���� ���� ��Ȱ��ȭ ���¶�� 
        else
        {
            //���� ���� Ȱ��ȭ
            if (info.jacket != 2)
            {
                // ���ְ� ���ش�
                info.GOjackets[info.jacket].SetActive(false);
            }

            //���� �� ��Ȱ��ȭ
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
        // ���� ���� Ȱ��ȭ ���¶�� 
        if (index.tieNum != 3)
        {
            //���� ���� Ȱ��ȭ
            if (info.tie != 3)
            {
                // ���ְ� ���ش�
                info.GOties[info.tie].SetActive(false);
                info.tie = index.tieNum;
                info.GOties[info.tie].SetActive(true);
            }

            //���� �� ��Ȱ��ȭ
            else
            {
                info.tie = index.tieNum;
                info.GOties[info.tie].SetActive(true);

            }
        }

        // ���� ���� ��Ȱ��ȭ ���¶�� 
        else
        {
            //���� ���� Ȱ��ȭ
            if (info.tie != 3)
            {
                // ���ְ� ���ش�
                info.GOties[info.tie].SetActive(false);
            }

            //���� �� ��Ȱ��ȭ
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
        //print($" FEET = {info.feet}�� �ε�");
        // ------------------------------------------------------------

        print($" LOAD : HAIR(HAT 7~9)= {info.hair}, JACKET(0~2)= {info.jacket}, CHEST(0~8)= {info.chest}, TIE(0~3= {info.tie}, LEGS(0~3)= {info.legs}, FEET= {info.feet}�� �ε� ");
    }
}
