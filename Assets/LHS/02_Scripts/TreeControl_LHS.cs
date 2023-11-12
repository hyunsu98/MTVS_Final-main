using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControl_LHS : MonoBehaviour
{
    Outline outline;

    Animator aiAnim;
    public GameObject aiPopup;

    void Start()
    {
        outline = transform.GetComponent<Outline>();

        aiAnim = aiPopup.GetComponent<Animator>();
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
            aiAnim.SetTrigger("open");

        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //outline.OutlineWidth = 5f;
            //MouseUI_LHS.instance.isChat = true;
        }

    }
        

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            aiAnim.SetTrigger("close");

            //MouseUI_LHS.instance.isChat = false;
        }
        //outline.OutlineWidth = 0f;
        //aiAnim.SetTrigger("close");

        //MouseUI_LHS.instance.isChat = false;
    }
}
