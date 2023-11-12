using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using ExitGames.Demos.DemoPunVoice;
using UnityEngine.UI;

[System.Serializable]
public struct AiVoiceDBInfo
{
    public string userNickname;
    public string opponentNickname;
    public byte[] voice;
}

//public delegate void SuccessDelegate(DownloadHandler handle);

public class UiController_LHS : MonoBehaviour
{
    public static UiController_LHS instance;

    public bool m_IsButtonDowning;

    //public GameObject pulsing;

    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingLengthSec = 15;
    private int _recordingHZ = 22050;

    AudioSource audioSource;

    public SuccessDelegate OnSuccess;

    Recorder recorder;

    public bool ismic = true;

    // Voice ������Ʈ
    //private PhotonVoiceView photonVoiceView;

    //public Recorder recorder;

    //private void Awake()
    //{
    //    if (this.recorder) // probably using a global recorder
    //    {
    //        return;
    //    }
    //    this.photonVoiceView = this.GetComponentInParent<PhotonVoiceView>();
    //    if (photonVoiceView.IsRecorder)
    //    {
    //        this.recorder = photonVoiceView.RecorderInUse;
    //    }
    //}

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _microphoneID = Microphone.devices[0];
        OnSuccess += OnPostComplete;

        //if (this.recorder) // probably using a global recorder
        //{
        //    return;
        //}
        //this.photonVoiceView = this.GetComponentInParent<PhotonVoiceView>();
        //if (photonVoiceView.IsRecorder)
        //{
        //    this.recorder = photonVoiceView.RecorderInUse;
        //}

        recorder = GetComponent<Recorder>();
        
    }

    void Update()
    {
        if(ismic)
        {
            if (m_IsButtonDowning)
            {

                recorder.TransmitEnabled = true;

                // ���⿡ �� ���� ������ �˴ϴ�.
                //pulsing.SetActive(true);

                //print("���� ������ ��!");

            }

            //ä�� ĥ ���� �� �� ���� �ؾ���
            if (Input.GetKeyDown(KeyCode.V))
            {
                print("���� �����̽� ������ ��");
                PointerDown();
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                print("���� �����̽� ��");
                PointerUp();
            }

            //if (Input.GetButtonDown("Jump"))
            //{
            //    print("���� �����̽� ������ ��");
            //    PointerDown();
            //}

            //if (Input.GetButtonUp("Jump"))
            //{
            //    print("���� �����̽� ������ ��");
            //    PointerUp();
            //}
        }
    }

    public void PointerDown()
    {
        ImageChangeON();

        m_IsButtonDowning = true;

        startRecording();

        print("���� Ȯ��!");

    }

    public void PointerUp()
    {
        ImageChangeOFF();

        m_IsButtonDowning = false;

        //pulsing.SetActive(false);

        print("���� ��!");

        recorder.TransmitEnabled = false;
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
            for (int i = 0; i < cutData.Length; i++)
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
            SavWav_LHS.Save(Application.streamingAssetsPath + "/DB_" + PhotonNetwork.NickName, newClip);
            //SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/voice", newClip);
            //SavWav_LHS.Save(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName, newClip);

            //----------------------------------------------------------

            //byte �ٲٱ� 
            //byte[] readFile = File.ReadAllBytes("C:/Users/HP/Desktop/Test/voice/voice.wav");

            // *****************
            byte[] readFile = File.ReadAllBytes(Application.streamingAssetsPath + "/DB_" + PhotonNetwork.NickName + ".wav");
            Debug.Log(readFile);

            WWWForm form = new WWWForm();

            form.AddField("userNickname", PhotonNetwork.NickName);
            //form.AddField("opponentNickname", 2);
            form.AddBinaryData("voice", readFile, "voice.wav");

            string deb = "";
            foreach (var item in form.headers)
            {
                deb += item.Key + " : " + item.Value + "\n";
            }
            Debug.Log(deb);

            HttpManager.instance.SendVoiceChat(form, OnSuccess);
        }
        return;
    }

    // �������� ��
    void OnPostComplete(DownloadHandler result)
    {
        print("ai ���̽�  ������ ����");
    }

    void OnPostFailed()
    {
        print("ai ���̽� ������ ����");
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
