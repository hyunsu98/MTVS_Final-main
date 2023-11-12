using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voiceControl_LHS : MonoBehaviour
{
    //Outline outline;

    //Animator aiAnim;

    public GameObject reVoice;

    void Start()
    {
        // outline = transform.GetComponent<Outline>();

        // aiAnim = aiPopup.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //outline.OutlineWidth = 5f;
            reVoice.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //outline.OutlineWidth = 0f;
        reVoice.SetActive(false);
    }
}