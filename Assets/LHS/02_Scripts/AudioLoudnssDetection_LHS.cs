using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnssDetection_LHS : MonoBehaviour
{
    public int sampleWindow = 64;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //오디오 클립에서 음량 가져오기
    //오디오의 위치를 나타내는 클립 위치 , 오디오 클립 자체
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;
        
        for (int i =0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
