using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectInfo : ScriptableObject
{
    // 종류
    public string objType;
    // 이름
    public string objName;
    // 프리팹
    public GameObject objPrefab;
}

[Serializable]
public class CreatedInfo
{
    public GameObject go;
    public int idx;
}

[Serializable]
public class SaveJsonInfo
{
    public int idx;
    public Vector3 position;
    public float angle;
    public float scaleValue;
}

[Serializable]
public class ArrayJson
{
    public List<SaveJsonInfo> datas;
}