using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public List<GameObject> playSwitch = new List<GameObject>();
    public List<AudioClip> clipList = new List<AudioClip>();
    public AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();

        for(int i = 0; i < playSwitch.Count; i++)
        {
            playSwitch[i].GetComponent<SoundTrigger>().soundManager = this;
            playSwitch[i].GetComponent<SoundTrigger>().idx = i + 1;
        }
    }

    public void PlayMusic(int idx)
    {
        audio.Stop();
        audio.clip = clipList[idx];
        audio.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            audio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseLikeCount();
        }
    }


    public bool isClicked;
    public GameObject normalLike;
    public GameObject fillLike;

    public void ChangeHeart()
    {
        if (!isClicked)
        {
            normalLike.SetActive(false);
            fillLike.SetActive(true);
            isClicked = true;
        }
        else
        {
            normalLike.SetActive(true);
            fillLike.SetActive(false);
            isClicked = false;
        }
    }

    public void IncreaseLikeCount()
    {
        isClicked = false;
        normalLike.transform.GetComponentInChildren<Text>().text = "814";
    }
}
