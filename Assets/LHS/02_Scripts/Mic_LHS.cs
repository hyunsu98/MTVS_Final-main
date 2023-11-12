using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (1) ��� : ��ư�� ������ �� �����ǰ� ����� �� �ְ�
public class Mic_LHS : MonoBehaviour
{
    //(1) ���
    //AudioSource aud;

    //(2) ���
    public AudioClip aud;
    int sampleRate = 44100;
    

    //���� ��ġ
    private float[] samples;
    public float rmsValue;
    public float modulate;
    public int resultValue;
    public int cutValue;

    void Start()
    {
        //(1) ���
        //aud = GetComponent<AudioSource>();    

        //(2) ���
        //�迭�� ũ��� sampleRate�� �ʱ�ȭ �����ֱ�
        samples = new float[sampleRate];
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
    }

    private void Update()
    {
        // ���� ������ ������ ��������
        aud.GetData(samples, 0); //-1f ~ 1f

        //������ ���� ����� ���ؾ���
        //�迭�� ���� ���� �����Ͽ� ������ �� �迭�� ũ�⸸ŭ �����ݴϴ�.
        //�׸��� �������� �����ָ� �˴ϴ�.
        //����� �Ǿ� 0�� �Ǵ� ���� ������
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
    #region (1) ���
    //�޼ҵ� 2�� ����
    //����
    //public void PlaySnd()
    //{
    //    aud.Play();
    //}
    ////���
    //public void RecSnd()
    //{
    //    //����̽��� �̸�, ����ؼ� ���������� ����, ������ Ŭ���� ����, ������ Ŭ���� ���÷���Ʈ
    //    aud.clip = Microphone.Start(Microphone.devices[0].ToString(), false, 3, 44100);
    //}
    #endregion
}
