using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Networking;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


//���̵� / ��й�ȣ �� �� �Է� �� �α��� ��ư Ȱ��ȭ��Ű��
//<�α��� Ŭ��> Post ������
//<�α��� ����> (1)�α��� ���� (2)��ū�� �ޱ� -> ��ȿ�Ⱓ�� �ֱ⶧���� ���������� �ΰ� �ٲ������
//<�α��� ����> ���� UI �߱�

// �α���
[System.Serializable]
public class LoginInfo
{
    public string username;
    public string password;
}

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [Header("���̵�")]
    public InputField inputUserName;
    [Header("���")]
    public InputField inputPassword;
    [Header("�α��� ��ư")]
    public Button btnConnect;
    [Header("ȸ������ ��ư")]
    public Button btnMembership;

    //[Header("Json ������")]
    // �� Ʋ�� �����غ���
    HttpGetData getData = new HttpGetData();

    void Start()
    {
        // �г���(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputUserName.onValueChanged.AddListener(OnUserNameValueChanged);
        inputPassword.onValueChanged.AddListener(OnPasswordValueChanged);

        // �г���(InputField)���� Enter�� ������ ȣ��Ǵ� �Լ� ���
        inputUserName.onSubmit.AddListener(OnSubmit);
        // �г���(InputField)���� Focusing�� �Ҿ����� ȣ��Ǵ� �Լ� ���
        inputUserName.onEndEdit.AddListener(OnEndEdit);

        //btnBack.onClick.AddListener(() => 
        btnMembership.onClick.AddListener(() => OnJoinConnect());
    }

    //���̵� / ��й�ȣ �� �� �Է� �� �α��� ��ư Ȱ��ȭ��Ű��
    public void OnUserNameValueChanged(string s)
    {
        btnConnect.interactable = s.Length > 0 && inputPassword.text.Length > 0;
    }

    public void OnPasswordValueChanged(string s)
    {
        btnConnect.interactable = s.Length > 0 && inputUserName.text.Length > 0;
    }

    #region ���� ��� ����
    public void OnSubmit(string s)
    {
        ////���࿡ s�� ���̰� 0���� ũ�ٸ�
        //if (s.Length > 0)
        //{
        //    //���� ����! 
        //    OnClickConnect();
        //}
    }

    public void OnEndEdit(string s)
    {

    }
    #endregion


    public void OnClickConnect()
    {
        //<�α��� Ŭ��> Post ������
        //<�α��� ����> (1)�α��� ���� (2)��ū�� �ޱ� -> ��ȿ�Ⱓ�� �ֱ⶧���� ���������� �ΰ� �ٲ������
        //<�α��� ����> ���� UI �߱�

        LoginInfo info = new LoginInfo();

        info.username = inputUserName.text;
        info.password = inputPassword.text;

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string jsonData = JsonUtility.ToJson(info, true);

        // ������ Test�� ����!
        //API Post ��� -> �ٵ� Http������� ������
        OnPost(jsonData);

        // �ֶȶ� Test�� ����!
        //PhotonNetwork.ConnectUsingSettings();
    }

    //--------------------------------- ��Ʈ��ũ ���� -----------------------------------//

    #region �α���
    public void OnPost(string s)
    {
        string url = "/login";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();
        
        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    // �������� ��
    void OnPostComplete(DownloadHandler result)
    {
        //JObject json = JObject.Parse(result.text);
        //var s = json.Properties["result"];

        #region ���� �� ������ ���� �޾ƾ� ��
        // result �� ������ֳ� �����.

        // ������ Ʋ�� Json �������� �ٲ� result ���� ����
        getData = JsonUtility.FromJson<HttpGetData>(result.text);
        #endregion

        //��ū����
        HttpManager.instance.token = getData.results.token;
        HttpManager.instance.username = getData.results.username;
        HttpManager.instance.nickname = getData.results.nickname;

        print(HttpManager.instance.token);

        //���� ���� ��û
        PhotonNetwork.ConnectUsingSettings();
    }

    #region ���� ��
    //{
    //    "httpStatus": 200,
    //    "message": "token generated",
    //    "results": {
    //        "nickname": "����",
    //        "token": "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c2VyMiIsImV4cCI6MTY2NzQ5NzY1OCwiaWF0IjoxNjY3NDU0NDU4fQ.mMcaPseXyYnELoH47fTbydiLhtoeQ89Mn9ObdNbwJbc"
    //    }
    //}
    #endregion

    void OnPostFailed()
    {
        print("�α��ο� �����ϼ̽��ϴ�");
    }
    #endregion

    //������ ���� ���Ӽ����� ȣ��(Lobby�� ������ �� ���� ����)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //������ ���� ���Ӽ����� ȣ��(Lobby�� ������ �� �ִ� ����)
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //�� �г��� ����
        //������ ���� �г������� �����Ѱ�..? => ȸ�������Ҷ�
        PhotonNetwork.NickName = getData.results.nickname;
        //PhotonNetwork.NickName = getData.results.username;

        //�κ� ���� ��û
        PhotonNetwork.JoinLobby();
    }

    //�κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //LobbyScene���� �̵�
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    void OnJoinConnect()
    {
        PhotonNetwork.LoadLevel("JoinScene_LHS");
    }


}
