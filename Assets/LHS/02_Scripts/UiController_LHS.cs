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

    // Voice 오브젝트
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

                // 여기에 할 일을 넣으면 됩니다.
                //pulsing.SetActive(true);

                //print("음성 누르는 중!");

            }

            //채팅 칠 때는 할 수 없게 해야함
            if (Input.GetKeyDown(KeyCode.V))
            {
                print("음성 스페이스 누르는 중");
                PointerDown();
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                print("음성 스페이스 뗌");
                PointerUp();
            }

            //if (Input.GetButtonDown("Jump"))
            //{
            //    print("음성 스페이스 누르는 중");
            //    PointerDown();
            //}

            //if (Input.GetButtonUp("Jump"))
            //{
            //    print("음성 스페이스 누르는 중");
            //    PointerUp();
            //}
        }
    }

    public void PointerDown()
    {
        ImageChangeON();

        m_IsButtonDowning = true;

        startRecording();

        print("음성 확인!");

    }

    public void PointerUp()
    {
        ImageChangeOFF();

        m_IsButtonDowning = false;

        //pulsing.SetActive(false);

        print("음성 뗌!");

        recorder.TransmitEnabled = false;
        //this.recorder.TransmitEnabled = false;

        stopRecording();
    }

    // 버튼을 OnPointerDown 할 때 호출
    public void startRecording()
    {
        Debug.Log("start recording");
        _recording = Microphone.Start(_microphoneID, false, _recordingLengthSec, _recordingHZ);
    }

    // 버튼을 OnPointerUp 할 때 호출
    public void stopRecording()
    {
        if (Microphone.IsRecording(_microphoneID))
        {

            if (_recording == null)
            {
                Debug.LogError("nothing recorded");
                return;
            }

            // 마지막 녹음 시점 가져오기
            int lastTime = Microphone.GetPosition(null);

            Microphone.End(_microphoneID);
            Debug.Log("stop recording");

            // 녹음한 샘플링을 배열에 담아놓기
            float[] sampleData = new float[_recording.samples];
            _recording.GetData(sampleData, 0);

            float[] cutData = new float[lastTime];

            // sampleData에 담긴 값을 lastTime 만큼만 cutData에 복사하기
            //Array.Copy(cutData, sampleData, lastTime - 1);
            for (int i = 0; i < cutData.Length; i++)
            {
                cutData[i] = sampleData[i];
            }

            AudioClip newClip = AudioClip.Create("temp", lastTime, _recording.channels, _recording.frequency, false);
            newClip.SetData(cutData, 0);

            //newClip = SavWav_LHS.TrimSilence(_recording, lastTime);

            // 현재 시간을 구하기
            DateTime dt = DateTime.Now;

            //string aFile = "C:/Users/HP/Desktop/Test/voice/voice";

            //저장된 파일
            //SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/voice_" + dt.ToString("yyyymmdd_hhmmss"), newClip);
            SavWav_LHS.Save(Application.streamingAssetsPath + "/DB_" + PhotonNetwork.NickName, newClip);
            //SavWav_LHS.Save("C:/Users/HP/Desktop/Test/voice/voice", newClip);
            //SavWav_LHS.Save(Application.streamingAssetsPath + "/" + PhotonNetwork.NickName, newClip);

            //----------------------------------------------------------

            //byte 바꾸기 
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

    // 성공했을 때
    void OnPostComplete(DownloadHandler result)
    {
        print("ai 보이스  보내기 성공");
    }

    void OnPostFailed()
    {
        print("ai 보이스 보내기 실패");
    }
    Texture2D picture;
    public Image Image;

    void ImageChangeON()
    {
        picture = Resources.Load<Texture2D>("MIC/" + "mic3");

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
    void ImageChangeOFF()
    {
        picture = Resources.Load<Texture2D>("MIC/" + "mic");

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
