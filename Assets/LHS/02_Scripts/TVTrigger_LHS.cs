using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVTrigger_LHS : MonoBehaviour
{
    //public SoundManager soundManager;
    //public int idx;
    GameObject interaction;

    public VideoPlayer video;
    public GameObject thumbnail;

    private void Awake()
    {
        interaction = transform.GetChild(0).gameObject;
        interaction.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interaction.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //soundManager.PlayMusic(idx);

                video.Play();
                interaction.SetActive(false);

                thumbnail.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interaction.SetActive(false);
            //soundManager.audio.Stop();

            video.Pause();
        }
    }

}
