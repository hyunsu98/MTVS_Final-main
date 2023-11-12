using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// ��ġ �������� ����
[System.Serializable]
public class CustomInfoList
{
    //���õ� ������Ʈ�� idx
    public List<CustomInfo> data;
}

// ���缭�� ��� �����
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
    [Header("��/��")]
    public GameObject female;
    public GameObject male;

    public GameObject customFemale;
    public GameObject customMale;

    bool isFemale = true;
    bool isMale = true;

    [Header("Ŀ���� ��ư")]
    //�������� ��ư�� ����� �־��-------------------
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

    [Header("���� / �ε� ��ư")]
    public Button but_Save;
    public Button but_Load;

    FemaleTPPrefabMaker_LHS femaleInfo;
    MaleTPPrefabMaker_LHS maleInfo;

    public GameObject player;

    [Header("Json ������")]
    //��ü ���� ������ ����
    //������� ������Ʈ�� ��Ƴ��� ����Ʈ
    public List<CustomInfo> customList = new List<CustomInfo>();

    //json ������
    string jsonData;

    void Start()
    {

        // �÷��̾��� �ڽĿ� �ִ� ��ũ��Ʈ�� ������
        femaleInfo = GameObject.Find("Player").GetComponentInChildren<FemaleTPPrefabMaker_LHS>();
        maleInfo = GameObject.Find("Player").GetComponentInChildren<MaleTPPrefabMaker_LHS>();

        //male.SetActive(false);
        //customMale.SetActive(false);
        //isMale = false;


        //femaleInfo.GetComponentInChildren<FemaleTPPrefabMaker_LHS>();
        //maleInfo.GetComponentInChildren<MaleTPPrefabMaker_LHS>();


        btn_gender[0].onClick.AddListener(() => Female());
        btn_gender[1].onClick.AddListener(() => Male());


        #region ���� Ŀ���� ��ư ���� �� -> �Լ� ����

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

        #region ���� Ŀ���� ��ư ���� �� -> �Լ� ����

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


        #region ����/�ε� ��ư ���� �� -> �Լ� ����
        but_Save.onClick.AddListener(() => SaveModel());
        #endregion

        //Ŀ���� �ε�
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

    // Ŀ���� ����
    void SaveModel()
    {
        CustomInfo info = new CustomInfo();

        info.nickName = PhotonNetwork.NickName;

        // ����
        if(isFemale == true)
        {
            print("�������������Ѵ�");

            info.gender = 1;

            // -------------------------------HAIR-------------------------
            // ���������� hair ���� ����
            info.hairNum = femaleInfo.hair;
            //print($" HAIR (HAT 7~9) = {info.hair} �� ����");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // ���������� hair ���� ����
            info.jacketNum = femaleInfo.jacket;
            //print($" JACKET (0~2) = {info.jacket} �� ����");
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            // ���������� hair ���� ����
            info.chestNum = femaleInfo.chest;
            //print($" CHEST (0~8) = {info.chest} �� ����");
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // ���������� hair ���� ����
            info.tieNum = femaleInfo.tie;
            //print($" TIE (0~3) = {info.tie} �� ����");
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            // ���������� hair ���� ����
            info.legsNum = femaleInfo.legs;
            //print($" LEGS (0~3) = {info.legs} �� ����");
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            // ���������� hair ���� ����
            info.feetNum = femaleInfo.feet;
            //print($" FEET = {info.feet} �� ����");
            // ------------------------------------------------------------

            //print($" SAVE : HAIR(HAT 7~9)= {info.hairNum}, JACKET(0~2)= {info.jacketNum}, CHEST(0~8)= {info.chestNum}, TIE(0~3= {info.tieNum}, LEGS(0~3)= {info.legsNum}, FEET= {info.feetNum}�� ���� ");

            jsonData = JsonUtility.ToJson(info, true);

            print($"������������ + {jsonData}");

            //API Post ��� -> �ٵ� Http������� ������
            OnPost(jsonData);

            //jsonData�� ���Ϸ� ���� //��� , ������
            //File.WriteAllText(Application.dataPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
            //File.WriteAllText(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
        }

        else if(isMale == true)
        {
            print("�������������Ѵ�");

            info.gender = 2;

            // -------------------------------HAIR-------------------------
            // ���������� hair ���� ����
            info.hairNum = maleInfo.hair;
            //print($" HAIR (HAT 7~9) = {info.hair} �� ����");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // ���������� hair ���� ����
            info.jacketNum = maleInfo.jacket;
            //print($" JACKET (0~2) = {info.jacket} �� ����");
            // ------------------------------------------------------------

            // -------------------------------CHEST------------------------
            // ���������� hair ���� ����
            info.chestNum = maleInfo.chest;
            //print($" CHEST (0~8) = {info.chest} �� ����");
            // -----------------------------------------------------------

            // -------------------------------TIE-------------------------
            // ���������� hair ���� ����
            info.tieNum = maleInfo.tie;
            //print($" TIE (0~3) = {info.tie} �� ����");
            // ------------------------------------------------------------

            // -------------------------------LEGS-------------------------
            // ���������� hair ���� ����
            info.legsNum = maleInfo.legs;
            //print($" LEGS (0~3) = {info.legs} �� ����");
            // ------------------------------------------------------------

            // -------------------------------FEET-------------------------
            // ���������� hair ���� ����
            info.feetNum = maleInfo.feet;
            //print($" FEET = {info.feet} �� ����");
            // ------------------------------------------------------------

            //print($" SAVE : HAIR(HAT 7~9)= {info.hairNum}, JACKET(0~2)= {info.jacketNum}, CHEST(0~8)= {info.chestNum}, TIE(0~3= {info.tieNum}, LEGS(0~3)= {info.legsNum}, FEET= {info.feetNum}�� ���� ");

            jsonData = JsonUtility.ToJson(info, true);

            print($"������������ + {jsonData}");

            //API Post ��� -> �ٵ� Http������� ������
            OnPost(jsonData);

            //jsonData�� ���Ϸ� ���� //��� , ������
            //File.WriteAllText(Application.dataPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
            //File.WriteAllText(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + ".txt", jsonData);
        }


    }

    //Ŀ���� ���� 
    public void OnPost(string s)
    {
        string url = "/character/";
        url += HttpManager.instance.username;
        print($"ĳ���� ���� �̸� + {HttpManager.instance.username}");

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester);
    }

    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("ĳ���� ���� ����");
    }

    void OnPostFailed()
    {
        print("ĳ���� ���� ����");

        // ���� ���� �� �������� ��û! PUT
        OnPut(jsonData);
    }

    // Ŀ���� ����
    public void OnPut(string s)
    {
        print(s);
        string url = "/character/";
        url += HttpManager.instance.username;
        print($"ĳ���� ���� �̸� + {HttpManager.instance.username}");

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.PUT, url, true);
        requester.body = s;
        requester.isChat = true;

        requester.onComplete = OnPutComplete;
        requester.onFailed = OnPutFailed;

        //HttpManager���� ��û
        HttpManager.instance.SendRequest(requester);
    }

    void OnPutComplete(DownloadHandler result)
    {
        print(result.text);
        print("ĳ���� ���� ����");
    }

    void OnPutFailed()
    {
        print("ĳ���� ���� ����");
    }

    // Ŀ���� �ε�
    public void OnGetPost()
    {
        string url = "/character/find/";
        url += HttpManager.instance.nickname;
        //url += PhotonNetwork.NickName;
        print($"ĳ���� �ε� �̸� + {HttpManager.instance.username}");

        //���� -> ������ ��ȸ -> ���� �־��� 
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
        print("�ҷ����� ����");

        HttpCharacterData characterData = new HttpCharacterData();
        characterData = JsonUtility.FromJson<HttpCharacterData>(result.text);

        LoadModel(characterData.results.nickName, characterData.results.gender, characterData.results.hairNum, characterData.results.jacketNum, characterData.results.chestNum, characterData.results.tieNum, characterData.results.legsNum, characterData.results.feetNum);
    }

    void OnGetFailed()
    {
        print("�ҷ����� ����!");
    }

    void LoadModel(string nickName, int gender, int hairNum, int jacketNum, int chestNum, int tieNum, int legsNum, int feetNum)
    {
        //���ڶ��
        if(gender == 1)
        {
            male.SetActive(false);
            customMale.SetActive(false);
            isMale = false;

            // -------------------------------HAIR-------------------------
            femaleInfo.GOhair[femaleInfo.hair].SetActive(false);
            femaleInfo.hair = hairNum;
            femaleInfo.GOhair[femaleInfo.hair].SetActive(true);
            //print($" HAIR (���ڹ�ȣ 7~9) = {index.hatNum}�� �ε�");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // ���� ���� Ȱ��ȭ ���¶�� 
            if (jacketNum != 2)
            {
                //���� ���� Ȱ��ȭ
                if (femaleInfo.jacket != 2)
                {
                    // ���ְ� ���ش�
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(false);
                    femaleInfo.jacket = jacketNum;
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(true);
                }

                //���� �� ��Ȱ��ȭ
                else
                {
                    femaleInfo.jacket = jacketNum;
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(true);

                }
            }

            // ���� ���� ��Ȱ��ȭ ���¶�� 
            else
            {
                //���� ���� Ȱ��ȭ
                if (femaleInfo.jacket != 2)
                {
                    // ���ְ� ���ش�
                    femaleInfo.GOjackets[femaleInfo.jacket].SetActive(false);
                }

                //���� �� ��Ȱ��ȭ
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
            // ���� ���� Ȱ��ȭ ���¶�� 
            if (tieNum != 3)
            {
                //���� ���� Ȱ��ȭ
                if (femaleInfo.tie != 3)
                {
                    // ���ְ� ���ش�
                    femaleInfo.GOties[femaleInfo.tie].SetActive(false);
                    femaleInfo.tie = tieNum;
                    femaleInfo.GOties[femaleInfo.tie].SetActive(true);
                }

                //���� �� ��Ȱ��ȭ
                else
                {
                    femaleInfo.tie = tieNum;
                    femaleInfo.GOties[femaleInfo.tie].SetActive(true);

                }
            }

            // ���� ���� ��Ȱ��ȭ ���¶�� 
            else
            {
                //���� ���� Ȱ��ȭ
                if (femaleInfo.tie != 3)
                {
                    // ���ְ� ���ش�
                    femaleInfo.GOties[femaleInfo.tie].SetActive(false);
                }

                //���� �� ��Ȱ��ȭ
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
            //print($" FEET = {info.feet}�� �ε�");
            // ------------------------------------------------------------

            print($" LOAD : HAIR(HAT 7~9)= {femaleInfo.hair}, JACKET(0~2)= {femaleInfo.jacket}, CHEST(0~8)= {femaleInfo.chest}, TIE(0~3= {femaleInfo.tie}, LEGS(0~3)= {femaleInfo.legs}, FEET= {femaleInfo.feet}�� �ε� ");
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
            //print($" HAIR (���ڹ�ȣ 7~9) = {index.hatNum}�� �ε�");
            // ------------------------------------------------------------

            // -------------------------------JACKET-----------------------
            // ���� ���� Ȱ��ȭ ���¶�� 
            if (jacketNum != 2)
            {
                //���� ���� Ȱ��ȭ
                if (maleInfo.jacket != 2)
                {
                    // ���ְ� ���ش�
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(false);
                    maleInfo.jacket = jacketNum;
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(true);
                }

                //���� �� ��Ȱ��ȭ
                else
                {
                    maleInfo.jacket = jacketNum;
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(true);

                }
            }

            // ���� ���� ��Ȱ��ȭ ���¶�� 
            else
            {
                //���� ���� Ȱ��ȭ
                if (maleInfo.jacket != 2)
                {
                    // ���ְ� ���ش�
                    maleInfo.GOjackets[maleInfo.jacket].SetActive(false);
                }

                //���� �� ��Ȱ��ȭ
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
            // ���� ���� Ȱ��ȭ ���¶�� 
            if (tieNum != 3)
            {
                //���� ���� Ȱ��ȭ
                if (maleInfo.tie != 3)
                {
                    // ���ְ� ���ش�
                    maleInfo.GOties[maleInfo.tie].SetActive(false);
                    maleInfo.tie = tieNum;
                    maleInfo.GOties[maleInfo.tie].SetActive(true);
                }

                //���� �� ��Ȱ��ȭ
                else
                {
                    maleInfo.tie = tieNum;
                    maleInfo.GOties[maleInfo.tie].SetActive(true);

                }
            }

            // ���� ���� ��Ȱ��ȭ ���¶�� 
            else
            {
                //���� ���� Ȱ��ȭ
                if (maleInfo.tie != 3)
                {
                    // ���ְ� ���ش�
                    maleInfo.GOties[maleInfo.tie].SetActive(false);
                }

                //���� �� ��Ȱ��ȭ
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
            //print($" FEET = {info.feet}�� �ε�");
            // ------------------------------------------------------------
        }
    }
}
