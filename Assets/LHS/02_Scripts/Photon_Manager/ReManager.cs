using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 복원기능을 누르면 나무생성
// 나무 선택 시 복원AI랑 채팅가능

// 변경

// 복원기능을 누르면
// 파일 복원 / 데이터 복원 기능 생성
// 다시 누르면 닫을 수 있게
// 파일 복원 -> 파일 업로드 후 즉각복원 // 혼자만 있을 때 활성화 되어야 함
// 데이터 복원 -> 로딩 후 즉각 복원 // 둘이 있다가 혼자가 되었을 때 활성화 되어야 함
// 나무 생성 , 배경 바뀌고 
// 채팅 AI 복원 / 음성 AI 복원으로 가져가야 함

public class ReManager : MonoBehaviour
{
    // 복원 버튼
    public Button btn_Restore;

    // 파일, 데이터 복원 UI // 확인 필요 지금 public 지움
    GameObject panelBtn;
    GameObject tail;

    // 나무
    public GameObject tree;

    [Header("복원 활성 시")]
    //복원 버튼
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
        //만약 0번을 누른다면 복원기능 활성화
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
