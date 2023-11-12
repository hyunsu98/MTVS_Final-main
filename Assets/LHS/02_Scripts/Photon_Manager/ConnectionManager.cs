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


//아이디 / 비밀번호 둘 다 입력 시 로그인 버튼 활성화시키기
//<로그인 클릭> Post 보내기
//<로그인 성공> (1)로그인 성공 (2)토큰값 받기 -> 유효기간이 있기때문에 변수명으로 두고 바꿔줘야함
//<로그인 실패> 실패 UI 뜨기

// 로그인
[System.Serializable]
public class LoginInfo
{
    public string username;
    public string password;
}

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [Header("아이디")]
    public InputField inputUserName;
    [Header("비번")]
    public InputField inputPassword;
    [Header("로그인 버튼")]
    public Button btnConnect;
    [Header("회원가입 버튼")]
    public Button btnMembership;

    //[Header("Json 데이터")]
    // 그 틀을 생성해보자
    HttpGetData getData = new HttpGetData();

    void Start()
    {
        // 닉네임(InputField)이 변경될때 호출되는 함수 등록
        inputUserName.onValueChanged.AddListener(OnUserNameValueChanged);
        inputPassword.onValueChanged.AddListener(OnPasswordValueChanged);

        // 닉네임(InputField)에서 Enter를 쳤을때 호출되는 함수 등록
        inputUserName.onSubmit.AddListener(OnSubmit);
        // 닉네임(InputField)에서 Focusing을 잃었을때 호출되는 함수 등록
        inputUserName.onEndEdit.AddListener(OnEndEdit);

        //btnBack.onClick.AddListener(() => 
        btnMembership.onClick.AddListener(() => OnJoinConnect());
    }

    //아이디 / 비밀번호 둘 다 입력 시 로그인 버튼 활성화시키기
    public void OnUserNameValueChanged(string s)
    {
        btnConnect.interactable = s.Length > 0 && inputPassword.text.Length > 0;
    }

    public void OnPasswordValueChanged(string s)
    {
        btnConnect.interactable = s.Length > 0 && inputUserName.text.Length > 0;
    }

    #region 아직 사용 안함
    public void OnSubmit(string s)
    {
        ////만약에 s의 길이가 0보다 크다면
        //if (s.Length > 0)
        //{
        //    //접속 하자! 
        //    OnClickConnect();
        //}
    }

    public void OnEndEdit(string s)
    {

    }
    #endregion


    public void OnClickConnect()
    {
        //<로그인 클릭> Post 보내기
        //<로그인 성공> (1)로그인 성공 (2)토큰값 받기 -> 유효기간이 있기때문에 변수명으로 두고 바꿔줘야함
        //<로그인 실패> 실패 UI 뜨기

        LoginInfo info = new LoginInfo();

        info.username = inputUserName.text;
        info.password = inputPassword.text;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string jsonData = JsonUtility.ToJson(info, true);

        // 현수기 Test를 위한!
        //API Post 방식 -> 바디 Http통신으로 보내기
        OnPost(jsonData);

        // 솔똑똑 Test를 위한!
        //PhotonNetwork.ConnectUsingSettings();
    }

    //--------------------------------- 네트워크 연결 -----------------------------------//

    #region 로그인
    public void OnPost(string s)
    {
        string url = "/login";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        
        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    // 성공했을 때
    void OnPostComplete(DownloadHandler result)
    {
        //JObject json = JObject.Parse(result.text);
        //var s = json.Properties["result"];

        #region 성공 시 나오는 값을 받아야 함
        // result 에 담겨져있네 결과가.

        // 생성한 틀에 Json 형식으로 바꾼 result 값을 넣자
        getData = JsonUtility.FromJson<HttpGetData>(result.text);
        #endregion

        //토큰셋팅
        HttpManager.instance.token = getData.results.token;
        HttpManager.instance.username = getData.results.username;
        HttpManager.instance.nickname = getData.results.nickname;

        print(HttpManager.instance.token);

        //서버 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    #region 성공 시
    //{
    //    "httpStatus": 200,
    //    "message": "token generated",
    //    "results": {
    //        "nickname": "남경",
    //        "token": "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c2VyMiIsImV4cCI6MTY2NzQ5NzY1OCwiaWF0IjoxNjY3NDU0NDU4fQ.mMcaPseXyYnELoH47fTbydiLhtoeQ89Mn9ObdNbwJbc"
    //    }
    //}
    #endregion

    void OnPostFailed()
    {
        print("로그인에 실패하셨습니다");
    }
    #endregion

    //마스터 서버 접속성공시 호출(Lobby에 진입할 수 없는 상태)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //마스터 서버 접속성공시 호출(Lobby에 진입할 수 있는 상태)
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //내 닉네임 설정
        //설정해 놓은 닉네임으로 가능한가..? => 회원가입할때
        PhotonNetwork.NickName = getData.results.nickname;
        //PhotonNetwork.NickName = getData.results.username;

        //로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    //로비 진입 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //LobbyScene으로 이동
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    void OnJoinConnect()
    {
        PhotonNetwork.LoadLevel("JoinScene_LHS");
    }


}
