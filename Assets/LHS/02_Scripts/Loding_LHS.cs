using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loding_LHS : MonoBehaviour
{
    public static Loding_LHS instance;

    private void Awake()
    {
        //���࿡ instance�� ���� ���ٸ�(HttpManager�� �ϳ��� �������� �ʾҴٸ�)
        if (instance == null)
        {
            //instance�� �� �ڽ��� �ִ´�.
            instance = this;

            //���� �ٲ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(gameObject);
        }
        //���࿡ instance�� ���� �ִٸ�(�̹� ������� HttpManager�� ���� �Ѵٸ�)
        else
        {
            print("�ߺ����� �����Ѵ�! �ı��϶�!");
            //�ı�����
            Destroy(gameObject);
        }
    }
}
