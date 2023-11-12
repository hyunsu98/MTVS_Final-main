using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ��������� ������ ��������
// ���� ���� �� ����AI�� ä�ð���

// ����

// ��������� ������
// ���� ���� / ������ ���� ��� ����
// �ٽ� ������ ���� �� �ְ�
// ���� ���� -> ���� ���ε� �� �ﰢ���� // ȥ�ڸ� ���� �� Ȱ��ȭ �Ǿ�� ��
// ������ ���� -> �ε� �� �ﰢ ���� // ���� �ִٰ� ȥ�ڰ� �Ǿ��� �� Ȱ��ȭ �Ǿ�� ��
// ���� ���� , ��� �ٲ�� 
// ä�� AI ���� / ���� AI �������� �������� ��

public class ReManager : MonoBehaviour
{
    // ���� ��ư
    public Button btn_Restore;

    // ����, ������ ���� UI // Ȯ�� �ʿ� ���� public ����
    GameObject panelBtn;
    GameObject tail;

    // ����
    public GameObject tree;

    [Header("���� Ȱ�� ��")]
    //���� ��ư
    public GameObject chatIcon;
    public GameObject soundIcon;
    public GameObject micIcon;

    public GameObject reIcon;
    public GameObject reOnIcon;
    public GameObject reNameIcon;

    public GameObject restoreTwo;
    public GameObject restoreThree;

    public GameObject reIconTwo;

    public static ReManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //panelBtn.SetActive(false);
        //tail.SetActive(false);

        //btn_Restore.onClick.AddListener(() => OnReChat());
    }

    // Update is called once per frame
    void Update()
    {
        //���� 0���� �����ٸ� ������� Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UiController_LHS.instance.ismic = false;
            MicTest_LHS.instance.isRemic = true;

            reIcon.SetActive(true);

            chatIcon.SetActive(false);
            soundIcon.SetActive(false);
            micIcon.SetActive(false);
            reIcon.SetActive(false);
            reIconTwo.SetActive(true);
        }
    }

    void createTree()
    {
        //tree.SetActive(true);

        restoreTwo.SetActive(false);
        restoreThree.SetActive(false);

        chatIcon.SetActive(false);
        micIcon.SetActive(false);

        reIcon.SetActive(false);
        reIconTwo.SetActive(true);

        UiController_LHS.instance.ismic = false;
        MicTest_LHS.instance.isRemic = true;

    }

    public void RestoreOn()
    {
        reNameIcon.SetActive(false);
        reOnIcon.SetActive(true);
    }

    void OnReChat()
    {
        panelBtn.SetActive(true);
        tail.SetActive(true);
    }
}
