using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    private void Init()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void CreateArc(Transform Owner,float radius, float angle, int split = 20)
    {
        Init();

        Vector3 lenVec = radius * transform.InverseTransformVector(Owner.forward);
        float absAngle = Mathf.Abs(angle / 2);
        float dAngle = 2 * absAngle / split;

        Vector3 origin = new Vector3(0, 0, 0);

        vertices.Clear();
        vertices.Add(origin);
        for (int i = 0; i <= split; i++)
        {
            vertices.Add(origin + Quaternion.AngleAxis(absAngle - i * dAngle, Owner.up) * lenVec);
        }

        triangles.Clear();
        for (int i = 0; i < split; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    public void ClearMesh()
    {
        mesh.Clear();
    }
}
