using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using UnityEngine.UI;

[System.Serializable]
public struct AiVoiceInfo
{
    public string userId;
    public string weId;
    public byte[] voice;
}

public delegate void SuccessDelegate(DownloadHandler handle);

public class MicTest_LHS : MonoBehaviour
{
	public static MicTest_LHS instance;

	public bool m_IsButtonDowning;

	public GameObject pulsing;

	private string _microphoneID = null;
	private AudioClip _recording = null;
	private int _recordingLengthSec = 15;
	private int _recordingHZ = 22050;

	AudioSource audioSource;

	public SuccessDelegate OnSuccess;

	//public Recorder recorder;

	public bool isRemic = false;

	private void Start()
	{
		instance = this;

		_microphoneID = Microphone.devices[0];
		OnSuccess += OnPostComplete;

		//recorder = GetComponent<Recorder>();
	}


	void Update()
	{
		if(isRemic)
        {
            if (m_IsButtonDowning)
            {

                //recorder.TransmitEnabled = true;

                // ���⿡ �� ���� ������ �˴ϴ�.
                pulsing.SetActive(true);

                //print("�������� ������ ��!");

            }

            //ä�� ĥ ���� �� �� ���� �ؾ���
            if (Input.GetKeyDown(KeyCode.V))
            {
                //print("���� �����̽� ������ ��");
                PointerDown();
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                //print("���� �����̽� ��");
                PointerUp();
            }

            //if (Input.GetButtonDown("Jump"))
            //{
            //    print("�������� �����̽� ������ ��");
            //    PointerDown();
            //}

            //if (Input.GetButtonUp("Jump"))
            //{
            //    print("�������� �����̽� ��");
            //    PointerUp();
            //}
        }
	}

	public void PointerDown()
	{
		ImageChangeON();

		m_IsButtonDowning = true;

		startRecording();

		//print("���� Ȯ��!");

	}

	public void PointerUp()
	{
		ImageChangeOFF();

		m_IsButtonDowning = false;

		pulsing.SetActive(false);

		//print("���� ��!");

		//recorder.TransmitEnabled = false;
		//this.recorder.TransmitEnabled = false;

		stopRecording();
	}
	// ��ư�� OnPointerDown �� �� ȣ��
	public void startRecording()
	{
		Debug.Log("start recording");
		_recording = Microphone.Start(_microphoneID, false, _recordingLengthSec, _recordingHZ);
	}

	// ��ư�� OnPointerUp �� �� ȣ��
	public void stopRecording()
	{

		if (Microphone.IsRecording(_microphoneID))
		{
			
			if (_recording == null)
			{
				Debug.LogError("nothing recorded");
				return;
			}

			// ������ ���� ���� ��������
			int lastTime = Microphone.GetPosition(null);

			Microphone.End(_microphoneID);
			Debug.Log("stop recording");

			// ������ ���ø��� �迭�� ��Ƴ���
			float[] sampleData = new float[_recording.samples];
            _recording.GetData(sampleData, 0);

            float[] cutData = new float[lastTime];

            // sampleData�� ��� ���� lastTime ��ŭ�� cutData�� �����ϱ�
            //Array.Copy(cutData, sampleData, lastTime - 1);
			for(int i = 0; i < cutData.Length; i++)
            {
				cutData[i] = sampleData[i];
            }

            AudioClip newClip = AudioClip.Create("temp", lastTime, _recording.channels, _recording.frequency, false);
            newClip.SetData(cutData, 0);

            //newClip = SavWav_LHS.TrimSilence(_recording, lastTime);

			// ���� �ð��� ���ϱ�
			DateTime dt = DateTime.Now;

			//string aFile = "C:/Users/HP/Desktop/Test/voice/voice";

			//����� ����
			//SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/voice_" + dt.ToString("yyyymmdd_hhmmss"), newClip);
			//SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/voice", newClip);
			SavWav_LHS.Save(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName, newClip);

			//----------------------------------------------------------

			//byte �ٲٱ� 
			//byte[] readFile = File.ReadAllBytes("C:/Users/HP/Desktop/Test/voice/voice.wav");
			byte[] readFile = File.ReadAllBytes(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + ".wav");
			Debug.Log(readFile);

			//UnityWebRequest[] files = new UnityWebRequest[2];
			WWWForm form = new WWWForm();

			//files[0] = UnityWebRequest.Get("file:///C:/Users/HP/Desktop/Test/voice/voice.wav");

			form.AddField("userNickname", PhotonNetwork.NickName);
			form.AddField("opponentNickname", "ȸ��");
			form.AddBinaryData("voice", readFile, "voice.wav");

			string deb = "";
			foreach (var item in form.headers)
			{
				deb += item.Key + " : " + item.Value + "\n";
			}
			Debug.Log(deb);

            HttpManager.instance.SendVoice(form, OnSuccess);

            //----------------------------------
            //AiVoiceInfo aiVoiceInfo = new AiVoiceInfo();
            //aiVoiceInfo.userId = "1";
            //aiVoiceInfo.weId = "2";
            //aiVoiceInfo.voice = readFile;

            //string aiJsonData = JsonUtility.ToJson(aiVoiceInfo, true);
            //print(aiJsonData);

            //OnPost(aiJsonData);
        }
		return;
	}

	public void OnPost(string s)
	{
		//print("�̰� ����Ǹ� �ȵ�");
		string url = "http://192.168.0.213:8001/voice";
		url += HttpManager.instance.username;

		//���� -> ������ ��ȸ -> ���� �־��� 
		HttpRequester requester = new HttpRequester();
        
		requester.SetUrl(RequestType.POST, url, false);
		requester.body = s;
		requester.isJson = true;
		requester.isChat = true;

		requester.onComplete = OnPostComplete;
		requester.onFailed = OnPostFailed;

		HttpManager.instance.SendRequest(requester);
	}

	// �������� ��
	void OnPostComplete(DownloadHandler result)
	{
		print("ai ���̽� ����");
		print(result.text);
		HttpChatVoiceData chatVoiceData = new HttpChatVoiceData();

		chatVoiceData = JsonUtility.FromJson<HttpChatVoiceData>(result.text);

		byte[] data = Convert.FromBase64String(chatVoiceData.results.voice.body);

		//byte[] downData = result.data;

		//SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/return_voice", clipData);
		//File.WriteAllBytes("C:/Users/HP/Desktop/Test/voice/return_voice.wav", result.data);

		File.WriteAllBytes(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav", data);
		//File.WriteAllBytes(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav", chatVoiceData.results.voice.body);

		//AudioClip downClip = AudioClip.Create("result", 0, 1, 22050, false);

		//StartCoroutine(GetWav2AudioClip("C:/Users/HP/Desktop/Test/voice/return_voice.wav"));
		StartCoroutine(GetWav2AudioClip(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName + "_return.wav"));
	}

	//������
	//"httpStatus": 201,
 //   "message": "postVoiceChatBot succeed",
 //   "results": {
 //       "voice": {
 //           "body":

	IEnumerator GetWav2AudioClip(string path)
	{
		Uri voiceURI = new Uri(path);
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(voiceURI, AudioType.WAV);

		yield return www.SendWebRequest();

		AudioClip clipData = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;

		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			audio.clip = clipData;
			audio.Play();
		}
	}


	void OnPostFailed()
	{
		print("ai ���̽� ����");
	}

	Texture2D picture;
	public Image Image;

	void ImageChangeON()
	{
		picture = Resources.Load<Texture2D>("MIC/" + "mic3");

		// ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
		if (picture != null) Image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
	}
	void ImageChangeOFF()
	{
		picture = Resources.Load<Texture2D>("MIC/" + "mic");

		// ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
		if (picture != null) Image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
	}
}
