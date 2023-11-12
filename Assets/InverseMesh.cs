using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMesh : MonoBehaviour
{
    MeshFilter mf;
    [SerializeField]
    int[] myTriangles;


    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        Mesh myMesh = mf.sharedMesh;
        myTriangles = myMesh.GetTriangles(0);

        for (int i = 0; i < myTriangles.Length; i += 3)
        {
            int temp = myTriangles[i];
            myTriangles[i] = myTriangles[i + 2];
            myTriangles[i + 2] = temp;
        }
        myMesh.SetTriangles(myTriangles, 0);

        myMesh.RecalculateBounds();
        myMesh.RecalculateTangents();
        myMesh.RecalculateNormals();

        mf.mesh = myMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
