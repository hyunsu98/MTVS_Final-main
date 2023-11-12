using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (1) 방법 : 버튼을 눌렀을 때 녹음되고 재생할 수 있게
public class Mic_LHS : MonoBehaviour
{
    //(1) 방법
    //AudioSource aud;

    //(2) 방법
    public AudioClip aud;
    int sampleRate = 44100;
    

    //시작 위치
    private float[] samples;
    public float rmsValue;
    public float modulate;
    public int resultValue;
    public int cutValue;

    void Start()
    {
        //(1) 방법
        //aud = GetComponent<AudioSource>();    

        //(2) 방법
        //배열의 크기는 sampleRate로 초기화 시켜주기
        samples = new float[sampleRate];
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
    }

    private void Update()
    {
        // 현재 녹음된 데이터 가져오기
        aud.GetData(samples, 0); //-1f ~ 1f

        //데이터 값의 평균을 구해야함
        //배열의 값을 전부 제곱하여 더해준 뒤 배열의 크기만큼 나눠줍니다.
        //그리고 제곱근을 구해주면 됩니다.
        //양수가 되어 0이 되는 것을 막아줌
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / samples.Length);
        rmsValue = rmsValue * modulate;
        rmsValue = Mathf.Clamp(rmsValue, 0, 100);
        resultValue = Mathf.RoundToInt(rmsValue);

        if(resultValue < cutValue)
        {
            resultValue = 0;
        }

        SavWav_LHS.Save("C:/Users/HP/Desktop/JS", aud);
    }
    #region (1) 방법
    //메소드 2개 생성
    //녹음
    //public void PlaySnd()
    //{
    //    aud.Play();
    //}
    ////재생
    //public void RecSnd()
    //{
    //    //디바이스의 이름, 계속해서 녹음할지의 여부, 녹음의 클립의 길이, 녹음된 클립의 샘플레이트
    //    aud.clip = Microphone.Start(Microphone.devices[0].ToString(), false, 3, 44100);
    //}
    #endregion
}
