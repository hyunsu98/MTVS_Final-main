using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCtrl_LHS : MonoBehaviour
{
    public VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
        // 비디오 재생
        video.Play();
        // 비디오 일시정지
        video.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
