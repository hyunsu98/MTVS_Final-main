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
        // ���� ���
        video.Play();
        // ���� �Ͻ�����
        video.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
