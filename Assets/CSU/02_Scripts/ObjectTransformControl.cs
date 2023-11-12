using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectTransformControl : MonoBehaviour
{
    public Transform objParent;

    Transform objTransformUI;
    Transform child;

    Slider positionX_Slider;
    Slider positionY_Slider;
    Slider positionZ_Slider;
    InputField positionX_IF;
    InputField positionY_IF;
    InputField positionZ_IF;

    Slider rotationSlider;
    InputField rotationIF;

    Slider scaleSlider;
    InputField scaleIF;

    private void Start()
    {
        ConnectUI();        // UI 연결
        AddEventUI();       // UI 리스너 추가
        InitializeValue();  // UI value 초기화
    }

    #region Awake 함수에서 설정하는 내용
    // UI 연결
    void ConnectUI()
    {
        objTransformUI = PublicObjectManager.instance.objectTransformUI;

        Transform positionPanel = objTransformUI.Find("Panel_Position");
        Transform rotationPanel = objTransformUI.Find("Panel_Rotation");
        Transform scalePanel = objTransformUI.Find("Panel_Scale");

        positionX_Slider = positionPanel.Find("PositionX").GetComponentInChildren<Slider>();
        positionY_Slider = positionPanel.Find("PositionY").GetComponentInChildren<Slider>();
        positionZ_Slider = positionPanel.Find("PositionZ").GetComponentInChildren<Slider>();
        positionX_IF = positionPanel.Find("PositionX").GetComponentInChildren<InputField>();
        positionY_IF = positionPanel.Find("PositionY").GetComponentInChildren<InputField>();
        positionZ_IF = positionPanel.Find("PositionZ").GetComponentInChildren<InputField>();

        rotationSlider = rotationPanel.GetComponentInChildren<Slider>();
        rotationIF = rotationPanel.GetComponentInChildren<InputField>();

        scaleSlider = scalePanel.GetComponentInChildren<Slider>();
        scaleIF = scalePanel.GetComponentInChildren<InputField>();
    }

    // UI 리스너 추가
    void AddEventUI()
    {
        positionX_Slider.onValueChanged.AddListener(delegate { OnValueChanged(positionX_Slider, "positionX"); });
        positionY_Slider.onValueChanged.AddListener(delegate { OnValueChanged(positionY_Slider, "positionY"); });
        positionZ_Slider.onValueChanged.AddListener(delegate { OnValueChanged(positionZ_Slider, "positionZ"); });
        positionX_IF.onValueChanged.AddListener(delegate { OnValueChanged(positionX_IF, "positionX"); });
        positionY_IF.onValueChanged.AddListener(delegate { OnValueChanged(positionY_IF, "positionY"); });
        positionZ_IF.onValueChanged.AddListener(delegate { OnValueChanged(positionZ_IF, "positionZ"); });

        rotationSlider.onValueChanged.AddListener(delegate { OnValueChanged(rotationSlider, "rotation"); });
        rotationIF.onValueChanged.AddListener(delegate { OnValueChanged(rotationIF, "rotation"); });

        scaleSlider.onValueChanged.AddListener(delegate { OnValueChanged(scaleSlider, "scale"); });
        scaleIF.onValueChanged.AddListener(delegate { OnValueChanged(scaleIF, "scale"); });
    }

    // Slider, InputFiled 초기값 설정
    void InitializeValue()
    {
        positionX_Slider.value = 0;
        positionY_Slider.value = 0;
        positionZ_Slider.value = 0;
        positionX_IF.text = "0";
        positionY_IF.text = "0";
        positionZ_IF.text = "0";

        rotationSlider.value = 0;
        rotationIF.text = "0";

        scaleSlider.value = 0;
        scaleIF.text = "0";
    }

    #endregion

    #region OnValueChanged
    // Slider 값이 변경 됐을 때 호출하는 함수
    void OnValueChanged(Slider slider, string type)
    {
        if (type.Contains("position"))
        {
            // 문자열의 마지막을 확인해서 X, Y, Z 분류
            char last = type[type.Length - 1];
            if (last == 'X') positionX_IF.text = positionX_Slider.value.ToString();
            else if (last == 'Y') positionY_IF.text = positionY_Slider.value.ToString();
            else if (last == 'Z') positionZ_IF.text = positionZ_Slider.value.ToString();
        }
        else if (type == "rotation")
        {
            rotationIF.text = rotationSlider.value.ToString();
        }
        else if (type == "scale")
        {
            scaleIF.text = scaleSlider.value.ToString();
        }
    }

    // InputField 값이 변경 됐을 때 호출하는 함수
    void OnValueChanged(InputField inputField, string type)
    {
        if (type.Contains("position"))
        {
            char last = type[type.Length - 1];
            if (last == 'X')
            {
                float f = 0;
                // InputField 의 첫 글자가 - 일 경우, 예외처리
                if (positionX_IF.text[0] == '-')
                {
                    if (positionX_IF.text.Length > 1)
                    {
                        f = float.Parse(positionX_IF.text);
                    }
                }
                else
                {
                    f = float.Parse(positionX_IF.text);
                }
                positionX_Slider.value = f;
            }
            else if (last == 'Y')
            {
                float f = 0;
                // InputField 의 첫 글자가 - 일 경우, 예외처리
                if (positionY_IF.text[0] == '-')
                {
                    if (positionY_IF.text.Length > 1)
                    {
                        f = float.Parse(positionY_IF.text);
                    }
                }
                else
                {
                    f = float.Parse(positionY_IF.text);
                }
                positionY_Slider.value = f;
            }
            else if (last == 'Z')
            {
                float f = 0;
                // InputField 의 첫 글자가 - 일 경우, 예외처리
                if (positionZ_IF.text[0] == '-')
                {
                    if (positionZ_IF.text.Length > 1)
                    {
                        f = float.Parse(positionZ_IF.text);
                    }
                }
                else
                {
                    f = float.Parse(positionZ_IF.text);
                }
                positionZ_Slider.value = f;
            }

            PositionChange(positionX_Slider.value, positionY_Slider.value, positionZ_Slider.value);
        }
        else if (type == "rotation")
        {
            float f = float.Parse(rotationIF.text);
            if (f > rotationSlider.maxValue) f = rotationSlider.maxValue;
            else if (f < rotationSlider.minValue) f = rotationSlider.minValue;
            rotationSlider.value = f;

            RotationChange(rotationSlider.value);
        }
        else if (type == "scale")
        {
            float f = float.Parse(scaleIF.text);
            if (f > scaleSlider.maxValue) f = scaleSlider.maxValue;
            else if (f < scaleSlider.minValue) f = scaleSlider.minValue;
            scaleSlider.value = f;

            ScaleChange(scaleSlider.value);
        }
    }
    #endregion

    #region Transform Change
    public void PositionChange(float x, float y, float z)
    {
        if (child != null)
        {
            child.position = new Vector3(x, y, z);
        }
    }

    public void RotationChange(float angle)
    {
        if (child != null)
        {
            child.eulerAngles = new Vector3(0, angle, 0);
        }
    }

    public void ScaleChange(float size)
    {
        if (child != null)
        {
            child.localScale = new Vector3(size, size, size);
        }
    }
    #endregion

    #region Reset 함수
    // Position Reset
    public void ResetPosition()
    {
        PositionChange(0f, 0f, 0f);
        SetPositionUI(new Vector3(0, 0, 0));
    }

    // Rotation Reset
    public void ResetRotation()
    {
        RotationChange(0f);
        SetRotationUI(0f);
    }

    // Scale Reset
    public void ResetScale()
    {
        ScaleChange(1f);
        SetScaleUI(1f);
    }
    #endregion

    #region Object 선택 시 해당 Object 의 Tranform 값으로 UI 변경
    public void SetPositionUI(Vector3 pos)
    {
        positionX_Slider.value = pos.x;
        positionY_Slider.value = pos.y;
        positionZ_Slider.value = pos.z;

        positionX_IF.text = positionX_Slider.value.ToString();
        positionY_IF.text = positionY_Slider.value.ToString();
        positionZ_IF.text = positionZ_Slider.value.ToString();
    }

    public void SetRotationUI(float angle)
    {
        rotationSlider.value = angle;
        rotationIF.text = rotationSlider.value.ToString();
    }

    public void SetScaleUI(float size)
    {
        scaleSlider.value = size;
        scaleIF.text = scaleSlider.value.ToString();
    }
    #endregion

    #region 자식(선택된 오브젝트) property
    public void MySetChild()
    {
        child = transform.GetChild(0);

        // UI 값 수정
        SetPositionUI(child.position);
        SetScaleUI(child.localScale.x);

        float angle = child.eulerAngles.y;
        if (angle > 180f)
        {
            angle -= 360f;
            SetRotationUI(angle);
        }
        else if (angle == -180f)
        {
            string s = "";
            s = angle.ToString();
            SetRotationUI(float.Parse(s));
        }
        else
        {
            SetRotationUI(angle);
        }
    }

    public Transform MyGetChild()
    {
        return child;
    }
    #endregion
}
