using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomMood : MonoBehaviour
{
    [Header("< Ground >")]
    public List<GameObject> grounds = new List<GameObject>();
    public Material matGround;

    [Header("< Wall >")]
    public List<GameObject> walls = new List<GameObject>();
    public Material matWall;

    [Header("< Wall >")]
    public List<GameObject> wallsTwo = new List<GameObject>();
    public Material matWallTwo;

    [Header("< Skybox >")]
    public Material matSkybox;

    public GameObject btn_restore;
    public GameObject btn_restoreTwo;

    public IEnumerator IEChangMood()
    {
        yield return new WaitForSeconds(2f);

        // 땅 변환
        for (int i = 0; i < grounds.Count; ++i)
        {
            MeshRenderer mr = grounds[i].GetComponent<MeshRenderer>();
            mr.material = matGround;
        }
        // 벽 변환
        for (int i = 0; i < walls.Count; ++i)
        {
            MeshRenderer mr = walls[i].GetComponent<MeshRenderer>();
            mr.material = matWall;
        }
        // 벽 변환
        for (int i = 0; i < wallsTwo.Count; ++i)
        {
            MeshRenderer mr = wallsTwo[i].GetComponent<MeshRenderer>();
            mr.material = matWallTwo;
        }

        // skybox 변환
        RenderSettings.skybox = matSkybox;

        btn_restore.SetActive(false);
        btn_restoreTwo.SetActive(true);

        ReManager.instance.tree.SetActive(true);
    }

    public void ChangeMood()
    {
        StartCoroutine(IEChangMood());
    }
}