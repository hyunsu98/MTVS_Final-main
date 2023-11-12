using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// 아이디 입력 시 아이디 중복확인 켜짐
// 이메일 주소 입력 시 이메일 중복확인 켜짐

// 아이디 / 닉네임 / 이메일 주소 / 비밀번호 입력 시 가입완료 켜짐
// 아이디 / 비밀번호는 중복확인 성공이 되어야 함

// 가입 완료 되면 다시 회원가입으로?
// 뒤로가기 버튼을 누르면 로그인 화면으로!

// 회원가입
[System.Serializable]
public class JoinInfo
{
    public string email;
    public string password;
    public string username;
    public string nickname;
}
#region 성공 시
//{
//    "httpStatus": 201,
//    "message": "user created",
//    "results": {
//        "userName": "user2",
//        "email": "qwerty1@google.com"
//    }
//}
#endregion

// 중복확인 : 아이디
[System.Serializable]
public class IDCheckInfo
{
    public string username;
}
#region 아이디 중복체크
// user1은 없다 : 통과
//{
//    "httpStatus": 200,
//    "message": "username is available",
//    "results": {
//        "username": "user1"
//    }
//}
// user1은 있다 : 통과 못함
//{
//    "httpStatus": 400,
//    "message": "username is duplicated",
//    "results": {
//        "username": "user2"
//    }
//}
#endregion 

// 중복확인 : 이메일
[System.Serializable]
public class EmailCheckInfo
{
    public string email;
}
#region 이메일 중복체크
// 중복 체크 통과 시 (qwerty2@google.com은 없다)
//{
//    "httpStatus": 200,
//    "message": "email is available",
//    "results": {
//        "email": "qwerty2@google.com"
//    }
//}
// 중복 체크 실패 시(qwerty1@google.com은 있다)
//{
//    "httpStatus": 400,
//    "message": "email is duplicated",
//    "results": {
//        "email": "qwerty1@google.com"
//    }
//}
#endregion

// 중복확인 : 닉네임
[System.Serializable]
public class NickCheckInfo
{
    public string nickname;
}
#region 닉네임 중복체크
// 중복 체크 통과 시 (회은은 없다)
//{
//    "httpStatus": 200,
//    "message": "nickname is available",
//    "results": {
//        "nickname": "회은"
//    }
//}
// 중복 체크 실패 시(남경은 있다)
//{
//    "httpStatus": 400,
//    "message": "nickname is duplicated",
//    "results": {
//        "nickname": "남경"
//    }
//}
#endregion

public class JoinManager_LHS : MonoBehaviour
{
    [Header("아이디")]
    public InputField inputUserName;
    [Header("닉네임")]
    public InputField inputNickName;
    [Header("이메일")]
    public InputField inputEmail;
    [Header("비번")]
    public InputField inputPassword;

    [Header("가입 버튼")]
    public Button btnConnect;
    [Header("ID중복확인 버튼")]
    public Button btnIDCheck;
    [Header("E-mail중복확인 버튼")]
    public Button btnECheck;
    [Header("NickName중복확인 버튼")]
    public Button btnNCheck;
    [Header("로그인씬으로 가는 버튼")]
    public Button btnBack;

    //중복확인 참!
    bool idCheckOk;
    bool emailCheckOk;
    bool nickCheckOk;
    private object btn;

    // Start is called before the first frame update
    void Start()
    {
        // (InputField)이 변경될때 호출되는 함수 등록
        inputUserName.onValueChanged.AddListener(OnUserNameValueChanged);
        inputEmail.onValueChanged.AddListener(OnEmailValueChanged);
        inputNickName.onValueChanged.AddListener(OnNickNameValueChanged);

        //중복확인
        btnIDCheck.onClick.AddListener(() => OnIDCheck());
        btnECheck.onClick.AddListener(() => OnEmailCheck());
        btnNCheck.onClick.AddListener(() => OnNickCheck());

        //회원가입
        btnConnect.onClick.AddListener(() => OnConnect());

        //로그인 씬
        btnBack.onClick.AddListener(() => OnBack());
    }

    #region 중복확인 버튼을 활성화 하기 위한 함수
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
        // 아이디 / 닉네임 / 이메일 주소 / 비밀번호 입력 시 가입완료 켜짐
        // 아이디 / 비밀번호는 중복확인 성공이 되어야 함
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

    // 회원가입
    void OnConnect()
    {
        // 아이디 / 이메일 주소 중복확인 성공이라면 -> 가입 완료
        // 아닐 시에는 중복확인 하라고 말함?
        JoinInfo joinInfo = new JoinInfo();

        joinInfo.email = inputEmail.text;
        joinInfo.password = inputPassword.text;
        joinInfo.username = inputUserName.text;
        joinInfo.nickname = inputNickName.text;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string jsonData = JsonUtility.ToJson(joinInfo, true);
        //print(jsonData);

        //API Post 방식 -> 바디 Http통신으로 보내기
        OnConnectPost(jsonData);
    }

    // 아이디 중복확인
    void OnIDCheck()
    {
        IDCheckInfo idInfo = new IDCheckInfo();

        idInfo.username = inputUserName.text;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string jsonData = JsonUtility.ToJson(idInfo, true);
        //print(jsonData);

        //API Post 방식 -> 바디 Http통신으로 보내기
        OnIDPost(jsonData);
    }

    // 이메일 중복확인
    void OnEmailCheck()
    {
        EmailCheckInfo emailInfo = new EmailCheckInfo();

        emailInfo.email = inputEmail.text;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string jsonData = JsonUtility.ToJson(emailInfo, true);
        //print(jsonData);

        //API Post 방식 -> 바디 Http통신으로 보내기
        OnEmailPost(jsonData);
    }

    // 닉네임 중복확인
    void OnNickCheck()
    {
        NickCheckInfo nickInfo = new NickCheckInfo();

        nickInfo.nickname = inputNickName.text;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string jsonData = JsonUtility.ToJson(nickInfo, true);
        //print(jsonData);

        //API Post 방식 -> 바디 Http통신으로 보내기
        OnNickPost(jsonData);
    }

    // 로그인씬으로 이동
    void OnBack()
    {
        PhotonNetwork.LoadLevel("ConnectionScene_LHS");
    }

    //--------------------------------- 네트워크 연결 -----------------------------------//

    #region 아이디 중복확인
    public void OnIDPost(string s)
    {
        string url = "/duplication/username";
        //url += PhotonNetwork.NickName;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("사용할 수 있는 아이디입니다!");
        idCheckOk = true;
        btnIDCheck.interactable = false;
    }

    void OnIDPostFailed()
    {
        //print("OnPostFailed , 통신 실패");
        print("중복된 아이디입니다");
        idCheckOk = false;
    }
    #endregion

    #region 이메일 중복확인
    public void OnEmailPost(string s)
    {
        string url = "/duplication/email";
        //url += PhotonNetwork.NickName;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("사용할 수 있는 이메일입니다!");
        emailCheckOk = true;
        btnECheck.interactable = false;
    }

    void OnEmailPostFailed()
    {
        print("중복된 이메일입니다");
        emailCheckOk = false;
    }
    #endregion

    #region 닉네임 중복확인
    public void OnNickPost(string s)
    {
        string url = "/duplication/nickname";
        //url += PhotonNetwork.NickName;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("사용할 수 있는 닉네임입니다!");
        nickCheckOk = true;
        btnNCheck.interactable = false;
    }

    void OnNickPostFailed()
    {
        print("중복된 닉네임입니다");
        nickCheckOk = false;
    }
    #endregion

    #region 회원가입
    public void OnConnectPost(string s)
    {
        string url = "/signup";
        //url += PhotonNetwork.NickName;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("회원가입 성공!");
    }

    void OnConnectPostFailed()
    {
        print("회원가입 실패!");
    }
    #endregion
}

