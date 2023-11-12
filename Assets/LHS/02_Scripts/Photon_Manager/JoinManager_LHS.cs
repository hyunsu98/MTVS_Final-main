using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// ���̵� �Է� �� ���̵� �ߺ�Ȯ�� ����
// �̸��� �ּ� �Է� �� �̸��� �ߺ�Ȯ�� ����

// ���̵� / �г��� / �̸��� �ּ� / ��й�ȣ �Է� �� ���ԿϷ� ����
// ���̵� / ��й�ȣ�� �ߺ�Ȯ�� ������ �Ǿ�� ��

// ���� �Ϸ� �Ǹ� �ٽ� ȸ����������?
// �ڷΰ��� ��ư�� ������ �α��� ȭ������!

// ȸ������
[System.Serializable]
public class JoinInfo
{
    public string email;
    public string password;
    public string username;
    public string nickname;
}
#region ���� ��
//{
//    "httpStatus": 201,
//    "message": "user created",
//    "results": {
//        "userName": "user2",
//        "email": "qwerty1@google.com"
//    }
//}
#endregion

// �ߺ�Ȯ�� : ���̵�
[System.Serializable]
public class IDCheckInfo
{
    public string username;
}
#region ���̵� �ߺ�üũ
// user1�� ���� : ���
//{
//    "httpStatus": 200,
//    "message": "username is available",
//    "results": {
//        "username": "user1"
//    }
//}
// user1�� �ִ� : ��� ����
//{
//    "httpStatus": 400,
//    "message": "username is duplicated",
//    "results": {
//        "username": "user2"
//    }
//}
#endregion 

// �ߺ�Ȯ�� : �̸���
[System.Serializable]
public class EmailCheckInfo
{
    public string email;
}
#region �̸��� �ߺ�üũ
// �ߺ� üũ ��� �� (qwerty2@google.com�� ����)
//{
//    "httpStatus": 200,
//    "message": "email is available",
//    "results": {
//        "email": "qwerty2@google.com"
//    }
//}
// �ߺ� üũ ���� ��(qwerty1@google.com�� �ִ�)
//{
//    "httpStatus": 400,
//    "message": "email is duplicated",
//    "results": {
//        "email": "qwerty1@google.com"
//    }
//}
#endregion

// �ߺ�Ȯ�� : �г���
[System.Serializable]
public class NickCheckInfo
{
    public string nickname;
}
#region �г��� �ߺ�üũ
// �ߺ� üũ ��� �� (ȸ���� ����)
//{
//    "httpStatus": 200,
//    "message": "nickname is available",
//    "results": {
//        "nickname": "ȸ��"
//    }
//}
// �ߺ� üũ ���� ��(������ �ִ�)
//{
//    "httpStatus": 400,
//    "message": "nickname is duplicated",
//    "results": {
//        "nickname": "����"
//    }
//}
#endregion

public class JoinManager_LHS : MonoBehaviour
{
    [Header("���̵�")]
    public InputField inputUserName;
    [Header("�г���")]
    public InputField inputNickName;
    [Header("�̸���")]
    public InputField inputEmail;
    [Header("���")]
    public InputField inputPassword;

    [Header("���� ��ư")]
    public Button btnConnect;
    [Header("ID�ߺ�Ȯ�� ��ư")]
    public Button btnIDCheck;
    [Header("E-mail�ߺ�Ȯ�� ��ư")]
    public Button btnECheck;
    [Header("NickName�ߺ�Ȯ�� ��ư")]
    public Button btnNCheck;
    [Header("�α��ξ����� ���� ��ư")]
    public Button btnBack;

    //�ߺ�Ȯ�� ��!
    bool idCheckOk;
    bool emailCheckOk;
    bool nickCheckOk;
    private object btn;

    // Start is called before the first frame update
    void Start()
    {
        // (InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputUserName.onValueChanged.AddListener(OnUserNameValueChanged);
        inputEmail.onValueChanged.AddListener(OnEmailValueChanged);
        inputNickName.onValueChanged.AddListener(OnNickNameValueChanged);

        //�ߺ�Ȯ��
        btnIDCheck.onClick.AddListener(() => OnIDCheck());
        btnECheck.onClick.AddListener(() => OnEmailCheck());
        btnNCheck.onClick.AddListener(() => OnNickCheck());

        //ȸ������
        btnConnect.onClick.AddListener(() => OnConnect());

        //�α��� ��
        btnBack.onClick.AddListener(() => OnBack());
    }

    #region �ߺ�Ȯ�� ��ư�� Ȱ��ȭ �ϱ� ���� �Լ�
    public void OnUserNameValueChanged(string s)
    {
        btnIDCheck.interactable = s.Length > 0;
    }

    public void OnEmailValueChanged(string s)
    {
        btnECheck.interactable = s.Length > 0;
    }

    public void OnNickNameValueChanged(string s)
    {
        btnNCheck.interactable = s.Length > 0;
    }
    #endregion


    void Update()
    {
        // ���̵� / �г��� / �̸��� �ּ� / ��й�ȣ �Է� �� ���ԿϷ� ����
        // ���̵� / ��й�ȣ�� �ߺ�Ȯ�� ������ �Ǿ�� ��
        if(idCheckOk == true && emailCheckOk == true && nickCheckOk == true && inputPassword.text.Length > 0)
        {
            //btnConnect.interactable = s.Length > 0 && inputUserName.text.Length > 0 && inputEmail.text.Length > 0 && inputPassword.text.Length > 0;
            btnConnect.interactable = true;
        }
        else
        {
            btnConnect.interactable = false;
        }
    }

    // ȸ������
    void OnConnect()
    {
        // ���̵� / �̸��� �ּ� �ߺ�Ȯ�� �����̶�� -> ���� �Ϸ�
        // �ƴ� �ÿ��� �ߺ�Ȯ�� �϶�� ����?
        JoinInfo joinInfo = new JoinInfo();

        joinInfo.email = inputEmail.text;
        joinInfo.password = inputPassword.text;
        joinInfo.username = inputUserName.text;
        joinInfo.nickname = inputNickName.text;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string jsonData = JsonUtility.ToJson(joinInfo, true);
        //print(jsonData);

        //API Post ��� -> �ٵ� Http������� ������
        OnConnectPost(jsonData);
    }

    // ���̵� �ߺ�Ȯ��
    void OnIDCheck()
    {
        IDCheckInfo idInfo = new IDCheckInfo();

        idInfo.username = inputUserName.text;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string jsonData = JsonUtility.ToJson(idInfo, true);
        //print(jsonData);

        //API Post ��� -> �ٵ� Http������� ������
        OnIDPost(jsonData);
    }

    // �̸��� �ߺ�Ȯ��
    void OnEmailCheck()
    {
        EmailCheckInfo emailInfo = new EmailCheckInfo();

        emailInfo.email = inputEmail.text;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string jsonData = JsonUtility.ToJson(emailInfo, true);
        //print(jsonData);

        //API Post ��� -> �ٵ� Http������� ������
        OnEmailPost(jsonData);
    }

    // �г��� �ߺ�Ȯ��
    void OnNickCheck()
    {
        NickCheckInfo nickInfo = new NickCheckInfo();

        nickInfo.nickname = inputNickName.text;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string jsonData = JsonUtility.ToJson(nickInfo, true);
        //print(jsonData);

        //API Post ��� -> �ٵ� Http������� ������
        OnNickPost(jsonData);
    }

    // �α��ξ����� �̵�
    void OnBack()
    {
        PhotonNetwork.LoadLevel("ConnectionScene_LHS");
    }

    //--------------------------------- ��Ʈ��ũ ���� -----------------------------------//

    #region ���̵� �ߺ�Ȯ��
    public void OnIDPost(string s)
    {
        string url = "/duplication/username";
        //url += PhotonNetwork.NickName;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnIDPostComplete;
        requester.onFailed = OnIDPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnIDPostComplete(DownloadHandler result)
    {
        print("����� �� �ִ� ���̵��Դϴ�!");
        idCheckOk = true;
        btnIDCheck.interactable = false;
    }

    void OnIDPostFailed()
    {
        //print("OnPostFailed , ��� ����");
        print("�ߺ��� ���̵��Դϴ�");
        idCheckOk = false;
    }
    #endregion

    #region �̸��� �ߺ�Ȯ��
    public void OnEmailPost(string s)
    {
        string url = "/duplication/email";
        //url += PhotonNetwork.NickName;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnEmailPostComplete;
        requester.onFailed = OnEmailPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnEmailPostComplete(DownloadHandler result)
    {
        print("����� �� �ִ� �̸����Դϴ�!");
        emailCheckOk = true;
        btnECheck.interactable = false;
    }

    void OnEmailPostFailed()
    {
        print("�ߺ��� �̸����Դϴ�");
        emailCheckOk = false;
    }
    #endregion

    #region �г��� �ߺ�Ȯ��
    public void OnNickPost(string s)
    {
        string url = "/duplication/nickname";
        //url += PhotonNetwork.NickName;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnNickPostComplete;
        requester.onFailed = OnNickPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnNickPostComplete(DownloadHandler result)
    {
        print("����� �� �ִ� �г����Դϴ�!");
        nickCheckOk = true;
        btnNCheck.interactable = false;
    }

    void OnNickPostFailed()
    {
        print("�ߺ��� �г����Դϴ�");
        nickCheckOk = false;
    }
    #endregion

    #region ȸ������
    public void OnConnectPost(string s)
    {
        string url = "/signup";
        //url += PhotonNetwork.NickName;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnConnectPostComplete;
        requester.onFailed = OnConnectPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnConnectPostComplete(DownloadHandler result)
    {
        print("ȸ������ ����!");
    }

    void OnConnectPostFailed()
    {
        print("ȸ������ ����!");
    }
    #endregion
}

