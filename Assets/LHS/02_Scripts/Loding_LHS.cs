using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loding_LHS : MonoBehaviour
{
    public static Loding_LHS instance;

    private void Awake()
    {
        //만약에 instance에 값이 없다면(HttpManager가 하나도 생성되지 않았다면)
        if (instance == null)
        {
            //instance에 나 자신을 넣는다.
            instance = this;

            //씬이 바껴도 파괴되지 않게 한다.
            DontDestroyOnLoad(gameObject);
        }
        //만약에 instance에 값이 있다면(이미 만들어진 HttpManager가 존재 한다면)
        else
        {
            print("중복으로 생성한다! 파괴하라!");
            //파괴하자
            Destroy(gameObject);
        }
    }
}
