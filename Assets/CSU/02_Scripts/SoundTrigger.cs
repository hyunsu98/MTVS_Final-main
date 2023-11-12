using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public SoundManager soundManager;
    public int idx;
    GameObject interaction;

    private void Awake()
    {
        interaction = transform.GetChild(0).gameObject;
        interaction.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interaction.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                soundManager.PlayMusic(idx);
                interaction.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interaction.SetActive(false);
            soundManager.audio.Stop();
        }
    }
}
