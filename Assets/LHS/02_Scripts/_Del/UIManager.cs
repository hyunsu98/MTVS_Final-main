using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickGetPost()
    {
        ////������ �Խù� ��ȸ ��û(/posts/1 , GET)
        ////HttRequester�� ����
        //HttpRequester requester = new HttpRequester();

        /////posts/1 , GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
        //requester.url = "https://jsonplaceholder.typicode.com/posts/1";
        //requester.requestType = RequestType.GET;
        //requester.onComplete = OnCompleteGetPost;

        ////HttpManager���� ��û
        //HttpManager.instance.SendRequest(requester);

        string url = "https://8c49-119-194-163-123.jp.ngrok.io/chat_bot?chat_request=";
        url += "chatText";
        url += "&user_id=" + 1;
        url += "&we_id=" + 1;

        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, false);

        HttpManager.instance.SendRequest(requester);
    }

    //public void OnCompleteGetPost(DownloadHandler handler)
    //{
    //    PostData postData = JsonUtility.FromJson<PostData>(handler.text);
    //    //Ÿ��Ʋ  UI�� ���
    //    //���� UI�� ���
    //    print("��ȸ �Ϸ�");
    //}

    //public void OnClickGetPostAll()
    //{
    //    //������ �Խù� ��ȸ ��û(/posts/1 , GET)
    //    //HttRequester�� ����
    //    HttpRequester requester = new HttpRequester();

    //    ///posts/1 , GET, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
    //    requester.url = "https://jsonplaceholder.typicode.com/posts";
    //    requester.requestType = RequestType.GET;
    //    requester.onComplete = OnCompleteGetPostAll;

    //    //HttpManager���� ��û
    //    HttpManager.instance.SendRequest(requester);
    //}

    //public void OnCompleteGetPostAll(DownloadHandler handler)
    //{
    //    //�迭 �����͸� Ű���� �ִ´�.
    //    string s = "{\"data\":" + handler.text + "}";

    //    //List<PostData>
    //    PostDataArray array = JsonUtility.FromJson<PostDataArray>(s);
    //    for (int i = 0; i < array.data.Count; i++)
    //    {
    //        print(array.data[i].id + "\n" + array.data[i].title + "\n" + array.data[i].body);
    //    }

    //    print("��ȸ �Ϸ�");
    //}

    //public void OnClickSignIn()
    //{
    //    //������ �Խù� ��ȸ ��û(/posts/1 , GET)
    //    //HttRequester�� ����
    //    HttpRequester requester = new HttpRequester();

    //    ///user , POST, �Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�
    //    requester.url = "https://jsonplaceholder.typicode.com/user";
    //    requester.requestType = RequestType.POST;

    //    //post data ����
    //    UserData data = new UserData();
    //    data.name = "������";
    //    data.email = "lokimve7@naver.com";
    //    data.id = "rapa_xr";
    //    data.age = 20;

    //    requester.postData = JsonUtility.ToJson(data, true);
    //    print(requester.postData);

    //    requester.onComplete = OnCompleteSignIn;

    //    //HttpManager���� ��û
    //    HttpManager.instance.SendRequest(requester);
    //}

    //public void OnCompleteSignIn(DownloadHandler handler)
    //{
    //    //�迭 �����͸� Ű���� �ִ´�.
    //    string s = "{\"data\":" + handler.text + "}";

    //    //List<PostData>
    //    PostDataArray array = JsonUtility.FromJson<PostDataArray>(s);
    //    for (int i = 0; i < array.data.Count; i++)
    //    {
    //        print(array.data[i].id + "\n" + array.data[i].title + "\n" + array.data[i].body);
    //    }

    //    print("��ȸ �Ϸ�");
    //}
}
